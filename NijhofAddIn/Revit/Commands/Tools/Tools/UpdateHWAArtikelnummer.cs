﻿using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;

namespace NijhofAddIn.Revit.Commands.Tools.Tools
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class UpdateHWAArtikelnummer : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            ElementCategoryFilter pipeCategoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_PipeCurves);

            int changedPipesCount = 0;
            int totalPipesToChange = 0;
            int nonDykaPipesCount = 0;

            using (Transaction trans = new Transaction(doc, "Vervang Pipe Types en Update Parameters"))
            {
                trans.Start();

                FilteredElementCollector pipeCollector = new FilteredElementCollector(doc)
                    .OfClass(typeof(Pipe))
                    .WherePasses(pipeCategoryFilter);

                foreach (Pipe pipe in pipeCollector)
                {
                    Parameter systemAbbreviationParam = pipe.LookupParameter("System Abbreviation");
                    if (systemAbbreviationParam != null)
                    {
                        string systemAbbreviationValue = systemAbbreviationParam.AsString();
                        if (systemAbbreviationValue == "M521" || systemAbbreviationValue == "M5210")
                        {
                            ElementType pipeType = doc.GetElement(pipe.GetTypeId()) as ElementType;
                            if (pipeType == null || (!pipeType.Name.Contains("DYKA") && !pipeType.Name.Contains("Dyka")))
                            {
                                nonDykaPipesCount++;
                                continue;
                            }

                            Parameter sizeParam = pipe.LookupParameter("Size");
                            if (sizeParam != null)
                            {
                                string sizeValue = sizeParam.AsString();
                                Parameter manufacturerParam = pipe.LookupParameter("Manufacturer Art. No.");

                                if (sizeValue != null && sizeValue.Contains("80"))
                                {
                                    totalPipesToChange++;
                                    if (manufacturerParam != null && !manufacturerParam.IsReadOnly && manufacturerParam.AsString() != "20033890")
                                    {
                                        // Stel het artikelnummer in op 20033890
                                        manufacturerParam.Set("20033890");
                                        changedPipesCount++;
                                    }
                                }
                                else if (sizeValue != null && sizeValue.Contains("100"))
                                {
                                    totalPipesToChange++;
                                    if (manufacturerParam != null && !manufacturerParam.IsReadOnly && manufacturerParam.AsString() != "20033900")
                                    {
                                        // Stel het artikelnummer in op 20033900
                                        manufacturerParam.Set("20033900");
                                        changedPipesCount++;
                                    }
                                }
                            }
                        }
                    }
                }

                trans.Commit();
            }

            if (nonDykaPipesCount > 0)
            {
                TaskDialog.Show("Waarschuwing", $"{nonDykaPipesCount} HWA-buizen zijn niet van DYKA en zijn overgeslagen.");
            }

            if (totalPipesToChange == 0)
            {
                TaskDialog.Show("Resultaat", "Geen pijpen gevonden om te updaten.");
            }
            else if (changedPipesCount > 0)
            {
                TaskDialog.Show("Resultaat", $"{changedPipesCount} pijpen zijn geüpdate.");
            }
            else
            {
                TaskDialog.Show("Resultaat", "Alles is al geüpdate.");
            }

            return Result.Succeeded;
        }
    }
}
