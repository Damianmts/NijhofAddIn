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
    internal class GroepTag : IExternalCommand
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

                List<Element> elementsWithGroepNummer = new List<Element>();

                foreach (Element elem in collector)
                {
                    Parameter GroepNumParam = elem.LookupParameter("Groep nummer");
                    Parameter GroepParam = elem.LookupParameter("Groep");

                    if ((GroepNumParam != null && GroepNumParam.HasValue && !string.IsNullOrWhiteSpace(GroepNumParam.AsString())) ||
                        (GroepParam != null && GroepParam.HasValue && !string.IsNullOrWhiteSpace(GroepParam.AsString())))
                    {
                        elementsWithGroepNummer.Add(elem);
                    }
                }

                FamilySymbol LoadTag(Document docu, string familyName, BuiltInCategory tagCategory, string filePath)
                {
                    FilteredElementCollector tagCollector = new FilteredElementCollector(docu)
                        .OfClass(typeof(FamilySymbol))
                        .OfCategory(tagCategory);
                    FamilySymbol tag = tagCollector
                        .Cast<FamilySymbol>()
                        .FirstOrDefault(t => t.FamilyName.Equals(familyName, StringComparison.OrdinalIgnoreCase));

                    if (tag == null)
                    {
                        Family family;
                        using (Transaction transaction = new Transaction(docu, "Load Family"))
                        {
                            transaction.Start();
                            bool loaded = docu.LoadFamily(filePath, out family);
                            transaction.Commit();

                            if (!loaded || family == null)
                            {
                                throw new InvalidOperationException($"Kon de familie '{familyName}' niet laden uit '{filePath}'.");
                            }
                        }

                        tag = new FilteredElementCollector(docu)
                            .OfClass(typeof(FamilySymbol))
                            .Cast<FamilySymbol>()
                            .FirstOrDefault(t => t.FamilyName.Equals(familyName, StringComparison.OrdinalIgnoreCase));

                        if (tag == null)
                        {
                            throw new InvalidOperationException($"Kon het symbool '{familyName}' niet vinden in de geladen familie.");
                        }

                        using (Transaction transaction = new Transaction(docu, "Activate Symbol"))
                        {
                            transaction.Start();
                            tag.Activate();
                            docu.Regenerate();
                            transaction.Commit();
                        }
                    }

                    return tag;
                }

                string multiCategoryTagPath = @"F:\Stabiplan\Custom\Families\Tags\Electrical\Groep Tag.rfa";

                FamilySymbol MCtagGT = LoadTag(doc, "Groep Tag", BuiltInCategory.OST_MultiCategoryTags, multiCategoryTagPath);

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

                bool ElementHasRelevantTag(Document document, Element elem)
                {
                    string electricalFixtureTagFamilyName = "Groep Nummer - Electrical Fixture";
                    string lightingDeviceTagFamilyName = "Groep Nummer - Lighting Device";
                    string FireAlarmDeviceTagFamilyName = "Groep Nummer - Fire Alarm Device";

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

                    var FireAlarmDeviceTags = new FilteredElementCollector(document)
                        .OfClass(typeof(IndependentTag))
                        .OfCategory(BuiltInCategory.OST_FireAlarmDeviceTags)
                        .WhereElementIsNotElementType()
                        .Cast<IndependentTag>()
                        .Where(t => t.GetTaggedLocalElementIds().Contains(elem.Id))
                        .Where(t => document.GetElement(t.GetTypeId()).Name == FireAlarmDeviceTagFamilyName);

                    return electricalFixtureTags.Any() || lightingDeviceTags.Any() || FireAlarmDeviceTags.Any();
                }

                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start("Tag Electrical Components");

                    foreach (Element elem in elementsWithGroepNummer)
                    {
                        XYZ tagPoint = null;
                        Location loc = elem.Location;
                        if (loc is LocationPoint locPoint)
                        {
                            tagPoint = locPoint.Point;
                        }

                        if (tagPoint == null)
                        {
                            continue;
                        }

#if REVIT2023
                        if (categories.Contains((BuiltInCategory)elem.Category.Id.IntegerValue))
                        {
                            if (!MCtagGT.IsActive)
                            {
                                MCtagGT.Activate();
                                doc.Regenerate();
                            }
#elif REVIT2024 || REVIT2025
                        if (categories.Contains((BuiltInCategory)elem.Category.Id.Value))
                        {
                            if (!MCtagGT.IsActive)
                            {
                                MCtagGT.Activate();
                                doc.Regenerate();
                            }
                        
#endif

                        if (ElementHasRelevantTag(doc, elem))
                            {
                                continue;
                            }

                            if (!ElementHasSpecificTag(doc, elem, activeView, MCtagGT.Id))
                            {
                                IndependentTag.Create(
                                    doc, MCtagGT.Id, activeView.Id, new Reference(elem), false, TagOrientation.Horizontal, tagPoint);
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
