using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NijhofAddIn.Revit.Commands.Content
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class FamilyLoader : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            string familyDirectory = @"F:\Stabiplan\Custom\Families";
            List<string> familyFiles = Directory.GetFiles(familyDirectory, "*.rfa", SearchOption.AllDirectories).ToList();

            if (familyFiles.Count == 0)
            {
                TaskDialog.Show("Family Loader", "Geen families gevonden in de opgegeven map.");
                return Result.Cancelled;
            }

            FamilySelectionWindow selectionWindow = new FamilySelectionWindow(familyFiles, commandData.Application);
            if (selectionWindow.ShowDialog() == true)
            {
                List<string> selectedFamilies = selectionWindow.SelectedFamilies;

                using (Transaction tx = new Transaction(doc, "Load Families"))
                {
                    tx.Start();
                    foreach (string familyPath in selectedFamilies)
                    {
                        if (!doc.LoadFamily(familyPath, out Family family))
                        {
                            TaskDialog.Show("Family Loader", $"Kan de family {Path.GetFileName(familyPath)} niet laden.");
                        }
                    }
                    tx.Commit();
                }

                TaskDialog.Show("Family Loader", "Geselecteerde families zijn geladen.");
            }

            return Result.Succeeded;
        }
    }
}
