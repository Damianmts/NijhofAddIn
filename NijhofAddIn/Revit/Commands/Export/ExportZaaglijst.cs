using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using NijhofAddIn.Revit.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NijhofAddIn.Revit.Commands.Export
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class ExportZaaglijst : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            // Get all schedules in the document and filter by name
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<Element> schedules = collector.OfClass(typeof(ViewSchedule))
                                                      .Cast<ViewSchedule>()
                                                      .Where(schedule => schedule.Name.Contains("Zaaglijst"))
                                                      .ToList<Element>();

            // Get the project number
            string projectNumber = GetProjectParameter(doc, "Project Nummer");
            string projectName = GetProjectParameter(doc, "Project Naam");

            // Show WPF popup for schedule selection
            ZaaglijstWPF selectionWindow = new ZaaglijstWPF(schedules, projectNumber, projectName);
            if (selectionWindow.ShowDialog() == true)
            {
                List<ViewSchedule> selectedSchedules = selectionWindow.SelectedSchedules;
                if (selectedSchedules == null || !selectedSchedules.Any())
                {
                    message = "No schedules selected for export.";
                    return Result.Cancelled;
                }

                // Get the export path from the user
                string path = selectionWindow.FilePathTextBox.Text;
                if (string.IsNullOrEmpty(path))
                {
                    message = "Export cancelled by user.";
                    return Result.Cancelled;
                }

                // Validate the export path
                if (!IsValidPath(path))
                {
                    message = "Invalid export path. Please provide a valid path.";
                    return Result.Cancelled;
                }

                try
                {
                    foreach (var schedule in selectedSchedules)
                    {
                        var zaaglijst = selectionWindow.Zaaglijsten.FirstOrDefault(z => z.Schedule == schedule);
                        ExportScheduleToCSV(schedule, path, zaaglijst);
                    }
                    TaskDialog.Show("Export Complete", "Selected schedules exported to CSV successfully.");
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
                return Result.Cancelled;
            }
        }

        public static void ExportScheduleToCSV(ViewSchedule schedule, string path, Zaaglijst zaaglijst)
        {
            if (schedule == null)
            {
                throw new ArgumentNullException(nameof(schedule));
            }

            try
            {
                // Get schedule data
                var tableData = schedule.GetTableData();
                var sectionData = tableData.GetSectionData(SectionType.Body);
                int rows = sectionData.NumberOfRows;
                int cols = sectionData.NumberOfColumns;

                // Get metadata from project/ schedule
                Document doc = schedule.Document;
                string projectNumber = GetProjectParameter(doc, "Project Nummer");
                string projectName = GetProjectParameter(doc, "Project Naam");
                string dateDrawn = GetProjectParameter(doc, "Datum getekend");
                string drawer = GetProjectParameter(doc, "Hoofd Tekenaar");
                string disciplineVWA = GetProjectParameter(doc, "Tx_Disipline_VWA");
                string disciplineHWA = GetProjectParameter(doc, "Tx_Disipline_HWA");
                string disciplineMV = GetProjectParameter(doc, "Tx_Dicipline_MV");
                string floor = GetScheduleParameter(schedule, "Bouwlaag");
                string kavel = GetScheduleParameter(schedule, "Kavel");

                // Get metadata from zaaglijst (WPF form)
                string disciplineWPF = zaaglijst.Discipline;
                string kavelWPF = zaaglijst.KavelNummer;
                string floorWPF = zaaglijst.Verdieping;
                string projectWPF = zaaglijst.ProjectNummer;

                // Filter rows with length 5000
                int countLength5000 = 0;
                List<int> rowsToExport = new List<int>();
                for (int row = 0; row < rows; row++)
                {
                    string lengthCellValue = schedule.GetCellText(SectionType.Body, row, 1); // Assuming length is in the second column (index 1)
                    if (lengthCellValue == "5000")
                    {
                        countLength5000++;
                    }
                    else
                    {
                        rowsToExport.Add(row);
                    }
                }

                // Create CSV file name
                string fileName = schedule.Name;
                if (countLength5000 > 0)
                {
                    fileName += $" (+{countLength5000}x 5m ø110)";
                }
                fileName += ".csv";

                string filePath = Path.Combine(path, fileName);

                using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    // Write metadata
                    sw.WriteLine("HWA Zaaglijst;;;;;;");
                    sw.WriteLine($"Projectnummer:;{projectNumber};Aantal kavels:;1;;;");
                    sw.WriteLine($"Projectnaam:;{projectName};;;;;");
                    sw.WriteLine($"Discipline, Bouwlaag;{disciplineVWA}{disciplineHWA}{disciplineMV};{floor};;;;");
                    sw.WriteLine($"Kavel:;{kavel};;;;;");
                    sw.WriteLine($"Datum geëxporteerd:;{dateDrawn};;;;;");
                    sw.WriteLine($"Tekenaar:;{drawer};;;;;");
                    sw.WriteLine(";;;;;;");

                    // Write schedule data
                    foreach (int row in rowsToExport)
                    {
                        for (int col = 0; col < cols; col++)
                        {
                            // Write cell value to CSV file
                            if (col > 0)
                            {
                                sw.Write(";");
                            }

                            // Fill columns E, F, and G with metadata from WPF form starting from row 2
                            if (row == 0 && col == 4)
                            {
                                sw.Write("Kavel");
                            }
                            else if (row == 0 && col == 5)
                            {
                                sw.Write("Verdieping");
                            }
                            else if (row == 0 && col == 6)
                            {
                                sw.Write("Project");
                            }
                            else if (row > 1 && col == 4)
                            {
                                sw.Write(disciplineWPF + " " + kavelWPF);
                            }
                            else if (row > 1 && col == 5)
                            {
                                sw.Write(floorWPF);
                            }
                            else if (row > 1 && col == 6)
                            {
                                sw.Write(projectWPF);
                            }
                            else
                            {
                                // Clear columns E to Z
                                if (col >= 4 && col <= 25)
                                {
                                    sw.Write("");
                                }
                                else
                                {
                                    var cellValue = schedule.GetCellText(SectionType.Body, row, col);

                                    // Escape double quotes in cell value
                                    if (cellValue.Contains("\""))
                                    {
                                        cellValue = cellValue.Replace("\"", "\"\"");
                                    }

                                    // Enclose the cell value in double quotes if it contains a comma
                                    if (cellValue.Contains(","))
                                    {
                                        cellValue = $"\"{cellValue}\"";
                                    }

                                    sw.Write(cellValue);
                                }
                            }
                        }
                        sw.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while exporting the schedule '{schedule.Name}': {ex.Message}");
            }
        }

        // Method to get project parameter
        private static string GetProjectParameter(Document doc, string parameterName)
        {
            ProjectInfo projectInfo = doc.ProjectInformation;
            Autodesk.Revit.DB.Parameter param = projectInfo.LookupParameter(parameterName);
            return param != null ? param.AsString() : string.Empty;
        }

        // Method to get schedule parameter
        private static string GetScheduleParameter(ViewSchedule schedule, string parameterName)
        {
            Autodesk.Revit.DB.Parameter param = schedule.LookupParameter(parameterName);
            return param != null ? param.AsString() : string.Empty;
        }

        // Method to validate the export path
        private bool IsValidPath(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    return false;
                }

                var tempFile = Path.Combine(path, Path.GetRandomFileName());
                using (var fs = File.Create(tempFile, 1, FileOptions.DeleteOnClose))
                { }
                return true;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Path Validation Error", $"The path '{path}' is invalid: {ex.Message}");
                return false;
            }
        }
    }
}
