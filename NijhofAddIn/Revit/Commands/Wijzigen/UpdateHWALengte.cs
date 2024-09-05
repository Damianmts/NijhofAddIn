using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;

namespace NijhofAddIn.Revit.Commands.Wijzigen
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class UpdateHWALengte : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            View activeView = uidoc.ActiveView;

            // Definieer de pijp types die je wilt zoeken
            List<string> pipeTypeNames = new List<string>
            {
                "DYKA PVC Hemelwaterafvoer_HWA 5,55m",
                "DYKA PVC Hemelwaterafvoer_HWA 6m",
                "DYKA PVC Hemelwaterafvoer_PVC 5,55m"
            };

            int modifiedPipesCount = 0;

            // Begin een transactie
            using (Transaction tx = new Transaction(doc, "Update Pipe Length"))
            {
                tx.Start();

                // Filter voor alle pijpen die zichtbaar zijn in de actieve view
                FilteredElementCollector collector = new FilteredElementCollector(doc, activeView.Id)
                    .OfClass(typeof(Pipe));

                // Loop door elke pijp en controleer of het type overeenkomt
                foreach (Element elem in collector)
                {
                    Pipe pipe = elem as Pipe;
                    if (pipe != null)
                    {
                        // Verkrijg het pijptype
                        PipeType pipeType = doc.GetElement(pipe.GetTypeId()) as PipeType;
                        if (pipeType != null && pipeTypeNames.Contains(pipeType.Name))
                        {
                            // Controleer of de pijp verticaal is
                            Line pipeLine = (pipe.Location as LocationCurve)?.Curve as Line;

                            if (pipeLine != null && IsVertical(pipeLine))
                            {
                                // Bereken de huidige lengte van de pijp
                                double currentLength = pipeLine.Length;

                                // Bepaal de gewenste lengte (800 mm)
                                double desiredLength = 800 / 304.8; // Revit gebruikt voet

                                // Bereken het verschil tussen de huidige lengte en de gewenste lengte
                                double lengthDifference = currentLength - desiredLength;

                                if (Math.Abs(lengthDifference) > 1e-9)
                                {
                                    // Verkrijg de huidige start- en eindpunten van de pijp
                                    XYZ startPoint = pipeLine.GetEndPoint(0);
                                    XYZ endPoint = pipeLine.GetEndPoint(1);

                                    // Verplaats de bovenkant van de pijp naar de gewenste hoogte
                                    XYZ newEndPoint;
                                    if (endPoint.Z > startPoint.Z) // Pijp gaat omhoog
                                    {
                                        newEndPoint = new XYZ(endPoint.X, endPoint.Y, startPoint.Z + desiredLength);
                                    }
                                    else // Pijp gaat omlaag
                                    {
                                        newEndPoint = new XYZ(startPoint.X, startPoint.Y, startPoint.Z + desiredLength);
                                    }

                                    // Stel de nieuwe curve in (startpunt blijft hetzelfde)
                                    (pipe.Location as LocationCurve).Curve = Line.CreateBound(startPoint, newEndPoint);

                                    modifiedPipesCount++;
                                }
                            }
                        }
                    }
                }

                tx.Commit();
            }

            // Meldingen aan de gebruiker
            if (modifiedPipesCount > 0)
            {
                TaskDialog.Show("Resultaat", $"{modifiedPipesCount} pijpen zijn aangepast naar een lengte van 800 mm.");
            }
            else
            {
                TaskDialog.Show("Resultaat", "Er zijn geen pijpen gevonden die aangepast moesten worden.");
            }

            return Result.Succeeded;
        }

        private bool IsVertical(Line line)
        {
            XYZ direction = line.Direction;
            // Check of de lijn verticaal is (richting van de lijn is (0,0,1) of (0,0,-1))
            return (Math.Abs(direction.X) < 1e-9 && Math.Abs(direction.Y) < 1e-9 && Math.Abs(Math.Abs(direction.Z) - 1) < 1e-9);
        }
    }
}
