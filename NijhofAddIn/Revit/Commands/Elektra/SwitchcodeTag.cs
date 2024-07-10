using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NijhofAddIn.Revit.Commands.Elektra
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class SwitchcodeTag : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            Document doc = uiApp.ActiveUIDocument.Document;
            View activeView = doc.ActiveView;

            try
            {
                List<BuiltInCategory> categories = new List<BuiltInCategory>
                {
                    BuiltInCategory.OST_ElectricalFixtures,
                    BuiltInCategory.OST_LightingFixtures,
                    BuiltInCategory.OST_LightingDevices,
                    BuiltInCategory.OST_FireAlarmDevices,
                };

                ElementMulticategoryFilter categoryFilter = new ElementMulticategoryFilter(categories);
                FilteredElementCollector collector = new FilteredElementCollector(doc).WherePasses(categoryFilter);

                List<Element> elementsWithSwitchCode = new List<Element>();

                foreach (Element elem in collector)
                {
                    Parameter switchParam = elem.LookupParameter("Switch code"); /// Project parameter
                    Parameter switchCodeParam = elem.LookupParameter("Switchcode"); /// Family parameter

                    if ((switchParam != null && switchParam.HasValue && !string.IsNullOrWhiteSpace(switchParam.AsString())) ||
                        (switchCodeParam != null && switchCodeParam.HasValue && !string.IsNullOrWhiteSpace(switchCodeParam.AsString())))
                    {
                        elementsWithSwitchCode.Add(elem);
                    }
                }

                /// Method to load tags if they are not present
                FamilySymbol LoadTag(Document document, string familyName, BuiltInCategory tagCategory, string filePath)
                {
                    /// Filters to find the correct tag
                    FilteredElementCollector tagCollector = new FilteredElementCollector(document)
                        .OfClass(typeof(FamilySymbol))
                        .OfCategory(tagCategory);
                    FamilySymbol tag = tagCollector
                        .Cast<FamilySymbol>()
                        .FirstOrDefault(t => t.FamilyName.Equals(familyName, StringComparison.OrdinalIgnoreCase));

                    if (tag == null)
                    {
                        /// Load the family and check if it is correctly loaded
                        Family family;
                        using (Transaction transaction = new Transaction(document, "Load Family"))
                        {
                            transaction.Start();
                            bool loaded = document.LoadFamily(filePath, out family);
                            transaction.Commit();

                            if (!loaded || family == null)
                            {
                                throw new InvalidOperationException($"Could not load the family '{familyName}' from '{filePath}'.");
                            }
                        }

                        /// Find the correct FamilySymbol within the loaded Family
                        tag = new FilteredElementCollector(document)
                            .OfClass(typeof(FamilySymbol))
                            .Cast<FamilySymbol>()
                            .FirstOrDefault(t => t.FamilyName.Equals(familyName, StringComparison.OrdinalIgnoreCase));

                        if (tag == null)
                        {
                            throw new InvalidOperationException($"Could not find the symbol '{familyName}' in the loaded family.");
                        }

                        /// Activate the symbol
                        using (Transaction transaction = new Transaction(document, "Activate Symbol"))
                        {
                            transaction.Start();
                            tag.Activate();
                            document.Regenerate();
                            transaction.Commit();
                        }
                    }

                    return tag;
                }

                string multiCategoryTagPath = @"F:\Stabiplan\Custom\Families\Tags\Electrical\Switchcode Tag.rfa";

                FamilySymbol MCtagST = LoadTag(doc, "Switchcode Tag", BuiltInCategory.OST_MultiCategoryTags, multiCategoryTagPath);

                /// Method to check if a specific tag is already on the element
                bool ElementHasSpecificTag(Document document, Element elem, View view, ElementId tagSymbolId)
                {
                    var tags = new FilteredElementCollector(document, view.Id)
                        .OfClass(typeof(IndependentTag))
                        .OfCategory(BuiltInCategory.OST_MultiCategoryTags)
                        .WhereElementIsNotElementType()
                        .Cast<IndependentTag>()
                        .Where(t => t.GetTaggedLocalElementIds().Contains(elem.Id) && t.GetTypeId() == tagSymbolId);

                    return tags.Any();
                }

                /// Method to check if an element has either the "Switch Code - Electrical Fixture" tag or the "Switch Code - Lighting Device" tag
                bool ElementHasRelevantTag(Document document, Element elem)
                {

                    // Define the family names of the relevant tags
                    string electricalFixtureTagFamilyName = "Switch Code - Electrical Fixture";
                    string lightingDeviceTagFamilyName = "Switch Code - Lighting Device";

                    var electricalFixtureTags = new FilteredElementCollector(document)
                        .OfClass(typeof(IndependentTag))
                        .OfCategory(BuiltInCategory.OST_ElectricalFixtureTags)
                        .WhereElementIsNotElementType()
                        .Cast<IndependentTag>()
                        .Where(t => t.GetTaggedLocalElementIds().Contains(elem.Id))
                        .Where(t => document.GetElement(t.GetTypeId()).Name == electricalFixtureTagFamilyName);

                    var lightingDeviceTags = new FilteredElementCollector(document)
                        .OfClass(typeof(IndependentTag))
                        .OfCategory(BuiltInCategory.OST_LightingDeviceTags)
                        .WhereElementIsNotElementType()
                        .Cast<IndependentTag>()
                        .Where(t => t.GetTaggedLocalElementIds().Contains(elem.Id))
                        .Where(t => document.GetElement(t.GetTypeId()).Name == lightingDeviceTagFamilyName);

                    return electricalFixtureTags.Any() || lightingDeviceTags.Any();
                }

                /// Get the hand orientation vector of the element
                XYZ GetHandOrientationVector(Element elem)
                {
                    dynamic handOrientationProperty = elem.GetType().GetProperty("HandOrientation")?.GetValue(elem, null);
                    if (handOrientationProperty != null)
                    {
                        return new XYZ(handOrientationProperty.X, handOrientationProperty.Y, handOrientationProperty.Z);
                    }
                    return null;
                }

                /// Calculate the tag point based on location and rotation
                XYZ CalculateTagPoint(Element elem)
                {
                    LocationPoint locPoint = elem.Location as LocationPoint;
                    if (locPoint == null)
                        return null;

                    XYZ handOrientation = GetHandOrientationVector(elem);

                    /// Determine the offset based on "HandOrientation" value
                    XYZ offset = null;
                    if (handOrientation != null)
                    {
                        if (handOrientation.IsAlmostEqualTo(new XYZ(0, -1, 0)))
                        {
                            offset = new XYZ(-1.2, 0, 0); /// Offset for "Left"
                        }
                        else if (handOrientation.IsAlmostEqualTo(new XYZ(0, 1, 0)))
                        {
                            offset = new XYZ(1.2, 0, 0); /// Offset for "Right"
                        }
                        else if (handOrientation.IsAlmostEqualTo(new XYZ(-1, 0, 0)))
                        {
                            offset = new XYZ(0, 1.2, 0); /// Offset for "Up"
                        }
                        else if (handOrientation.IsAlmostEqualTo(new XYZ(1, 0, 0)))
                        {
                            offset = new XYZ(0, -1.2, 0); /// Offset for "Down"
                        }
                    }

                    if (offset == null)
                        return null; /// Return null if HandOrientation doesn't match known values

                    XYZ tagPoint = locPoint.Point + offset;
                    return tagPoint;
                }

                /// Start a transaction to tag elements
                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start("Tag Electrical Components");

                    foreach (Element elem in elementsWithSwitchCode)
                    {
                        XYZ tagPoint = CalculateTagPoint(elem);

                        if (tagPoint == null)
                        {
                            continue;
                        }

#if REVIT2023
                        if (categories.Contains((BuiltInCategory)elem.Category.Id.IntegerValue))
#elif REVIT2024 || REVIT2025
                        if (categories.Contains((BuiltInCategory)elem.Category.Id.Value))
#endif
                        {
                            if (!MCtagST.IsActive)
                            {
                                MCtagST.Activate();
                                doc.Regenerate();
                            }

                            /// Check if the element already has the relevant tag
                            if (ElementHasRelevantTag(doc, elem))
                            {
                                continue;
                            }

                            if (!ElementHasSpecificTag(doc, elem, activeView, MCtagST.Id))
                            {
                                IndependentTag.Create(
                                    doc, MCtagST.Id, activeView.Id, new Reference(elem), false, TagOrientation.Horizontal, tagPoint);
                            }
                        }
                    }

                    tx.Commit();
                }

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }
}
