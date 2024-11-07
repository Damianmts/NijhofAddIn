using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Exceptions;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Plumbing;

namespace NijhofAddIn.Revit.Commands.Tools.Wijzigen
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class AansluitenElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            ConnectElements(uidoc, doc);

            return Result.Succeeded;
        }

        private bool ConnectElements(UIDocument uidoc, Document doc)
        {
            try
            {
                Reference movedReference;
                Element movedElement;
                XYZ movedPoint;

                try
                {
                    movedReference = uidoc.Selection.PickObject(ObjectType.Element, new NoInsulationFilter(), "Pick element to move");
                    movedElement = doc.GetElement(movedReference);
                    movedPoint = movedReference.GlobalPoint;
                }
                catch (System.OperationCanceledException)
                {
                    return false;
                }

                Reference targetReference;
                Element targetElement;
                XYZ targetPoint;

                try
                {
                    targetReference = uidoc.Selection.PickObject(ObjectType.Element, new NoInsulationFilter(), "Pick element to be connected to");
                    targetElement = doc.GetElement(targetReference);
                    targetPoint = targetReference.GlobalPoint;
                }
                catch (System.OperationCanceledException)
                {
                    return true;
                }
                catch (Exception)
                {
                    TaskDialog.Show("Invalid Object", "Oops, it looks like you chose an invalid object");
                    return true;
                }

                if (targetElement.Id == movedElement.Id)
                {
                    TaskDialog.Show("Attribute Error", "Oops, it looks like you've selected the same object twice.");
                    return true;
                }

                Connector movedConnector = GetClosestConnector(movedElement, movedPoint);
                Connector targetConnector = GetClosestConnector(targetElement, targetPoint);

                if (movedConnector == null || targetConnector == null)
                {
                    TaskDialog.Show("AttributeError", "It looks like one of the objects has no unused connector.");
                    return true;
                }

                if (movedConnector.Domain != targetConnector.Domain)
                {
                    TaskDialog.Show("Domain Error", "You picked 2 connectors of different domain. Please retry.");
                    return true;
                }

                XYZ movedDirection = movedConnector.CoordinateSystem.BasisZ;
                XYZ targetDirection = targetConnector.CoordinateSystem.BasisZ;

                double angle = movedDirection.AngleTo(targetDirection);
                XYZ vector = null;

                if (angle != Math.PI)
                {
                    if (angle == 0)
                    {
                        vector = movedConnector.CoordinateSystem.BasisY;
                    }
                    else
                    {
                        vector = movedDirection.CrossProduct(targetDirection);
                    }

                    try
                    {
                        Line rotationAxis = Line.CreateBound(movedPoint, movedPoint + vector);
                        using (Transaction t = new Transaction(doc, "Rotate Element"))
                        {
                            t.Start();
                            movedElement.Location.Rotate(rotationAxis, angle - Math.PI);
                            t.Commit();
                        }
                    }
                    catch (System.ArgumentException)
                    {
                        TaskDialog.Show("Debug", "Vector : " + vector.ToString() + "; Angle : " + angle);
                    }
                }

                using (Transaction t = new Transaction(doc, "Move and Connect Elements"))
                {
                    t.Start();
                    ((LocationPoint)movedElement.Location).Move(targetConnector.Origin - movedConnector.Origin);
                    movedConnector.ConnectTo(targetConnector);
                    t.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return false;
            }
        }

        private Connector GetClosestConnector(Element element, XYZ point)
        {
            ConnectorSet connectors = null;
            if (element is FamilyInstance familyInstance)
            {
                connectors = familyInstance.MEPModel?.ConnectorManager?.Connectors;
            }
            else if (element is Pipe pipe)
            {
                connectors = pipe.ConnectorManager?.Connectors;
            }
            else if (element is Duct duct)
            {
                connectors = duct.ConnectorManager?.Connectors;
            }

            if (connectors == null)
                return null;

            Connector closestConnector = null;
            double minDistance = double.MaxValue;

            foreach (Connector connector in connectors)
            {
                if (connector.IsConnected)
                    continue;

                double distance = connector.Origin.DistanceTo(point);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestConnector = connector;
                }
            }

            return closestConnector;
        }

        private class NoInsulationFilter : ISelectionFilter
        {
            public bool AllowElement(Element elem)
            {
                return !(elem is InsulationLiningBase) && HasConnectors(elem);
            }

            public bool AllowReference(Reference reference, XYZ position)
            {
                return true;
            }

            private bool HasConnectors(Element elem)
            {
                try
                {
                    return elem is FamilyInstance || elem is Pipe || elem is Duct;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
