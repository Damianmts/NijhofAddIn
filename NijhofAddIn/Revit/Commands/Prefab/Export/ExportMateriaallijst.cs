using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using NijhofAddIn.Revit.Core.WPF;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace NijhofAddIn.Revit.Commands.Prefab.Export
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class ExportMateriaallijst : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            // Verzamel de schedules met "Mat" in de naam
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<Element> schedules = collector.OfClass(typeof(ViewSchedule))
                                                      .Cast<ViewSchedule>()
                                                      .Where(schedule => schedule.Name.Contains("Mat"))
                                                      .ToList<Element>();

            // Open het selectie venster
            MateriaallijstWPF selectionWindow = new MateriaallijstWPF(schedules);
            bool? dialogResult = selectionWindow.ShowDialog();

            // Controleer of de gebruiker op "OK" heeft geklikt in plaats van alleen het venster te sluiten
            if (dialogResult == true)
            {
                // Verkrijg de geselecteerde schedules
                List<ViewSchedule> selectedSchedules = selectionWindow.SelectedSchedules;
                if (selectedSchedules == null || !selectedSchedules.Any())
                {
                    message = "No schedules selected for export.";
                    return Result.Cancelled;
                }

                // Controleer of het pad is ingevuld
                string path = selectionWindow.PathTextBox.Text;
                if (string.IsNullOrEmpty(path))
                {
                    message = "Export cancelled by user.";
                    return Result.Cancelled;
                }

                try
                {
                    // Voer de export uit
                    foreach (var schedule in selectedSchedules)
                    {
                        ExportScheduleToExcel(schedule, path);
                    }
                    TaskDialog.Show("Export Complete", "Selected schedules exported to Excel successfully.");
                    return Result.Succeeded;
                }
                catch (Exception e)
                {
                    message = e.Message;
                    return Result.Failed;
                }
            }

            // Geen actie als het venster gesloten wordt zonder export
            return Result.Cancelled;
        }

        private void ExportScheduleToExcel(ViewSchedule schedule, string path)
        {
            Excel.Application xlApp = new Excel.Application();
            if (xlApp == null)
            {
                throw new InvalidOperationException("Excel is not installed.");
            }

            // Maak een Excel werkboek en werkblad aan
            Excel.Workbook xlWorkBook = xlApp.Workbooks.Add();
            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets[1];
            xlWorkSheet.Name = schedule.Name;

            // Haal de gegevens van de schedule op
            var tableData = schedule.GetTableData();
            var sectionData = tableData.GetSectionData(SectionType.Body);
            int rows = sectionData.NumberOfRows;
            int cols = sectionData.NumberOfColumns;

            // Voeg de gegevens toe aan het werkblad
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    var cellValue = schedule.GetCellText(SectionType.Body, row, col);
                    Excel.Range cell = (Excel.Range)xlWorkSheet.Cells[row + 1, col + 1];
                    cell.Value = cellValue;

                    // Stel de uitlijning in op links
                    cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

                    // Maak de eerste rij vet
                    if (row == 0)
                    {
                        cell.Font.Bold = true;
                    }
                }
            }

            // Pas de kolombreedte automatisch aan
            xlWorkSheet.Columns.AutoFit();

            // Sla het Excel-bestand op
            string filePath = Path.Combine(path, schedule.Name + ".xlsx");
            xlWorkBook.SaveAs(filePath);
            xlWorkBook.Close(false);
            xlApp.Quit();

            // Release de COM-objecten
            ReleaseObject(xlWorkSheet);
            ReleaseObject(xlWorkBook);
            ReleaseObject(xlApp);
        }

        private void ReleaseObject(object obj)
        {
            try
            {
                Marshal.ReleaseComObject(obj);
            }
            catch
            {
                // Negeer uitzonderingen
            }
            finally
            {
                obj = null;
                GC.Collect();
            }
        }
    }
}
