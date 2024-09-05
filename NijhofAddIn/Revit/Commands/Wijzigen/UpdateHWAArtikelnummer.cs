using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System.Linq;

public class UpdateHWAArtikelnummer : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        // Verkrijg het huidige document
        Document doc = commandData.Application.ActiveUIDocument.Document;

        // Maak een filter voor het zoeken naar Pipe types
        ElementCategoryFilter pipeCategoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_PipeCurves);

        // Verzameling van alle pipe types
        FilteredElementCollector pipeTypeCollector = new FilteredElementCollector(doc)
            .OfClass(typeof(PipeType))
            .WherePasses(pipeCategoryFilter);

        // Vind de doel pipe types
        PipeType targetPipeType80 = pipeTypeCollector
            .Cast<PipeType>()
            .FirstOrDefault(pt => pt.Name == "DYKA PVC Hemelwaterafvoer_HWA 6m");

        PipeType targetPipeType100 = pipeTypeCollector
            .Cast<PipeType>()
            .FirstOrDefault(pt => pt.Name == "DYKA PVC Hemelwaterafvoer_HWA 5,55m");

        if (targetPipeType80 == null || targetPipeType100 == null)
        {
            message = "Een van de doel pipe types is niet gevonden.";
            return Result.Failed;
        }

        int changedPipesCount = 0;
        int totalPipesToChange = 0;

        // Transactie voor het wijzigen van de pipe types en het aanpassen van parameters
        using (Transaction trans = new Transaction(doc, "Vervang Pipe Types en Update Parameters"))
        {
            trans.Start();

            // Zoek naar alle pijpen in het project
            FilteredElementCollector pipeCollector = new FilteredElementCollector(doc)
                .OfClass(typeof(Pipe))
                .WherePasses(pipeCategoryFilter);

            // Loop door alle pijpen en bepaal of er wijzigingen nodig zijn
            foreach (Pipe pipe in pipeCollector)
            {
                // Controleer de "Size" parameter
                Parameter sizeParam = pipe.LookupParameter("Size");
                if (sizeParam != null)
                {
                    string sizeValue = sizeParam.AsString();

                    // Zoek het "Manufacturer Art. No." parameter
                    Parameter manufacturerParam = pipe.LookupParameter("Manufacturer Art. No.");

                    if (sizeValue == "ø80")
                    {
                        totalPipesToChange++;

                        // Controleer of de pipe al het juiste type en artikelnummer heeft
                        if (pipe.GetTypeId() != targetPipeType80.Id || (manufacturerParam != null && manufacturerParam.AsString() != "20033890"))
                        {
                            // Vervang het pipe type door "DYKA PVC Hemelwaterafvoer_HWA 6m"
                            pipe.ChangeTypeId(targetPipeType80.Id);
                            changedPipesCount++;

                            // Stel het artikelnummer in op 20033890
                            if (manufacturerParam != null && !manufacturerParam.IsReadOnly)
                            {
                                manufacturerParam.Set("20033890");
                            }
                        }
                    }
                    else if (sizeValue == "ø100")
                    {
                        totalPipesToChange++;

                        // Controleer of de pipe al het juiste type en artikelnummer heeft
                        if (pipe.GetTypeId() != targetPipeType100.Id || (manufacturerParam != null && manufacturerParam.AsString() != "20033900"))
                        {
                            // Vervang het pipe type door "DYKA PVC Hemelwaterafvoer_HWA 5,55m"
                            pipe.ChangeTypeId(targetPipeType100.Id);
                            changedPipesCount++;

                            // Stel het artikelnummer in op 20033900
                            if (manufacturerParam != null && !manufacturerParam.IsReadOnly)
                            {
                                manufacturerParam.Set("20033900");
                            }
                        }
                    }
                }
            }

            trans.Commit();
        }

        // Toon een popup met het aantal wijzigingen
        if (totalPipesToChange == 0)
        {
            TaskDialog.Show("Resultaat", "Geen pijpen gevonden om te controleren.");
        }
        else if (changedPipesCount > 0)
        {
            TaskDialog.Show("Resultaat", $"{changedPipesCount} pijpen zijn bijgewerkt.");
        }
        else
        {
            TaskDialog.Show("Resultaat", "Alles is al geüpdatet.");
        }

        return Result.Succeeded;
    }
}
