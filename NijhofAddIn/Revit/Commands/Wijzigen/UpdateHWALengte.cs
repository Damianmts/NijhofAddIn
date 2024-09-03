using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB.Plumbing;

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
            "DYKA PVC Hemelwaterafvoer_HWA 5.55m",
            "DYKA PVC Hemelwaterafvoer_HWA 6m",
            "DYKA PVC Hemelwaterafvoer_PVC 5.55m"
        };

            int modifiedPipesCount = 0;

            // Begin een transactie
            using (Transaction tx = new Transaction(doc, "Update Pipe Top Elevation"))
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

                                // Bepaal het verschil om de pijp naar 800 mm te brengen
                                double desiredLength = 800 / 304.8; // Revit gebruikt voet
                                double lengthDifference = currentLength - desiredLength;

                                // Pas de Top Elevation aan
                                Parameter topElevationParam = pipe.get_Parameter(BuiltInParameter.RBS_PIPE_TOP_ELEVATION);
                                if (topElevationParam != null && !topElevationParam.IsReadOnly)
                                {
                                    double currentTopElevation = topElevationParam.AsDouble();
                                    topElevationParam.Set(currentTopElevation - lengthDifference);
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
