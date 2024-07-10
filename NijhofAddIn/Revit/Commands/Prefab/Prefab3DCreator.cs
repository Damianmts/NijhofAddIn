using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using NijhofAddIn.Revit.Commands.Prefab.Extensions;
using NijhofAddIn.Revit.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NijhofAddIn.Revit.Commands.Prefab
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class Prefab3DCreator : IExternalCommand
    {
        [Obsolete]
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            View tempActiveView = doc.ActiveView;
            UIApplication uiapp = commandData.Application;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
            CAV closeActiveView = new CAV();
            BBE bboxExpander = new BBE();
            VFA VFHVA = new VFA();

            try
            {
                /////-----User moet view selecteren-----   
                // Check if the active view is a sheet
                if (!(tempActiveView is ViewSheet))
                {
                    TaskDialog.Show("Error", "Deze functie werkt alleen in een sheet. Zorg er ook voor dat je uit de viewport bent.");
                    return Result.Cancelled;
                }

                // Prompt the user to select a viewport
                Reference reference = uidoc.Selection.PickObject(ObjectType.Element, new ESF(), "Selecteer een viewport");
                Element viewElement = doc.GetElement(reference);
                Viewport viewport = viewElement as Viewport;
                //Autodesk.Revit.DB.View activeview = viewElement as Autodesk.Revit.DB.View;
                ElementId viewId = viewport.ViewId;
                View activeView = doc.GetElement(viewId) as View;
                //uidoc.RequestViewChange(activeView);

                // Check if the selected element is a view and not a legend or 3D
                if (!(viewport is Viewport)
                    || activeView.ViewType.ToString() == "Legend"
                    || activeView.ViewType.ToString() == "ThreeD")
                {
                    TaskDialog.Show("Error", "Please select a floorplan.");
                    closeActiveView.CloseActiveView(uidoc);
                    return Result.Cancelled;
                }

                using (Transaction tagTransaction = new Transaction(doc, "NT - 3D maken"))
                {
                    tagTransaction.Start();

                    /////-----Create 3D view of prefab------
                    var multiClassFilter = new ElementMulticlassFilter(
                        new List<Type>()
                        {
                            typeof(Pipe),
                            typeof(Duct)
                        });

                    var multiCategoryFilter = new ElementMulticategoryFilter(
                        new List<BuiltInCategory>()
                        {
                            BuiltInCategory.OST_PipeCurves,
                            BuiltInCategory.OST_PipeFitting,
                            BuiltInCategory.OST_DuctCurves,
                            BuiltInCategory.OST_DuctFitting,
                        });

                    var parameterFilter1 = new ElementParameterFilter(
                        new FilterStringRule(
                            new ParameterValueProvider(
                                new ElementId(BuiltInParameter.RBS_SYSTEM_CLASSIFICATION_PARAM)),
                                    new FilterStringEquals(), "Sanitary"));

                    var parameterFilter2 = new ElementParameterFilter(
                        new FilterStringRule(
                            new ParameterValueProvider(
                                new ElementId(BuiltInParameter.RBS_SYSTEM_CLASSIFICATION_PARAM)),
                                    new FilterStringEquals(), "Exhaust Air"));

                    var parameterFilter3 = new ElementParameterFilter(
                        new FilterStringRule(
                            new ParameterValueProvider(
                                new ElementId(BuiltInParameter.RBS_SYSTEM_CLASSIFICATION_PARAM)),
                                    new FilterStringEquals(), "Supply Air"));

                    var parameterFilter4 = new ElementParameterFilter(
                        new FilterStringRule(
                            new ParameterValueProvider(
                                new ElementId(BuiltInParameter.RBS_SYSTEM_CLASSIFICATION_PARAM)),
                                    new FilterStringEquals(), "Return Air"));

                    var parameterFilter = new List<ElementFilter>();
                    parameterFilter.AddRange(new[] { parameterFilter1, parameterFilter2, parameterFilter3, parameterFilter4 });
                    var logicalOrFilterParameters = new LogicalOrFilter(parameterFilter);

                    var logicalOrFilter = new LogicalOrFilter(multiCategoryFilter, multiClassFilter);

                    var totalFilter = new LogicalAndFilter(logicalOrFilter, logicalOrFilterParameters);

                    FilteredElementCollector prefabCollector = new FilteredElementCollector(doc, activeView.Id)
                        .WherePasses(totalFilter);

                    //Check if collector is empty, otherwise cancel action
                    if (prefabCollector.ToElementIds().Count == 0)
                    {
                        TaskDialog.Show("Error", "No elements found in view.");
                        closeActiveView.CloseActiveView(uidoc);
                        return Result.Cancelled;
                    }

                    ////Alle omhooggaande pipes uit de set halen
                    IList<ElementId> elementsToHide = new List<ElementId>();
                    IList<Element> elementsTo3D = new List<Element>();
                    foreach (Element elem in prefabCollector)
                    {
                        if (elem.Category.Id == new ElementId(BuiltInCategory.OST_PipeCurves))
                        {
                            Pipe pipeElem = elem as Pipe;
                            // If Pipe is verticaal, dan toevoegen aan hide list anders aan tag list
                            var location = pipeElem.Location as LocationCurve;
                            var direction = location.Curve.GetEndPoint(1) - location.Curve.GetEndPoint(0);
                            direction = direction.Normalize();

                            if (direction.Z < 0.8 && direction.Z > -0.8)
                            {
                                elementsTo3D.Add(pipeElem as Element);
                            }
                            else
                            {
                                elementsToHide.Add(elem.Id as ElementId);
                            }

                        }
                        else if (elem.Category.Id == new ElementId(BuiltInCategory.OST_DuctCurves))
                        {
                            Duct pipeElem = elem as Duct;
                            // If duct is verticaal, dan toevoegen aan hide list anders aan tag list
                            var locationDuct = pipeElem.Location as LocationCurve;
                            var directionDuct = locationDuct.Curve.GetEndPoint(1) - locationDuct.Curve.GetEndPoint(0);
                            directionDuct = directionDuct.Normalize();

                            if (directionDuct.Z < 0.8 && directionDuct.Z > -0.8)
                            {
                                elementsTo3D.Add(pipeElem as Element);
                            }
                            else
                            {
                                elementsToHide.Add(elem.Id as ElementId);
                            }

                        }
                        else
                        {
                            elementsTo3D.Add(elem as Element);
                        }
                    }


                    //zoek of er meer pipes of ducts in de collection zit
                    var elementsPipes = new int();
                    var elementsDucts = new int();
                    elementsPipes = 0;
                    elementsDucts = 0;
                    foreach (var element in prefabCollector)
                    {
                        var categoryElement = element.Category.Name;
                        if (categoryElement == "Ducts" || categoryElement == "Duct Fittings")
                        {
                            elementsDucts++;
                        }
                        else
                        {
                            elementsPipes++;
                        }
                    }

                    string viewTemplateString = string.Empty;
                    Boolean MorePipesThanDucts = false;
                    if (elementsPipes > elementsDucts)
                    {
                        viewTemplateString = "04_Plot_Prefab_Riool_3D";
                        MorePipesThanDucts = true;
                    }
                    else
                    {
                        viewTemplateString = "03_Plot_Prefab_Lucht_3D";
                    }

                    //filteredellementcollector die of ducts of pipes filterd.
                    IList<Element> elementsTo3DNew = new List<Element>();
                    if (MorePipesThanDucts)
                    {
                        elementsTo3DNew = elementsTo3D
                            .Where(element => (element.Category.Id.IntegerValue == (int)BuiltInCategory.OST_PipeCurves) ||
                                                (element.Category.Id.IntegerValue == (int)BuiltInCategory.OST_PipeFitting))
                            .ToList();
                    }
                    else
                    {
                        elementsTo3DNew = elementsTo3D
                            .Where(element => (element.Category.Id.IntegerValue == (int)BuiltInCategory.OST_DuctCurves) ||
                                              (element.Category.Id.IntegerValue == (int)BuiltInCategory.OST_DuctFitting))
                            .ToList();
                    }

                    //Maak boundingbox van alle elementen in prefabCollector
                    BoundingBoxXYZ boundingBox = new BoundingBoxXYZ();
                    Element firstElement = elementsTo3DNew[0]; // Haal het eerste element uit de lijst
                    boundingBox = firstElement.get_BoundingBox(null); // Haal de BoundingBox van het element op
                    foreach (var element in elementsTo3DNew)
                    {
                        BoundingBoxXYZ elementBoundingBox = element.get_BoundingBox(null);
                        if (elementBoundingBox != null)
                        {
                            if (boundingBox == null)
                            {
                                boundingBox = elementBoundingBox;
                            }
                            else
                            {
                                boundingBox = bboxExpander.BoundingBoxExpander(boundingBox, element);
                            }
                        }
                    }
                    boundingBox.Min = new XYZ(boundingBox.Min.X - 0.4, boundingBox.Min.Y - 0.4, boundingBox.Min.Z - 0.4);
                    boundingBox.Max = new XYZ(boundingBox.Max.X + 0.4, boundingBox.Max.Y + 0.4, boundingBox.Max.Z + 0.4);


                    //Template vinden voor 3d view
                    IEnumerable<View> views = new FilteredElementCollector(doc)
                        .OfClass(typeof(View))
                        .Cast<View>()
                        .Where(v => v.Name.Equals(viewTemplateString));


                    ElementId templateId = null;
                    if (views.Count() == 0)
                    {
                        SelectViewTemplateWPF wpfFormTemplate = new SelectViewTemplateWPF(doc);
                        wpfFormTemplate.ShowDialog();
                        View template = wpfFormTemplate.ViewTemplateSelect;
                        templateId = template.Id;

                    }
                    else
                    {
                        View template = views.FirstOrDefault();
                        templateId = template.Id;
                    }

                    //Orientatie maken voor 3d view
                    double angleHorizD = 135; //XY Plane 0 - 360
                    double angleVertD = -30; // tilt -90 - 90

                    XYZ eye = XYZ.Zero;
                    XYZ forward = VFHVA.VectorFromHorizVertAngles(angleHorizD, angleVertD);
                    XYZ up = VFHVA.VectorFromHorizVertAngles(angleHorizD, angleVertD + 90);

                    ViewOrientation3D viewOrientation3D = new ViewOrientation3D(eye, up, forward);
                    ViewFamilyType viewFamilyType3D = new FilteredElementCollector(doc)
                        .OfClass(typeof(ViewFamilyType))
                        .Cast<ViewFamilyType>()
                        .FirstOrDefault<ViewFamilyType>(x => ViewFamily.ThreeDimensional == x.ViewFamily);

                    //3d view maken
                    View3D view3d = View3D.CreateIsometric(doc, viewFamilyType3D.Id);

                    //-----3D view instelleingen-----
                    String sheetNumber = tempActiveView.get_Parameter(BuiltInParameter.SHEET_NUMBER).AsValueString();

                    var collector3D = new FilteredElementCollector(doc)
                            .OfCategory(BuiltInCategory.OST_Views)
                            .WhereElementIsNotElementType()
                            .Cast<View>()
                            .Where(v => v.ViewType == ViewType.ThreeD);

                    Boolean dublicateName = false;
                    int dublicateCount = 1;

                    string naamSheet = "Nr " + sheetNumber + " - 3D view";

                    foreach (var viewName in collector3D)
                    {
                        if (viewName.Name.Contains(naamSheet))
                        {
                            dublicateName = true;

                            if (viewName.Name.Split(' ').Last() != "view")
                            {
                                dublicateCount++;
                            }
                        }
                    }
                    if (dublicateName)
                    {
                        view3d.Name = "Nr " + sheetNumber + " - 3D view " + dublicateCount.ToString();
                    }
                    else
                    {
                        view3d.Name = "Nr " + sheetNumber + " - 3D view";
                    }

                    view3d.SetOrientation(viewOrientation3D);
                    view3d.IsSectionBoxActive = true;
                    view3d.SetSectionBox(boundingBox);
                    view3d.ViewTemplateId = templateId;
                    if (elementsToHide.Count > 0)
                    {
                        view3d.HideElements(elementsToHide);
                    }

                    //3D op sheet plaatsen
                    Viewport viewport3D = Viewport.Create(doc, tempActiveView.Id, view3d.Id, viewport.GetBoxCenter());

                    tagTransaction.Commit();
                }

            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }
            return Result.Succeeded;
        }
    }
}
