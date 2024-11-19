using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using NijhofAddIn.Revit.Core.WPF;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NijhofAddIn.Revit.Commands.Tools.Content
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

            // Open het venster niet-modaal zodat Revit gebruikt kan blijven worden
            FamilySelectionWindow selectionWindow = new FamilySelectionWindow(familyFiles, commandData.Application);
            selectionWindow.Show(); // Gebruik Show() in plaats van ShowDialog() om het venster niet-modaal te maken

            // Direct terugkeren om Revit beschikbaar te houden
            return Result.Succeeded;
        }
    }
}
