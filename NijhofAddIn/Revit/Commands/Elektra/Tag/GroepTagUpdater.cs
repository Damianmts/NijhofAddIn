using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Linq;

namespace NijhofAddIn.Revit.Commands.Elektra.Tag
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class GroepTagUpdater : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;
            View activeView = doc.ActiveView;

            // Stap 1: Haal het FamilySymbol op voor de groep-tag
            ElementId tagSymbolId;
            try
            {
                tagSymbolId = GetGroepTagSymbolId(doc);
            }
            catch (InvalidOperationException ex)
            {
                TaskDialog.Show("Fout", ex.Message);
                return Result.Failed;
            }

            // Stap 2: Voeg ontbrekende groep-tags toe
            AddMissingGroepTags(doc, activeView, tagSymbolId);

            return Result.Succeeded;
        }

        private ElementId GetGroepTagSymbolId(Document doc)
        {
            // Zoek FamilySymbol
            var symbol = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .WhereElementIsNotElementType()
                .Cast<FamilySymbol>()
                .FirstOrDefault(fs => fs.FamilyName.Equals("Groep Tag", StringComparison.OrdinalIgnoreCase)
                                      && fs.Name.Equals("Groep Tag", StringComparison.OrdinalIgnoreCase));

            if (symbol == null)
            {
                // Zoek direct via Family
                var family = new FilteredElementCollector(doc)
                    .OfClass(typeof(Family))
                    .Cast<Family>()
                    .FirstOrDefault(f => f.Name.Equals("Groep Tag", StringComparison.OrdinalIgnoreCase));

                if (family != null)
                {
                    foreach (ElementId symbolId in family.GetFamilySymbolIds())
                    {
                        var familySymbol = doc.GetElement(symbolId) as FamilySymbol;
                        if (familySymbol.Name.Equals("Groep Tag", StringComparison.OrdinalIgnoreCase))
                        {
                            if (!familySymbol.IsActive)
                            {
                                using (Transaction tx = new Transaction(doc, "Activeer symbool"))
                                {
                                    tx.Start();
                                    familySymbol.Activate();
                                    tx.Commit();
                                }
                            }
                            return familySymbol.Id;
                        }
                    }
                }

                throw new InvalidOperationException("Het FamilySymbol 'Groep Tag' is niet gevonden.");
            }

            // Activeer symbool indien nodig
            if (!symbol.IsActive)
            {
                using (Transaction tx = new Transaction(doc, "Activeer symbool"))
                {
                    tx.Start();
                    symbol.Activate();
                    tx.Commit();
                }
            }

            return symbol.Id;
        }

        private void AddMissingGroepTags(Document doc, View activeView, ElementId tagSymbolId)
        {
            var categories = new BuiltInCategory[]
            {
                BuiltInCategory.OST_FireAlarmDevices,
                BuiltInCategory.OST_LightingDevices,
                BuiltInCategory.OST_LightingFixtures,
                BuiltInCategory.OST_ElectricalFixtures
            };

            using (Transaction tx = new Transaction(doc, "Groep-tags toevoegen"))
            {
                tx.Start();

                foreach (var category in categories)
                {
                    var elementsToTag = new FilteredElementCollector(doc, activeView.Id)
                        .OfCategory(category)
                        .WhereElementIsNotElementType()
                        .ToElements()
                        .Where(el => NeedsGroepTag(el, doc, activeView, tagSymbolId));

                    foreach (var element in elementsToTag)
                    {
                        CreateGroepTag(doc, activeView, element);
                    }
                }

                tx.Commit();
            }
        }

        private bool NeedsGroepTag(Element element, Document doc, View activeView, ElementId tagSymbolId)
        {
            Parameter groepParam = element.LookupParameter("Groep");
            if (groepParam == null || string.IsNullOrWhiteSpace(groepParam.AsString()))
            {
                return false;
            }

            var tags = new FilteredElementCollector(doc, activeView.Id)
                .OfClass(typeof(IndependentTag))
                .OfCategory(BuiltInCategory.OST_MultiCategoryTags)
                .WhereElementIsNotElementType()
                .Cast<IndependentTag>()
                .Where(t => t.GetTaggedLocalElementIds().Contains(element.Id));

            return !tags.Any();
        }

        private void CreateGroepTag(Document doc, View activeView, Element element)
        {
            LocationPoint locationPoint = element.Location as LocationPoint;
            if (locationPoint != null)
            {
                XYZ location = locationPoint.Point;
                Reference reference = new Reference(element);

                IndependentTag.Create(
                    doc,
                    activeView.Id,
                    reference,
                    false,
                    TagMode.TM_ADDBY_MULTICATEGORY,
                    TagOrientation.Horizontal,
                    location
                );
            }
        }
    }
}
