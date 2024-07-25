using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NijhofAddIn.Revit.Commands.Overig
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class Info : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Haal het pad op van de huidige assembly
            string assemblyPath = Assembly.GetExecutingAssembly().Location;
            string directoryPath = Path.GetDirectoryName(assemblyPath);

            // Voeg de bestandsnaam van het CHANGELOG-bestand toe
            string changelogPath = Path.Combine(directoryPath, "CHANGELOG.md");

            // Lees het bestand
            if (File.Exists(changelogPath))
            {
                string changelogContent = File.ReadAllText(changelogPath);

                // Regex om de laatste versie te vinden
                Regex versionRegex = new Regex(@"## \[(\d+\.\d+\.\d+)\]");
                Match match = versionRegex.Match(changelogContent);

                if (match.Success)
                {
                    string latestVersion = match.Groups[1].Value;

                    // Toon de versie-informatie
                    TaskDialog.Show("Melding", $"Versie: {latestVersion}");
                }
                else
                {
                    TaskDialog.Show("Melding", "Versie informatie niet gevonden in CHANGELOG.md");
                }
            }
            else
            {
                TaskDialog.Show("Melding", "CHANGELOG.md bestand niet gevonden.");
            }

            return Result.Succeeded;
        }
    }
}
