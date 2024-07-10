using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using NijhofAddIn.Revit.Commands.Prefab.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NijhofAddIn.Revit.Commands.Prefab
{
    #region 2.5mm
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class MVtag25 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            View initialActiveView = doc.ActiveView;  /// Bewaar de initiële view

            if (!(initialActiveView is ViewSheet))
            {
                TaskDialog.Show("Error", "Deze functie werkt alleen in een sheet. Zorg ervoor dat je uit de viewport bent.");
                return Result.Cancelled;
            }

            try
            {
                Reference reference = uidoc.Selection.PickObject(ObjectType.Element, new ESF(), "Selecteer een viewport");
                Element viewElement = doc.GetElement(reference);
                if (!(viewElement is Viewport viewport))
                {
                    TaskDialog.Show("Error", "Selecteer een geldige viewport.");
                    return Result.Cancelled;
                }

                if (!(doc.GetElement(viewport.ViewId) is View activeView) || activeView.ViewType == ViewType.Legend || activeView.ViewType == ViewType.ThreeD)
                {
                    TaskDialog.Show("Error", "Selecteer een geschikte floorplan.");
                    return Result.Cancelled;
                }

                using (Transaction tagTransaction = new Transaction(doc, "NT - Prefab taggen"))
                {
                    tagTransaction.Start();

                    List<Element> filterDucts = new FilteredElementCollector(doc, activeView.Id)
                        .OfCategory(BuiltInCategory.OST_DuctCurves)
                        .WhereElementIsNotElementType()
                        .WherePasses(new ElementClassFilter(typeof(Duct)))
                        .ToElements()
                        .ToList();

                    if (filterDucts.Count == 0)
                    {
                        TaskDialog.Show("Error", "Geen prefab onderdelen gevonden in de view.");
                        return Result.Cancelled;
                    }

                    IList<ElementId> elementsToHide = new List<ElementId>();
                    IList<Element> elementsToBeTagged = new List<Element>();

                    foreach (Duct elem in filterDucts.Cast<Duct>())
                    {
                        /// If Duct is verticaal, dan toevoegen aan hide list anders aan tag list
                        var location = elem.Location as LocationCurve;
                        var direction = location.Curve.GetEndPoint(1) - location.Curve.GetEndPoint(0);
                        direction = direction.Normalize();

                        if (direction.Z > 0.8 || direction.Z < -0.8)
                        {
                            elementsToHide.Add(elem.Id as ElementId);
                        }
                        else
                        {
                            elementsToBeTagged.Add(elem as Element);
                        }

                    }

                    /// Filters voor Family en Type
                    FilteredElementCollector tagCollector = new FilteredElementCollector(doc)
                        .OfClass(typeof(FamilySymbol))
                        .OfCategory(BuiltInCategory.OST_DuctTags);
                    FilterRule familyRule = ParameterFilterRuleFactory.CreateEqualsRule(new ElementId(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM), "VE_Duct_Tag_Nijhof");
                    ElementParameterFilter familyFilter = new ElementParameterFilter(familyRule);
                    FilterRule typeRule = ParameterFilterRuleFactory.CreateEqualsRule(new ElementId(BuiltInParameter.SYMBOL_NAME_PARAM), "Length 2.5 mm");
                    ElementParameterFilter typeFilter = new ElementParameterFilter(typeRule);

                    /// Combineer de filters en pas ze toe
                    LogicalAndFilter combinedFilter = new LogicalAndFilter(familyFilter, typeFilter);
                    List<Element> filteredTags = tagCollector.WherePasses(combinedFilter).ToElements().ToList();

                    if (!(filteredTags.FirstOrDefault() is FamilySymbol selectedDuctTag))
                    {
                        TaskDialog.Show("Error", "Tag niet gevonden. Laad de tag en probeer het opnieuw");
                        return Result.Cancelled;
                    }

                    /// Verberg alle elementen in de lijst van te verbergen elementen
                    if (elementsToHide.Count > 0)
                    {
                        activeView.HideElements(elementsToHide);
                    }

                    foreach (Element duct in elementsToBeTagged)
                    {
                        LocationCurve ductCurve = duct.Location as LocationCurve;
                        XYZ midpoint = (ductCurve.Curve.GetEndPoint(0) + ductCurve.Curve.GetEndPoint(1)) / 2;
                        XYZ tagPoint = new XYZ(midpoint.X, midpoint.Y, activeView.GenLevel.Elevation);
                        XYZ tagOffset = new XYZ();

                        if (!selectedDuctTag.IsActive)
                            selectedDuctTag.Activate();

                        IndependentTag.Create(
                            doc, selectedDuctTag.Id, activeView.Id, new Reference(duct), true, TagOrientation.Horizontal, tagPoint);
                    }

                    tagTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return Result.Failed;
            }
            finally
            {
                // Herstel de initiële view
                if (uidoc.ActiveView.Id != initialActiveView.Id)
                {
                    uidoc.ActiveView = initialActiveView;
                }
            }

            return Result.Succeeded;
        }
    }
    #endregion

    #region 3.5mm
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class MVtag35 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            View initialActiveView = doc.ActiveView;  /// Bewaar de initiële view

            if (!(initialActiveView is ViewSheet))
            {
                TaskDialog.Show("Error", "Deze functie werkt alleen in een sheet. Zorg ervoor dat je uit de viewport bent.");
                return Result.Cancelled;
            }

            try
            {
                Reference reference = uidoc.Selection.PickObject(ObjectType.Element, new ESF(), "Selecteer een viewport");
                Element viewElement = doc.GetElement(reference);
                if (!(viewElement is Viewport viewport))
                {
                    TaskDialog.Show("Error", "Selecteer een geldige viewport.");
                    return Result.Cancelled;
                }

                if (!(doc.GetElement(viewport.ViewId) is View activeView) || activeView.ViewType == ViewType.Legend || activeView.ViewType == ViewType.ThreeD)
                {
                    TaskDialog.Show("Error", "Selecteer een geschikte floorplan.");
                    return Result.Cancelled;
                }

                using (Transaction tagTransaction = new Transaction(doc, "NT - Prefab taggen"))
                {
                    tagTransaction.Start();

                    List<Element> filterDucts = new FilteredElementCollector(doc, activeView.Id)
                        .OfCategory(BuiltInCategory.OST_DuctCurves)
                        .WhereElementIsNotElementType()
                        .WherePasses(new ElementClassFilter(typeof(Duct)))
                        .ToElements()
                        .ToList();

                    if (filterDucts.Count == 0)
                    {
                        TaskDialog.Show("Error", "Geen prefab onderdelen gevonden in de view.");
                        return Result.Cancelled;
                    }

                    IList<ElementId> elementsToHide = new List<ElementId>();
                    IList<Element> elementsToBeTagged = new List<Element>();

                    foreach (Duct elem in filterDucts.Cast<Duct>())
                    {
                        /// If Duct is verticaal, dan toevoegen aan hide list anders aan tag list
                        var location = elem.Location as LocationCurve;
                        var direction = location.Curve.GetEndPoint(1) - location.Curve.GetEndPoint(0);
                        direction = direction.Normalize();

                        if (direction.Z > 0.8 || direction.Z < -0.8)
                        {
                            elementsToHide.Add(elem.Id as ElementId);
                        }
                        else
                        {
                            elementsToBeTagged.Add(elem as Element);
                        }

                    }

                    /// Filters voor Family en Type
                    FilteredElementCollector tagCollector = new FilteredElementCollector(doc)
                        .OfClass(typeof(FamilySymbol))
                        .OfCategory(BuiltInCategory.OST_DuctTags);
                    FilterRule familyRule = ParameterFilterRuleFactory.CreateEqualsRule(new ElementId(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM), "VE_Duct_Tag_Nijhof");
                    ElementParameterFilter familyFilter = new ElementParameterFilter(familyRule);
                    FilterRule typeRule = ParameterFilterRuleFactory.CreateEqualsRule(new ElementId(BuiltInParameter.SYMBOL_NAME_PARAM), "Length 3.5 mm");
                    ElementParameterFilter typeFilter = new ElementParameterFilter(typeRule);

                    /// Combineer de filters en pas ze toe
                    LogicalAndFilter combinedFilter = new LogicalAndFilter(familyFilter, typeFilter);
                    List<Element> filteredTags = tagCollector.WherePasses(combinedFilter).ToElements().ToList();

                    if (!(filteredTags.FirstOrDefault() is FamilySymbol selectedDuctTag))
                    {
                        TaskDialog.Show("Error", "Tag niet gevonden. Laad de tag en probeer het opnieuw");
                        return Result.Cancelled;
                    }

                    /// Verberg alle elementen in de lijst van te verbergen elementen
                    if (elementsToHide.Count > 0)
                    {
                        activeView.HideElements(elementsToHide);
                    }

                    foreach (Element duct in elementsToBeTagged)
                    {
                        LocationCurve ductCurve = duct.Location as LocationCurve;
                        XYZ midpoint = (ductCurve.Curve.GetEndPoint(0) + ductCurve.Curve.GetEndPoint(1)) / 2;
                        XYZ tagPoint = new XYZ(midpoint.X, midpoint.Y, activeView.GenLevel.Elevation);
                        XYZ tagOffset = new XYZ();

                        if (!selectedDuctTag.IsActive)
                            selectedDuctTag.Activate();

                        IndependentTag.Create(
                            doc, selectedDuctTag.Id, activeView.Id, new Reference(duct), true, TagOrientation.Horizontal, tagPoint);
                    }

                    tagTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return Result.Failed;
            }
            finally
            {
                // Herstel de initiële view
                if (uidoc.ActiveView.Id != initialActiveView.Id)
                {
                    uidoc.ActiveView = initialActiveView;
                }
            }

            return Result.Succeeded;
        }
    }
    #endregion

    #region 5.0mm
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class MVtag50 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            View initialActiveView = doc.ActiveView;  /// Bewaar de initiële view

            if (!(initialActiveView is ViewSheet))
            {
                TaskDialog.Show("Error", "Deze functie werkt alleen in een sheet. Zorg ervoor dat je uit de viewport bent.");
                return Result.Cancelled;
            }

            try
            {
                Reference reference = uidoc.Selection.PickObject(ObjectType.Element, new ESF(), "Selecteer een viewport");
                Element viewElement = doc.GetElement(reference);
                if (!(viewElement is Viewport viewport))
                {
                    TaskDialog.Show("Error", "Selecteer een geldige viewport.");
                    return Result.Cancelled;
                }

                if (!(doc.GetElement(viewport.ViewId) is View activeView) || activeView.ViewType == ViewType.Legend || activeView.ViewType == ViewType.ThreeD)
                {
                    TaskDialog.Show("Error", "Selecteer een geschikte floorplan.");
                    return Result.Cancelled;
                }

                using (Transaction tagTransaction = new Transaction(doc, "NT - Prefab taggen"))
                {
                    tagTransaction.Start();

                    List<Element> filterDucts = new FilteredElementCollector(doc, activeView.Id)
                        .OfCategory(BuiltInCategory.OST_DuctCurves)
                        .WhereElementIsNotElementType()
                        .WherePasses(new ElementClassFilter(typeof(Duct)))
                        .ToElements()
                        .ToList();

                    if (filterDucts.Count == 0)
                    {
                        TaskDialog.Show("Error", "Geen prefab onderdelen gevonden in de view.");
                        return Result.Cancelled;
                    }

                    IList<ElementId> elementsToHide = new List<ElementId>();
                    IList<Element> elementsToBeTagged = new List<Element>();

                    foreach (Duct elem in filterDucts.Cast<Duct>())
                    {
                        /// If Duct is verticaal, dan toevoegen aan hide list anders aan tag list
                        var location = elem.Location as LocationCurve;
                        var direction = location.Curve.GetEndPoint(1) - location.Curve.GetEndPoint(0);
                        direction = direction.Normalize();

                        if (direction.Z > 0.8 || direction.Z < -0.8)
                        {
                            elementsToHide.Add(elem.Id as ElementId);
                        }
                        else
                        {
                            elementsToBeTagged.Add(elem as Element);
                        }

                    }

                    /// Filters voor Family en Type
                    FilteredElementCollector tagCollector = new FilteredElementCollector(doc)
                        .OfClass(typeof(FamilySymbol))
                        .OfCategory(BuiltInCategory.OST_DuctTags);
                    FilterRule familyRule = ParameterFilterRuleFactory.CreateEqualsRule(new ElementId(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM), "VE_Duct_Tag_Nijhof");
                    ElementParameterFilter familyFilter = new ElementParameterFilter(familyRule);
                    FilterRule typeRule = ParameterFilterRuleFactory.CreateEqualsRule(new ElementId(BuiltInParameter.SYMBOL_NAME_PARAM), "Length 5 mm");
                    ElementParameterFilter typeFilter = new ElementParameterFilter(typeRule);

                    /// Combineer de filters en pas ze toe
                    LogicalAndFilter combinedFilter = new LogicalAndFilter(familyFilter, typeFilter);
                    List<Element> filteredTags = tagCollector.WherePasses(combinedFilter).ToElements().ToList();

                    if (!(filteredTags.FirstOrDefault() is FamilySymbol selectedDuctTag))
                    {
                        TaskDialog.Show("Error", "Tag niet gevonden. Laad de tag en probeer het opnieuw");
                        return Result.Cancelled;
                    }

                    /// Verberg alle elementen in de lijst van te verbergen elementen
                    if (elementsToHide.Count > 0)
                    {
                        activeView.HideElements(elementsToHide);
                    }

                    foreach (Element duct in elementsToBeTagged)
                    {
                        LocationCurve ductCurve = duct.Location as LocationCurve;
                        XYZ midpoint = (ductCurve.Curve.GetEndPoint(0) + ductCurve.Curve.GetEndPoint(1)) / 2;
                        XYZ tagPoint = new XYZ(midpoint.X, midpoint.Y, activeView.GenLevel.Elevation);
                        XYZ tagOffset = new XYZ();

                        if (!selectedDuctTag.IsActive)
                            selectedDuctTag.Activate();

                        IndependentTag.Create(
                            doc, selectedDuctTag.Id, activeView.Id, new Reference(duct), true, TagOrientation.Horizontal, tagPoint);
                    }

                    tagTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return Result.Failed;
            }
            finally
            {
                // Herstel de initiële view
                if (uidoc.ActiveView.Id != initialActiveView.Id)
                {
                    uidoc.ActiveView = initialActiveView;
                }
            }

            return Result.Succeeded;
        }
    }
    #endregion

    #region 7.5mm
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class MVtag75 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            View initialActiveView = doc.ActiveView;  /// Bewaar de initiële view

            if (!(initialActiveView is ViewSheet))
            {
                TaskDialog.Show("Error", "Deze functie werkt alleen in een sheet. Zorg ervoor dat je uit de viewport bent.");
                return Result.Cancelled;
            }

            try
            {
                Reference reference = uidoc.Selection.PickObject(ObjectType.Element, new ESF(), "Selecteer een viewport");
                Element viewElement = doc.GetElement(reference);
                if (!(viewElement is Viewport viewport))
                {
                    TaskDialog.Show("Error", "Selecteer een geldige viewport.");
                    return Result.Cancelled;
                }

                if (!(doc.GetElement(viewport.ViewId) is View activeView) || activeView.ViewType == ViewType.Legend || activeView.ViewType == ViewType.ThreeD)
                {
                    TaskDialog.Show("Error", "Selecteer een geschikte floorplan.");
                    return Result.Cancelled;
                }

                using (Transaction tagTransaction = new Transaction(doc, "NT - Prefab taggen"))
                {
                    tagTransaction.Start();

                    List<Element> filterDucts = new FilteredElementCollector(doc, activeView.Id)
                        .OfCategory(BuiltInCategory.OST_DuctCurves)
                        .WhereElementIsNotElementType()
                        .WherePasses(new ElementClassFilter(typeof(Duct)))
                        .ToElements()
                        .ToList();

                    if (filterDucts.Count == 0)
                    {
                        TaskDialog.Show("Error", "Geen prefab onderdelen gevonden in de view.");
                        return Result.Cancelled;
                    }

                    IList<ElementId> elementsToHide = new List<ElementId>();
                    IList<Element> elementsToBeTagged = new List<Element>();

                    foreach (Duct elem in filterDucts.Cast<Duct>())
                    {
                        /// If Duct is verticaal, dan toevoegen aan hide list anders aan tag list
                        var location = elem.Location as LocationCurve;
                        var direction = location.Curve.GetEndPoint(1) - location.Curve.GetEndPoint(0);
                        direction = direction.Normalize();

                        if (direction.Z > 0.8 || direction.Z < -0.8)
                        {
                            elementsToHide.Add(elem.Id as ElementId);
                        }
                        else
                        {
                            elementsToBeTagged.Add(elem as Element);
                        }

                    }

                    /// Filters voor Family en Type
                    FilteredElementCollector tagCollector = new FilteredElementCollector(doc)
                        .OfClass(typeof(FamilySymbol))
                        .OfCategory(BuiltInCategory.OST_DuctTags);
                    FilterRule familyRule = ParameterFilterRuleFactory.CreateEqualsRule(new ElementId(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM), "VE_Duct_Tag_Nijhof");
                    ElementParameterFilter familyFilter = new ElementParameterFilter(familyRule);
                    FilterRule typeRule = ParameterFilterRuleFactory.CreateEqualsRule(new ElementId(BuiltInParameter.SYMBOL_NAME_PARAM), "Length 7.5 mm");
                    ElementParameterFilter typeFilter = new ElementParameterFilter(typeRule);

                    /// Combineer de filters en pas ze toe
                    LogicalAndFilter combinedFilter = new LogicalAndFilter(familyFilter, typeFilter);
                    List<Element> filteredTags = tagCollector.WherePasses(combinedFilter).ToElements().ToList();

                    if (!(filteredTags.FirstOrDefault() is FamilySymbol selectedDuctTag))
                    {
                        TaskDialog.Show("Error", "Tag niet gevonden. Laad de tag en probeer het opnieuw");
                        return Result.Cancelled;
                    }

                    /// Verberg alle elementen in de lijst van te verbergen elementen
                    if (elementsToHide.Count > 0)
                    {
                        activeView.HideElements(elementsToHide);
                    }

                    foreach (Element duct in elementsToBeTagged)
                    {
                        LocationCurve ductCurve = duct.Location as LocationCurve;
                        XYZ midpoint = (ductCurve.Curve.GetEndPoint(0) + ductCurve.Curve.GetEndPoint(1)) / 2;
                        XYZ tagPoint = new XYZ(midpoint.X, midpoint.Y, activeView.GenLevel.Elevation);
                        XYZ tagOffset = new XYZ();

                        if (!selectedDuctTag.IsActive)
                            selectedDuctTag.Activate();

                        IndependentTag.Create(
                            doc, selectedDuctTag.Id, activeView.Id, new Reference(duct), true, TagOrientation.Horizontal, tagPoint);
                    }

                    tagTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return Result.Failed;
            }
            finally
            {
                // Herstel de initiële view
                if (uidoc.ActiveView.Id != initialActiveView.Id)
                {
                    uidoc.ActiveView = initialActiveView;
                }
            }

            return Result.Succeeded;
        }
    }
    #endregion

    #region 10mm
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class MVtag100 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            View initialActiveView = doc.ActiveView;  /// Bewaar de initiële view

            if (!(initialActiveView is ViewSheet))
            {
                TaskDialog.Show("Error", "Deze functie werkt alleen in een sheet. Zorg ervoor dat je uit de viewport bent.");
                return Result.Cancelled;
            }

            try
            {
                Reference reference = uidoc.Selection.PickObject(ObjectType.Element, new ESF(), "Selecteer een viewport");
                Element viewElement = doc.GetElement(reference);
                if (!(viewElement is Viewport viewport))
                {
                    TaskDialog.Show("Error", "Selecteer een geldige viewport.");
                    return Result.Cancelled;
                }

                if (!(doc.GetElement(viewport.ViewId) is View activeView) || activeView.ViewType == ViewType.Legend || activeView.ViewType == ViewType.ThreeD)
                {
                    TaskDialog.Show("Error", "Selecteer een geschikte floorplan.");
                    return Result.Cancelled;
                }

                using (Transaction tagTransaction = new Transaction(doc, "NT - Prefab taggen"))
                {
                    tagTransaction.Start();

                    List<Element> filterDucts = new FilteredElementCollector(doc, activeView.Id)
                        .OfCategory(BuiltInCategory.OST_DuctCurves)
                        .WhereElementIsNotElementType()
                        .WherePasses(new ElementClassFilter(typeof(Duct)))
                        .ToElements()
                        .ToList();

                    if (filterDucts.Count == 0)
                    {
                        TaskDialog.Show("Error", "Geen prefab onderdelen gevonden in de view.");
                        return Result.Cancelled;
                    }

                    IList<ElementId> elementsToHide = new List<ElementId>();
                    IList<Element> elementsToBeTagged = new List<Element>();

                    foreach (Duct elem in filterDucts.Cast<Duct>())
                    {
                        /// If Duct is verticaal, dan toevoegen aan hide list anders aan tag list
                        var location = elem.Location as LocationCurve;
                        var direction = location.Curve.GetEndPoint(1) - location.Curve.GetEndPoint(0);
                        direction = direction.Normalize();

                        if (direction.Z > 0.8 || direction.Z < -0.8)
                        {
                            elementsToHide.Add(elem.Id as ElementId);
                        }
                        else
                        {
                            elementsToBeTagged.Add(elem as Element);
                        }

                    }

                    /// Filters voor Family en Type
                    FilteredElementCollector tagCollector = new FilteredElementCollector(doc)
                        .OfClass(typeof(FamilySymbol))
                        .OfCategory(BuiltInCategory.OST_DuctTags);
                    FilterRule familyRule = ParameterFilterRuleFactory.CreateEqualsRule(new ElementId(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM), "VE_Duct_Tag_Nijhof");
                    ElementParameterFilter familyFilter = new ElementParameterFilter(familyRule);
                    FilterRule typeRule = ParameterFilterRuleFactory.CreateEqualsRule(new ElementId(BuiltInParameter.SYMBOL_NAME_PARAM), "Length 10 mm");
                    ElementParameterFilter typeFilter = new ElementParameterFilter(typeRule);

                    /// Combineer de filters en pas ze toe
                    LogicalAndFilter combinedFilter = new LogicalAndFilter(familyFilter, typeFilter);
                    List<Element> filteredTags = tagCollector.WherePasses(combinedFilter).ToElements().ToList();


                    if (!(filteredTags.FirstOrDefault() is FamilySymbol selectedDuctTag))
                    {
                        TaskDialog.Show("Error", "Tag niet gevonden. Laad de tag en probeer het opnieuw");
                        return Result.Cancelled;
                    }

                    /// Verberg alle elementen in de lijst van te verbergen elementen
                    if (elementsToHide.Count > 0)
                    {
                        activeView.HideElements(elementsToHide);
                    }

                    foreach (Element duct in elementsToBeTagged)
                    {
                        LocationCurve ductCurve = duct.Location as LocationCurve;
                        XYZ midpoint = (ductCurve.Curve.GetEndPoint(0) + ductCurve.Curve.GetEndPoint(1)) / 2;
                        XYZ tagPoint = new XYZ(midpoint.X, midpoint.Y, activeView.GenLevel.Elevation);
                        XYZ tagOffset = new XYZ();

                        if (!selectedDuctTag.IsActive)
                            selectedDuctTag.Activate();

                        IndependentTag.Create(
                            doc, selectedDuctTag.Id, activeView.Id, new Reference(duct), true, TagOrientation.Horizontal, tagPoint);
                    }

                    tagTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return Result.Failed;
            }
            finally
            {
                // Herstel de initiële view
                if (uidoc.ActiveView.Id != initialActiveView.Id)
                {
                    uidoc.ActiveView = initialActiveView;
                }
            }

            return Result.Succeeded;
        }
    }
    #endregion
}
