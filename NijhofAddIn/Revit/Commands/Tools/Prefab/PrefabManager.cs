using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NijhofAddIn.Revit.Commands.Tools.Prefab
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class PrefabManager : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Haal de huidige Revit-document
            Document doc = commandData.Application.ActiveUIDocument.Document;

            // Definieer de parameternaam voor de Prefab Set en System Abbreviation
            string prefabSetParameterName = "Prefab Set";
            string systemAbbreviationParameterName = "System Abbreviation";

            // Mapping van system abbreviations naar disciplines
            Dictionary<string, string> systemAbbreviationToDiscipline = new Dictionary<string, string>
            {
                { "M521", "HWA" },
                { "M524", "VWA" },
                { "M525", "Systeem 2" },
                // Voeg indien nodig meer afkortingen en disciplines toe
            };

            // Haal alle elementen met een Prefab Set parameter
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            IList<Element> allElementsWithPrefabSet = collector
                .WhereElementIsNotElementType()
                .Where(e => e.LookupParameter(prefabSetParameterName) != null)
                .ToList();

            // Maak een lijst om de prefab sets en bijbehorende disciplines op te slaan
            Dictionary<string, string> prefabSetDisciplineMap = new Dictionary<string, string>();

            foreach (Element elem in allElementsWithPrefabSet)
            {
                string prefabSetValue = elem.LookupParameter(prefabSetParameterName)?.AsString();
                string systemAbbreviation = elem.LookupParameter(systemAbbreviationParameterName)?.AsString();

                if (!string.IsNullOrEmpty(prefabSetValue) && !string.IsNullOrEmpty(systemAbbreviation))
                {
                    // Haal de discipline op op basis van de system abbreviation
                    if (systemAbbreviationToDiscipline.TryGetValue(systemAbbreviation, out string discipline))
                    {
                        prefabSetDisciplineMap[prefabSetValue] = discipline;
                    }
                    else
                    {
                        prefabSetDisciplineMap[prefabSetValue] = "Onbekende discipline";
                    }
                }
            }

            // Maak de lijst van prefabsets en disciplines om te tonen in de popup
            string prefabSetList = string.Join("\n", prefabSetDisciplineMap
                .OrderBy(x => x.Key)
                .Select(x => $"{x.Key} - {x.Value}"));

            // Toon de popup met de prefabsets en disciplines
            TaskDialog.Show("Prefab Sets en Disciplines",
                string.IsNullOrEmpty(prefabSetList)
                ? "Geen prefab sets gevonden in het project."
                : $"Prefab sets en bijbehorende disciplines:\n\n{prefabSetList}");

            return Result.Succeeded;
        }
    }
}
