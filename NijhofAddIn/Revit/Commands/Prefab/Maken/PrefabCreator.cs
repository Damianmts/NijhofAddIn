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
    public class PrefabCreator : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Start de transactie
            using (Transaction transaction = new Transaction(doc, "Prefab Set Assign"))
            {
                transaction.Start();

                try
                {
                    // Selecteer meerdere objecten
                    IList<Reference> selectedObjects = uidoc.Selection.PickObjects(ObjectType.Element, "Selecteer objecten");
                    if (selectedObjects == null || selectedObjects.Count == 0)
                    {
                        message = "Geen elementen geselecteerd.";
                        return Result.Cancelled;
                    }

                    // Zoek beschikbare 'Prefab Set' nummers en 'Prefab Color ID'
                    HashSet<int> existingSetNumbers = GetUsedPrefabSetNumbers(doc);
                    int nextAvailableNumber = FindNextAvailableNumber(existingSetNumbers);
                    string prefabColorID = GetNextAvailableColorID(doc, nextAvailableNumber);

                    // Haal de elementen op en sorteer ze op hun locatie (Y- en X-coördinaten)
                    List<Element> sortedElements = selectedObjects
                        .Select(reference => doc.GetElement(reference))
                        .Where(element => element != null)
                        .OrderBy(element => GetElementLocation(element).Y) // Sorteren op Y (van onder naar boven)
                        .ThenBy(element => GetElementLocation(element).X)  // Daarna sorteren op X (links naar rechts)
                        .ToList();

                    // Begin nummering voor elk element binnen de prefab set
                    int prefabElementNumber = 1;

                    // Verwerk de selectie en wijs dezelfde 'Prefab Set' en 'Prefab Color ID' toe
                    foreach (Element element in sortedElements)
                    {
                        // Wijs 'Prefab Set' toe
                        Parameter prefabSetParam = element.LookupParameter("Prefab Set");
                        if (prefabSetParam?.StorageType == StorageType.String)
                        {
                            prefabSetParam.Set(nextAvailableNumber.ToString());
                        }

                        // Wijs 'Prefab Color ID' toe
                        Parameter prefabColorIDParam = element.LookupParameter("Prefab Color ID");
                        if (prefabColorIDParam?.StorageType == StorageType.String)
                        {
                            prefabColorIDParam.Set(prefabColorID);
                        }

                        // Wijs een uniek 'Prefab Number' toe binnen de set
                        Parameter prefabNumberParam = element.LookupParameter("Prefab Number");
                        if (prefabNumberParam?.StorageType == StorageType.String)
                        {
                            prefabNumberParam.Set(prefabElementNumber.ToString());
                            prefabElementNumber++; // Verhoog voor elk nieuw element
                        }

                        // Controleer of het element een pijp is en de juiste 'Size' heeft
                        if (element.Category.Name == "Pipes")
                        {
                            Parameter sizeParam = element.LookupParameter("Size");
                            if (sizeParam != null)
                            {
                                string sizeValue = sizeParam.AsString();
                                string manufacturerArtNo = GetManufacturerArtNoBySize(sizeValue);

                                // Wijs het artikelnummer toe aan 'Manufacturer Art. No.' alleen voor de juiste 'Size'
                                if (!string.IsNullOrEmpty(manufacturerArtNo))
                                {
                                    Parameter artNoParam = element.LookupParameter("Manufacturer Art. No.");
                                    if (artNoParam != null && artNoParam.StorageType == StorageType.String)
                                    {
                                        artNoParam.Set(manufacturerArtNo);
                                    }
                                }
                            }
                        }
                    }

                    // Commit de transactie
                    transaction.Commit();

                    // Toon één melding met het toegewezen 'Prefab Set' nummer en 'Prefab Color ID'
                    TaskDialog.Show("Prefab Set", $"Prefab Set {nextAvailableNumber} met Prefab Color ID {prefabColorID} is aangemaakt.");
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

        // Methode om het juiste artikelnummer te verkrijgen op basis van de 'Size' parameter
        private string GetManufacturerArtNoBySize(string size)
        {
            Dictionary<string, string> sizeToArtNoMap = new Dictionary<string, string>
            {
                { "32", "20034683" },
                { "40", "20034686" },
                { "50", "20034688" },
                { "75", "20034690" },
                { "90", "20034692" },
                { "110", "20034694" },
                { "125", "20034697" },
                { "160", "20034700" }
            };

            foreach (var sizeKey in sizeToArtNoMap.Keys)
            {
                if (size.Contains(sizeKey))
                {
                    return sizeToArtNoMap[sizeKey];
                }
            }

            return null; // Geen artikelnummer gevonden voor de gegeven 'Size'
        }

        private HashSet<int> GetUsedPrefabSetNumbers(Document doc)
        {
            HashSet<int> usedNumbers = new HashSet<int>();
            FilteredElementCollector collector = new FilteredElementCollector(doc).WhereElementIsNotElementType();

            foreach (Element element in collector)
            {
                Parameter prefabSetParam = element.LookupParameter("Prefab Set");
                if (prefabSetParam?.StorageType == StorageType.String && int.TryParse(prefabSetParam.AsString(), out int setValue) && setValue > 0)
                {
                    usedNumbers.Add(setValue);
                }
            }

            return usedNumbers;
        }

        private int FindNextAvailableNumber(HashSet<int> existingSetNumbers)
        {
            int number = 1;
            while (existingSetNumbers.Contains(number))
            {
                number++;
            }
            return number;
        }

        private string GetNextAvailableColorID(Document doc, int prefabSetNumber)
        {
            int colorID = (prefabSetNumber - 1) % 10 + 1;
            return colorID.ToString("D2"); // Zorg ervoor dat het een twee-cijferige string is, bv. "01"
        }

        private XYZ GetElementLocation(Element element)
        {
            Location location = element.Location;
            if (location is LocationPoint pointLocation)
            {
                return pointLocation.Point;
            }
            else if (location is LocationCurve curveLocation)
            {
                return curveLocation.Curve.GetEndPoint(0); // Startpunt van de curve
            }
            return XYZ.Zero;
        }
    }
}
