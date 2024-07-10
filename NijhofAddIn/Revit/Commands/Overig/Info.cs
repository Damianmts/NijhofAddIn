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
            TaskDialog.Show("Melding", "Versie: 1405.24"); /// Versie is datum in ddmm.jj formaat
            return Result.Succeeded;
        }
    }
}
