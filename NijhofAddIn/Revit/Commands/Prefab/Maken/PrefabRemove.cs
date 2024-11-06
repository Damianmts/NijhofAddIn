using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NijhofAddIn.Revit.Commands.Prefab.Maken
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class PrefabRemove : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Start de transactie
            using (Transaction transaction = new Transaction(doc, "Prefab Set Remove"))
            {
                transaction.Start();

                try
                {
                    // Selecteer meerdere objecten
                    IList<Reference> selectedObjects = uidoc.Selection.PickObjects(ObjectType.Element, "Selecteer objecten om prefab gegevens te resetten");
                    if (selectedObjects == null || selectedObjects.Count == 0)
                    {
                        message = "Geen elementen geselecteerd.";
                        return Result.Cancelled;
                    }

                    // Haal de geselecteerde elementen op
                    List<Element> elementsToClear = selectedObjects
                        .Select(reference => doc.GetElement(reference))
                        .Where(element => element != null)
                        .ToList();

                    foreach (Element element in elementsToClear)
                    {
                        // Reset 'Prefab Set'
                        Parameter prefabSetParam = element.LookupParameter("Prefab Set");
                        if (prefabSetParam != null && prefabSetParam.StorageType == StorageType.String)
                        {
                            prefabSetParam.Set(string.Empty);
                        }

                        // Reset 'Prefab Color ID'
                        Parameter prefabColorIDParam = element.LookupParameter("Prefab Color ID");
                        if (prefabColorIDParam != null && prefabColorIDParam.StorageType == StorageType.String)
                        {
                            prefabColorIDParam.Set(string.Empty);
                        }

                        // Reset 'Prefab Number'
                        Parameter prefabNumberParam = element.LookupParameter("Prefab Number");
                        if (prefabNumberParam != null && prefabNumberParam.StorageType == StorageType.String)
                        {
                            prefabNumberParam.Set(string.Empty);
                        }

                        // Laat 'Manufacturer Art. No.' intact
                    }

                    // Commit de transactie
                    transaction.Commit();

                    return Result.Succeeded;
                }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                {
                    message = "Selectie geannuleerd.";
                    return Result.Cancelled;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    return Result.Failed;
                }
            }
        }
    }
}
