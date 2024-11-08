using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;

namespace NijhofAddIn.Revit.Commands.Tools.Views
{
    [Transaction(TransactionMode.Manual)]
    public class RefreshView : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            // Verkrijg de actieve UIDocument en Document
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            View activeView = doc.ActiveView;

            using (Transaction trans = new Transaction(doc, "Toggle Temporary View Template"))
            {
                try
                {
                    trans.Start();

                    // Sla de huidige View Template op
                    ElementId originalTemplateId = activeView.ViewTemplateId;

                    // Zet de View Template tijdelijk uit als er eentje actief is
                    if (originalTemplateId != ElementId.InvalidElementId)
                    {
                        // Zet de template uit door deze op "InvalidElementId" te zetten
                        activeView.ViewTemplateId = ElementId.InvalidElementId;
                    }

                    // Regenerate om de wijzigingen te laten doorvoeren
                    doc.Regenerate();

                    // Zet de oorspronkelijke View Template terug
                    if (originalTemplateId != ElementId.InvalidElementId)
                    {
                        activeView.ViewTemplateId = originalTemplateId;
                    }

                    trans.Commit();
                }
                catch (System.Exception ex)
                {
                    // Als er iets fout gaat, rollback en toon de foutmelding
                    message = ex.Message;
                    return Result.Failed;
                }
            }

            return Result.Succeeded;
        }
    }
}
