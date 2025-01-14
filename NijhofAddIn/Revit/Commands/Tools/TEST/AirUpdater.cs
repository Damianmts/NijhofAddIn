using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Linq;

namespace NijhofAddIn.Revit.Commands.Tools.TEST
{
    [Transaction(TransactionMode.Manual)]
    internal class AirUpdater : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Verkrijg het actieve document
            Document doc = commandData.Application.ActiveUIDocument.Document;

            // Definieer de family en het type
            string familyName = "VE_Connection Piece_Oval to Round_MEPcontent_DYKA_Valve Adapter 2Socket";
            string typeName = "H100 235-160";
            string newManufacturerArtNo = "20055320"; // Nieuwe waarde als tekst

            // Zoek naar de FamilySymbol
            var familySymbols = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .Cast<FamilySymbol>()
                .Where(fs => fs.FamilyName == familyName && fs.Name == typeName)
                .ToList();

            // Controleer of er FamilySymbols zijn gevonden
            if (!familySymbols.Any())
            {
                TaskDialog.Show("Resultaat", $"Geen FamilySymbols gevonden voor family '{familyName}' en type '{typeName}'.");
                return Result.Failed;
            }

            using (Transaction trans = new Transaction(doc, "Update Manufacturer Art. No."))
            {
                trans.Start();

                foreach (FamilySymbol symbol in familySymbols)
                {
                    // Verkrijg de parameter
                    Parameter manufacturerArtNoParam = symbol.LookupParameter("Manufacturer Art. No.");

                    if (manufacturerArtNoParam != null && !manufacturerArtNoParam.IsReadOnly)
                    {
                        // Controleer of de parameter tekst ondersteunt
                        if (manufacturerArtNoParam.StorageType == StorageType.String)
                        {
                            manufacturerArtNoParam.Set(newManufacturerArtNo);
                        }
                        else
                        {
                            TaskDialog.Show("Fout", $"Parameter 'Manufacturer Art. No.' is geen tekstparameter voor FamilySymbol '{symbol.Name}'.");
                        }
                    }
                    else
                    {
                        TaskDialog.Show("Fout", $"Kan parameter 'Manufacturer Art. No.' niet bijwerken voor FamilySymbol '{symbol.Name}'.");
                    }
                }

                trans.Commit();
            }

            TaskDialog.Show("Resultaat", $"'Manufacturer Art. No.' succesvol bijgewerkt naar '{newManufacturerArtNo}' voor FamilySymbol '{familyName}' - '{typeName}'.");
            return Result.Succeeded;
        }
    }
}
