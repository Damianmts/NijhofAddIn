using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NijhofAddIn.Revit.Commands.Wijzigen
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class UpdateHWALengte : IExternalCommand
    {
        public Result Execute(ExternalCommandData extCmdData, ref string msg, ElementSet elements)
        {
            UIApplication uiapp = extCmdData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                // Select pipes
                var selectedPipes = uidoc.Selection.PickObjects(ObjectType.Element, new ESFPipe(), "Trek een hatch over het hele hwa systeem.");
                if (selectedPipes.Count == 0)
                {
                    TaskDialog.Show("Error", "Niks geselecteerd.");
                    return Result.Cancelled;
                }

                List<Element> pipeListToModify = new List<Element>();
                foreach (var item in selectedPipes)
                {
                    var pipeElement = doc.GetElement(item) as Element;
                    if (pipeElement != null)
                    {
                        Parameter parameterType = pipeElement.LookupParameter("Article Type");
                        Parameter parameterDiameter = pipeElement.LookupParameter("D1 Description");
                        if (parameterType != null && parameterType.HasValue &&
                            (parameterType.AsString() == "HWA 6m" || parameterType.AsString() == "PVC 5,55m" || parameterType.AsString() == "HWA 5,55m") &&
                            (parameterDiameter.AsString() == "80" || parameterDiameter.AsString() == "100"))
                        {
                            pipeListToModify.Add(pipeElement);
                        }
                    }
                }

                List<LocationCurve> pipesLocationCurve = new List<LocationCurve>();
                foreach (var pipe in pipeListToModify)
                {
                    if (pipe.Location is LocationCurve locationCurve)
                    {
                        pipesLocationCurve.Add(locationCurve);
                    }
                }

                List<Autodesk.Revit.DB.Line> pipeLines = new List<Autodesk.Revit.DB.Line>();
                foreach (var pipeCurve in pipesLocationCurve)
                {
                    if (pipeCurve.Curve is Autodesk.Revit.DB.Line line)
                    {
                        pipeLines.Add(line);
                    }
                }

                List<XYZ> newStartList = new List<XYZ>();
                List<XYZ> newEndList = new List<XYZ>();

                for (int i = 0; i < pipeListToModify.Count; i++)
                {
                    XYZ newStart;
                    XYZ newEnd;
                    var pipeDirection = pipeLines[i].Direction;

                    if (pipeDirection.Z < -0.9)
                    {
                        newStart = new XYZ(
                            pipesLocationCurve[i].Curve.GetEndPoint(0).X,
                            pipesLocationCurve[i].Curve.GetEndPoint(0).Y,
                            pipesLocationCurve[i].Curve.GetEndPoint(1).Z + 2.624671916);
                        newEnd = pipesLocationCurve[i].Curve.GetEndPoint(1);
                    }
                    else
                    {
                        newStart = pipesLocationCurve[i].Curve.GetEndPoint(0);
                        newEnd = new XYZ(
                            pipesLocationCurve[i].Curve.GetEndPoint(1).X,
                            pipesLocationCurve[i].Curve.GetEndPoint(1).Y,
                            pipesLocationCurve[i].Curve.GetEndPoint(0).Z + 2.624671916);
                    }

                    newStartList.Add(newStart);
                    newEndList.Add(newEnd);
                }

                XYZ translation = new XYZ(0.00001, 0.00001, 0);
                XYZ translationBack = new XYZ(-0.00001, -0.00001, 0);

                using (var transaction = new Transaction(doc, "NT - HWA Length 800"))
                {
                    transaction.Start();

                    for (int i = 0; i < pipeListToModify.Count; i++)
                    {
                        Autodesk.Revit.DB.Line line = Autodesk.Revit.DB.Line.CreateBound(newStartList[i], newEndList[i]);
                        pipesLocationCurve[i].Curve = line;

                        ElementTransformUtils.MoveElement(doc, pipeListToModify[i].Id, translation);
                        ElementTransformUtils.MoveElement(doc, pipeListToModify[i].Id, translationBack);
                    }

                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                return Result.Failed;
            }

            return Result.Succeeded;
        }
    }

    // Example implementation of ESFPipe class as a selection filter (needs to be adjusted based on actual requirements)
    public class ESFPipe : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
#if REVIT2023
            // Add logic to filter elements (e.g., check if elem is a pipe)
            return elem.Category?.Id.IntegerValue == (int)BuiltInCategory.OST_PipeCurves;
#elif REVIT2024 || REVIT2025
            return elem.Category?.Id.Value == (int)BuiltInCategory.OST_PipeCurves;
#endif
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}
