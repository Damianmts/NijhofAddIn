using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Plumbing;

namespace NijhofAddIn.Revit.Commands.Tools.Tools
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class AansluitenElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                bool success = ConnectElements(uidoc, doc);
                return success ? Result.Succeeded : Result.Cancelled;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return Result.Cancelled;
            }
        }

        private bool ConnectElements(UIDocument uidoc, Document doc)
        {
            Reference movedReference = uidoc.Selection.PickObject(ObjectType.Element, new NoInsulationFilter(), "Selecteer het element wat je wilt verbinden");
            Element movedElement = doc.GetElement(movedReference);
            XYZ movedPoint = movedReference.GlobalPoint;

            Reference targetReference = uidoc.Selection.PickObject(ObjectType.Element, new NoInsulationFilter(), "Selecteer het element waarmee je wilt verbinden");
            Element targetElement = doc.GetElement(targetReference);
            XYZ targetPoint = targetReference.GlobalPoint;

            if (targetElement.Id == movedElement.Id)
            {
                TaskDialog.Show("Foutmelding", "Oeps, het lijkt erop dat je hetzelfde element hebt geselecteerd.");
                return true;
            }

            Connector movedConnector = GetClosestConnector(movedElement, movedPoint);
            Connector targetConnector = GetClosestConnector(targetElement, targetPoint);

            if (movedConnector == null || targetConnector == null)
            {
                TaskDialog.Show("Foutmelding", "Het lijkt erop dat het geselecteerde element geen ongebruikte connector heeft.");
                return true;
            }

            if (movedConnector.Domain != targetConnector.Domain)
            {
                TaskDialog.Show("Foutmelding", "Je hebt 2 elementen van verschillende systemen geselecteerd.");
                return true;
            }

            XYZ movedDirection = movedConnector.CoordinateSystem.BasisZ;
            XYZ targetDirection = targetConnector.CoordinateSystem.BasisZ;

            double angle = movedDirection.AngleTo(targetDirection);
            XYZ vector = null;

            if (angle != Math.PI)
            {
                vector = angle == 0 ? movedConnector.CoordinateSystem.BasisY : movedDirection.CrossProduct(targetDirection);

                Line rotationAxis = Line.CreateBound(movedPoint, movedPoint + vector);
                using (Transaction t = new Transaction(doc, "Draai Element"))
                {
                    t.Start();
                    movedElement.Location.Rotate(rotationAxis, angle - Math.PI);
                    t.Commit();
                }
            }

            using (Transaction t = new Transaction(doc, "Verplaats En Verbind Elementen"))
            {
                t.Start();
                ((LocationPoint)movedElement.Location).Move(targetConnector.Origin - movedConnector.Origin);
                movedConnector.ConnectTo(targetConnector);
                t.Commit();
            }

            return true;
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
