using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace NijhofAddIn.Revit.Commands.Tools.Content
{
    public class PlaceFamilyEventHandler : IExternalEventHandler
    {
        private Family _familyToPlace;
        private bool _familyLoaded = false;

        public void SetFamilyToPlace(Family family)
        {
            _familyToPlace = family;
            _familyLoaded = true;  // Zet de flag zodra de familie is ingesteld
        }

        public void Execute(UIApplication app)
        {
            Document doc = app.ActiveUIDocument.Document;

            if (!_familyLoaded || _familyToPlace == null)
            {
                TaskDialog.Show("Fout", "De familie is nog niet volledig geladen. Wacht even en probeer opnieuw.");
                return;
            }

            // Zoek het eerste FamilySymbol in de geladen Family
            FamilySymbol familySymbol = null;
            foreach (ElementId id in _familyToPlace.GetFamilySymbolIds())
            {
                familySymbol = doc.GetElement(id) as FamilySymbol;
                if (familySymbol != null)
                {
                    break;
                }
            }

            if (familySymbol == null)
            {
                TaskDialog.Show("Fout", "Er zijn geen FamilySymbols gevonden in de geladen Family.");
                return;
            }

            if (!familySymbol.IsActive)
            {
                using (Transaction tx = new Transaction(doc, "Activate Family Symbol"))
                {
                    tx.Start();
                    familySymbol.Activate();
                    doc.Regenerate();
                    tx.Commit();
                }
            }

            // Gebruik PostRequestForElementTypePlacement om de plaatsingsmodus te activeren
            app.ActiveUIDocument.PostRequestForElementTypePlacement(familySymbol);
            _familyLoaded = false;  // Reset de flag na plaatsing
        }

        public string GetName()
        {
            return "Place Family Event Handler";
        }
    }
}
