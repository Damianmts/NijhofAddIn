using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Plumbing;
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
    public class VWAtag25 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            View initialActiveView = doc.ActiveView;  // Bewaar de initiële view

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

                    List<Element> filterPipes = new FilteredElementCollector(doc, activeView.Id)
                        .OfCategory(BuiltInCategory.OST_PipeCurves)
                        .WhereElementIsNotElementType()
                        .WherePasses(new ElementClassFilter(typeof(Pipe)))
                        .WherePasses(new ElementParameterFilter(
                            new FilterStringRule(new ParameterValueProvider(new ElementId(BuiltInParameter.RBS_SYSTEM_CLASSIFICATION_PARAM)),
                            new FilterStringEquals(),
                            "Sanitary")))
                        .ToElements()
                        .ToList();

                    if (filterPipes.Count == 0)
                    {
                        TaskDialog.Show("Error", "Geen prefab onderdelen gevonden in de view.");
                        return Result.Cancelled;
                    }

                    IList<ElementId> elementsToHide = new List<ElementId>();
                    IList<Element> elementsToBeTagged = new List<Element>();

                    foreach (Pipe elem in filterPipes.Cast<Pipe>())
                    {
                        /// If Pipe is verticaal, dan toevoegen aan hide list anders aan tag list
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
                        .OfCategory(BuiltInCategory.OST_PipeTags);
                    FilterRule familyRule = ParameterFilterRuleFactory.CreateEqualsRule(new ElementId(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM), "M_Pipe_Tag_Nijhof");
                    ElementParameterFilter familyFilter = new ElementParameterFilter(familyRule);
                    FilterRule typeRule = ParameterFilterRuleFactory.CreateEqualsRule(new ElementId(BuiltInParameter.SYMBOL_NAME_PARAM), "Length 2.5 mm");
                    ElementParameterFilter typeFilter = new ElementParameterFilter(typeRule);

                    /// Combineer de filters en pas ze toe
                    LogicalAndFilter combinedFilter = new LogicalAndFilter(familyFilter, typeFilter);
                    List<Element> filteredTags = tagCollector.WherePasses(combinedFilter).ToElements().ToList();

                    if (!(filteredTags.FirstOrDefault() is FamilySymbol selectedPipeTag))
                    {
                        TaskDialog.Show("Error", "Tag niet gevonden. Laad de tag en probeer het opnieuw");
                        return Result.Cancelled;
                    }

                    /// Verberg alle elementen in de lijst van te verbergen elementen
                    if (elementsToHide.Count > 0)
                    {
                        activeView.HideElements(elementsToHide);
                    }

                    foreach (Element pipe in elementsToBeTagged)
                    {
                        LocationCurve pipeCurve = pipe.Location as LocationCurve;
                        XYZ midpoint = (pipeCurve.Curve.GetEndPoint(0) + pipeCurve.Curve.GetEndPoint(1)) / 2;
                        XYZ tagPoint = new XYZ(midpoint.X, midpoint.Y, activeView.GenLevel.Elevation);
                        XYZ tagOffset = new XYZ();

                        if (!selectedPipeTag.IsActive)
                            selectedPipeTag.Activate();

                        IndependentTag.Create(
                            doc, selectedPipeTag.Id, activeView.Id, new Reference(pipe), true, TagOrientation.Horizontal, tagPoint);
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
    public class VWAtag35 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            View initialActiveView = doc.ActiveView;  // Bewaar de initiële view

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

                    List<Element> filterPipes = new FilteredElementCollector(doc, activeView.Id)
                        .OfCategory(BuiltInCategory.OST_PipeCurves)
                        .WhereElementIsNotElementType()
                        .WherePasses(new ElementClassFilter(typeof(Pipe)))
                        .WherePasses(new ElementParameterFilter(
                            new FilterStringRule(new ParameterValueProvider(new ElementId(BuiltInParameter.RBS_SYSTEM_CLASSIFICATION_PARAM)),
                            new FilterStringEquals(),
                            "Sanitary")))
                        .ToElements()
                        .ToList();

                    if (filterPipes.Count == 0)
                    {
                        TaskDialog.Show("Error", "Geen prefab onderdelen gevonden in de view.");
                        return Result.Cancelled;
                    }

                    IList<ElementId> elementsToHide = new List<ElementId>();
                    IList<Element> elementsToBeTagged = new List<Element>();

                    foreach (Pipe elem in filterPipes.Cast<Pipe>())
                    {
                        /// If Pipe is verticaal, dan toevoegen aan hide list anders aan tag list
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
                        .OfCategory(BuiltInCategory.OST_PipeTags);
                    FilterRule familyRule = ParameterFilterRuleFactory.CreateEqualsRule(new ElementId(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM), "M_Pipe_Tag_Nijhof");
                    ElementParameterFilter familyFilter = new ElementParameterFilter(familyRule);
                    FilterRule typeRule = ParameterFilterRuleFactory.CreateEqualsRule(new ElementId(BuiltInParameter.SYMBOL_NAME_PARAM), "Length 3.5 mm");
                    ElementParameterFilter typeFilter = new ElementParameterFilter(typeRule);

                    /// Combineer de filters en pas ze toe
                    LogicalAndFilter combinedFilter = new LogicalAndFilter(familyFilter, typeFilter);
                    List<Element> filteredTags = tagCollector.WherePasses(combinedFilter).ToElements().ToList();

                    if (!(filteredTags.FirstOrDefault() is FamilySymbol selectedPipeTag))
                    {
                        TaskDialog.Show("Error", "Tag niet gevonden. Laad de tag en probeer het opnieuw");
                        return Result.Cancelled;
                    }

                    /// Verberg alle elementen in de lijst van te verbergen elementen
                    if (elementsToHide.Count > 0)
                    {
                        activeView.HideElements(elementsToHide);
                    }

                    foreach (Element pipe in elementsToBeTagged)
                    {
                        LocationCurve pipeCurve = pipe.Location as LocationCurve;
                        XYZ midpoint = (pipeCurve.Curve.GetEndPoint(0) + pipeCurve.Curve.GetEndPoint(1)) / 2;
                        XYZ tagPoint = new XYZ(midpoint.X, midpoint.Y, activeView.GenLevel.Elevation);
                        XYZ tagOffset = new XYZ();

                        if (!selectedPipeTag.IsActive)
                            selectedPipeTag.Activate();

                        IndependentTag.Create(
                            doc, selectedPipeTag.Id, activeView.Id, new Reference(pipe), true, TagOrientation.Horizontal, tagPoint);
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
    public class VWAtag50 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            View initialActiveView = doc.ActiveView;  // Bewaar de initiële view

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

                    List<Element> filterPipes = new FilteredElementCollector(doc, activeView.Id)
                        .OfCategory(BuiltInCategory.OST_PipeCurves)
                        .WhereElementIsNotElementType()
                        .WherePasses(new ElementClassFilter(typeof(Pipe)))
                        .WherePasses(new ElementParameterFilter(
                            new FilterStringRule(new ParameterValueProvider(new ElementId(BuiltInParameter.RBS_SYSTEM_CLASSIFICATION_PARAM)),
                            new FilterStringEquals(),
                            "Sanitary")))
                        .ToElements()
                        .ToList();

                    if (filterPipes.Count == 0)
                    {
                        TaskDialog.Show("Error", "Geen prefab onderdelen gevonden in de view.");
                        return Result.Cancelled;
                    }

                    IList<ElementId> elementsToHide = new List<ElementId>();
                    IList<Element> elementsToBeTagged = new List<Element>();

                    foreach (Pipe elem in filterPipes.Cast<Pipe>())
                    {
                        /// If Pipe is verticaal, dan toevoegen aan hide list anders aan tag list
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
                        .OfCategory(BuiltInCategory.OST_PipeTags);
                    FilterRule familyRule = ParameterFilterRuleFactory.CreateEqualsRule(new ElementId(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM), "M_Pipe_Tag_Nijhof");
                    ElementParameterFilter familyFilter = new ElementParameterFilter(familyRule);
                    FilterRule typeRule = ParameterFilterRuleFactory.CreateEqualsRule(new ElementId(BuiltInParameter.SYMBOL_NAME_PARAM), "Length 5 mm");
                    ElementParameterFilter typeFilter = new ElementParameterFilter(typeRule);

                    /// Combineer de filters en pas ze toe
                    LogicalAndFilter combinedFilter = new LogicalAndFilter(familyFilter, typeFilter);
                    List<Element> filteredTags = tagCollector.WherePasses(combinedFilter).ToElements().ToList();

                    if (!(filteredTags.FirstOrDefault() is FamilySymbol selectedPipeTag))
                    {
                        TaskDialog.Show("Error", "Tag niet gevonden. Laad de tag en probeer het opnieuw");
                        return Result.Cancelled;
                    }

                    /// Verberg alle elementen in de lijst van te verbergen elementen
                    if (elementsToHide.Count > 0)
                    {
                        activeView.HideElements(elementsToHide);
                    }

                    foreach (Element pipe in elementsToBeTagged)
                    {
                        LocationCurve pipeCurve = pipe.Location as LocationCurve;
                        XYZ midpoint = (pipeCurve.Curve.GetEndPoint(0) + pipeCurve.Curve.GetEndPoint(1)) / 2;
                        XYZ tagPoint = new XYZ(midpoint.X, midpoint.Y, activeView.GenLevel.Elevation);
                        XYZ tagOffset = new XYZ();

                        if (!selectedPipeTag.IsActive)
                            selectedPipeTag.Activate();

                        IndependentTag.Create(
                            doc, selectedPipeTag.Id, activeView.Id, new Reference(pipe), true, TagOrientation.Horizontal, tagPoint);
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
    public class VWAtag75 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            View initialActiveView = doc.ActiveView;  // Bewaar de initiële view

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

                    List<Element> filterPipes = new FilteredElementCollector(doc, activeView.Id)
                        .OfCategory(BuiltInCategory.OST_PipeCurves)
                        .WhereElementIsNotElementType()
                        .WherePasses(new ElementClassFilter(typeof(Pipe)))
                        .WherePasses(new ElementParameterFilter(
                            new FilterStringRule(new ParameterValueProvider(new ElementId(BuiltInParameter.RBS_SYSTEM_CLASSIFICATION_PARAM)),
                            new FilterStringEquals(),
                            "Sanitary")))
                        .ToElements()
                        .ToList();

                    if (filterPipes.Count == 0)
                    {
                        TaskDialog.Show("Error", "Geen prefab onderdelen gevonden in de view.");
                        return Result.Cancelled;
                    }

                    IList<ElementId> elementsToHide = new List<ElementId>();
                    IList<Element> elementsToBeTagged = new List<Element>();

                    foreach (Pipe elem in filterPipes.Cast<Pipe>())
                    {
                        /// If Pipe is verticaal, dan toevoegen aan hide list anders aan tag list
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
                        .OfCategory(BuiltInCategory.OST_PipeTags);
                    FilterRule familyRule = ParameterFilterRuleFactory.CreateEqualsRule(new ElementId(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM), "M_Pipe_Tag_Nijhof");
                    ElementParameterFilter familyFilter = new ElementParameterFilter(familyRule);
                    FilterRule typeRule = ParameterFilterRuleFactory.CreateEqualsRule(new ElementId(BuiltInParameter.SYMBOL_NAME_PARAM), "Length 7.5 mm");
                    ElementParameterFilter typeFilter = new ElementParameterFilter(typeRule);

                    /// Combineer de filters en pas ze toe
                    LogicalAndFilter combinedFilter = new LogicalAndFilter(familyFilter, typeFilter);
                    List<Element> filteredTags = tagCollector.WherePasses(combinedFilter).ToElements().ToList();

                    if (!(filteredTags.FirstOrDefault() is FamilySymbol selectedPipeTag))
                    {
                        TaskDialog.Show("Error", "Tag niet gevonden. Laad de tag en probeer het opnieuw");
                        return Result.Cancelled;
                    }

                    /// Verberg alle elementen in de lijst van te verbergen elementen
                    if (elementsToHide.Count > 0)
                    {
                        activeView.HideElements(elementsToHide);
                    }

                    foreach (Element pipe in elementsToBeTagged)
                    {
                        LocationCurve pipeCurve = pipe.Location as LocationCurve;
                        XYZ midpoint = (pipeCurve.Curve.GetEndPoint(0) + pipeCurve.Curve.GetEndPoint(1)) / 2;
                        XYZ tagPoint = new XYZ(midpoint.X, midpoint.Y, activeView.GenLevel.Elevation);
                        XYZ tagOffset = new XYZ();

                        if (!selectedPipeTag.IsActive)
                            selectedPipeTag.Activate();

                        IndependentTag.Create(
                            doc, selectedPipeTag.Id, activeView.Id, new Reference(pipe), true, TagOrientation.Horizontal, tagPoint);
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
    public class VWAtag100 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            View initialActiveView = doc.ActiveView;  // Bewaar de initiële view

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

                    List<Element> filterPipes = new FilteredElementCollector(doc, activeView.Id)
                        .OfCategory(BuiltInCategory.OST_PipeCurves)
                        .WhereElementIsNotElementType()
                        .WherePasses(new ElementClassFilter(typeof(Pipe)))
                        .WherePasses(new ElementParameterFilter(
                            new FilterStringRule(new ParameterValueProvider(new ElementId(BuiltInParameter.RBS_SYSTEM_CLASSIFICATION_PARAM)),
                            new FilterStringEquals(),
                            "Sanitary")))
                        .ToElements()
                        .ToList();

                    if (filterPipes.Count == 0)
                    {
                        TaskDialog.Show("Error", "Geen prefab onderdelen gevonden in de view.");
                        return Result.Cancelled;
                    }

                    IList<ElementId> elementsToHide = new List<ElementId>();
                    IList<Element> elementsToBeTagged = new List<Element>();

                    foreach (Pipe elem in filterPipes.Cast<Pipe>())
                    {
                        /// If Pipe is verticaal, dan toevoegen aan hide list anders aan tag list
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
                        .OfCategory(BuiltInCategory.OST_PipeTags);
                    FilterRule familyRule = ParameterFilterRuleFactory.CreateEqualsRule(new ElementId(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM), "M_Pipe_Tag_Nijhof");
                    ElementParameterFilter familyFilter = new ElementParameterFilter(familyRule);
                    FilterRule typeRule = ParameterFilterRuleFactory.CreateEqualsRule(new ElementId(BuiltInParameter.SYMBOL_NAME_PARAM), "Length 10 mm");
                    ElementParameterFilter typeFilter = new ElementParameterFilter(typeRule);

                    /// Combineer de filters en pas ze toe
                    LogicalAndFilter combinedFilter = new LogicalAndFilter(familyFilter, typeFilter);
                    List<Element> filteredTags = tagCollector.WherePasses(combinedFilter).ToElements().ToList();

                    if (!(filteredTags.FirstOrDefault() is FamilySymbol selectedPipeTag))
                    {
                        TaskDialog.Show("Error", "Tag niet gevonden. Laad de tag en probeer het opnieuw");
                        return Result.Cancelled;
                    }

                    /// Verberg alle elementen in de lijst van te verbergen elementen
                    if (elementsToHide.Count > 0)
                    {
                        activeView.HideElements(elementsToHide);
                    }

                    foreach (Element pipe in elementsToBeTagged)
                    {
                        LocationCurve pipeCurve = pipe.Location as LocationCurve;
                        XYZ midpoint = (pipeCurve.Curve.GetEndPoint(0) + pipeCurve.Curve.GetEndPoint(1)) / 2;
                        XYZ tagPoint = new XYZ(midpoint.X, midpoint.Y, activeView.GenLevel.Elevation);
                        XYZ tagOffset = new XYZ();

                        if (!selectedPipeTag.IsActive)
                            selectedPipeTag.Activate();

                        IndependentTag.Create(
                            doc, selectedPipeTag.Id, activeView.Id, new Reference(pipe), true, TagOrientation.Horizontal, tagPoint);
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
