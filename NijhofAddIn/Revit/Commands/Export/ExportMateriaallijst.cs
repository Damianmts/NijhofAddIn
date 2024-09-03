using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using NijhofAddIn.Revit.Core;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace NijhofAddIn.Revit.Commands.Export
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

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<Element> schedules = collector.OfClass(typeof(ViewSchedule))
                                                      .Cast<ViewSchedule>()
                                                      .Where(schedule => schedule.Name.Contains("Mat"))
                                                      .ToList<Element>();

            MateriaallijstWPF selectionWindow = new MateriaallijstWPF(schedules);
            bool? dialogResult = selectionWindow.ShowDialog();

            if (dialogResult == true)
            {
                List<ViewSchedule> selectedSchedules = selectionWindow.SelectedSchedules;
                if (selectedSchedules == null || !selectedSchedules.Any())
                {
                    message = "No schedules selected for export.";
                    return Result.Cancelled;
                }

                string path = selectionWindow.PathTextBox.Text;
                if (string.IsNullOrEmpty(path))
                {
                    message = "Export cancelled by user.";
                    return Result.Cancelled;
                }

                try
                {
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
            else
            {
                message = "Export cancelled by user."; // Show only if user cancels explicitly
                return Result.Cancelled;
            }
        }

        private void ExportScheduleToExcel(ViewSchedule schedule, string path)
        {
            Excel.Application xlApp = new Excel.Application();
            if (xlApp == null)
            {
                throw new InvalidOperationException("Excel is not installed.");
            }

            // Create Excel workbook and worksheet
            Excel.Workbook xlWorkBook = xlApp.Workbooks.Add();
            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets[1];
            xlWorkSheet.Name = schedule.Name;

            // Get schedule data
            var tableData = schedule.GetTableData();
            var sectionData = tableData.GetSectionData(SectionType.Body);
            int rows = sectionData.NumberOfRows;
            int cols = sectionData.NumberOfColumns;

            // Add schedule data
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    var cellValue = schedule.GetCellText(SectionType.Body, row, col);
                    Excel.Range cell = (Excel.Range)xlWorkSheet.Cells[row + 1, col + 1];
                    cell.Value = cellValue;

                    // Set alignment to left
                    cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

                    // Make the first row bold
                    if (row == 0)
                    {
                        cell.Font.Bold = true;
                    }
                }
            }

            // Auto-fit the columns
            xlWorkSheet.Columns.AutoFit();

            // Save the Excel file
            string filePath = Path.Combine(path, schedule.Name + ".xlsx");
            xlWorkBook.SaveAs(filePath);
            xlWorkBook.Close(false);
            xlApp.Quit();

            // Release COM objects
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
                // Ignore exceptions
            }
            finally
            {
                obj = null;
                GC.Collect();
            }
        }
    }
}
