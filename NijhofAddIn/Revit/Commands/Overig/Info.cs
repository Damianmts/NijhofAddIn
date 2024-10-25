using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace NijhofAddIn.Revit.Commands.Overig
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class Info : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Directe tekstweergave in de popup
            string changelogText = @"## [1.3.0] / 2024-10-25
### Added
- Nieuw tabblad 'Nijhof Prefab'
- Paneel 'Maken'
- Paneel 'View'
- Knop 'Beheer Sets'
- Knop 'Nieuwe Set'
- Knop 'Toevoegen aan Set'
- Knop 'Verwijderen uit Set'
- Knop 'Verwijder Prefabset'
- Knop 'Refresh View'

### Changed
- Meerdere knoppen verplaatst naar 'Nijhof Prefab' tabblad

### Fixed
- Tag 'Groepnummer'
- Tag 'Switchcode'";

            TaskDialog.Show("Changelog", changelogText);

            return Result.Succeeded;
        }
    }
}
