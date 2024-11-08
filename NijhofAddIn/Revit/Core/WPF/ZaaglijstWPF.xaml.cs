using Autodesk.Revit.DB;
using NijhofAddIn.Revit.Commands.Tools.Export;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NijhofAddIn.Revit.Core.WPF
{
    public partial class ZaaglijstWPF : Window, INotifyPropertyChanged
    {
        private string _sharedDataFilePath;
        private bool _selectAllCheckBox;
        private const string DefaultExportPath = @"T:\Data\!Zaagmachine"; // Standaard exportlocatie
        private string _projectNumber;
        private string _projectName;

        public ObservableCollection<Zaaglijst> Zaaglijsten { get; set; }
        public ObservableCollection<string> VerdiepingOptions { get; set; }
        public List<ViewSchedule> SelectedSchedules { get; private set; }

        public bool SelectAllCheckBox
        {
            get { return _selectAllCheckBox; }
            set
            {
                _selectAllCheckBox = value;
                OnPropertyChanged(nameof(SelectAllCheckBox));
                SelectOrDeselectAll(_selectAllCheckBox);
            }
        }

        public ZaaglijstWPF(ICollection<Element> schedules, string projectNumber, string projectName)
        {
            InitializeComponent();
            DataContext = this;
            VerdiepingOptions = new ObservableCollection<string> { "fundering", "1e verd", "2e verd" };
            _projectNumber = projectNumber;
            _projectName = projectName;
            _sharedDataFilePath = Path.Combine(@"T:\Data\!Zaagmachine\z_NIET VERWIJDEREN\Zaaglijst Data Revit", $"{projectNumber} zaaglijstdata {projectName}.txt"); /// Dynamische bestandsnaam
            LoadZaaglijsten(schedules, projectNumber, projectName);
            FilePathTextBox.Text = DefaultExportPath; /// Stel standaard exportlocatie in
            FilePathTextBox.Foreground = Brushes.Gray; /// Standaard tekst kleur instellen
            Closing += Window_Closing;
        }

        private void LoadZaaglijsten(ICollection<Element> schedules, string projectNumber, string projectName)
        {
            var zaaglijstenLijst = new List<Zaaglijst>();

            if (File.Exists(_sharedDataFilePath))
            {
                var lines = File.ReadAllLines(_sharedDataFilePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 5)
                    {
                        var schedule = schedules.FirstOrDefault(s => s.Name == parts[0]) as ViewSchedule;
                        if (schedule != null)
                        {
                            zaaglijstenLijst.Add(new Zaaglijst
                            {
                                Name = parts[0],
                                Verdieping = parts[1],
                                Discipline = parts[2],
                                KavelNummer = parts[3],
                                ProjectNummer = parts[4],
                                Schedule = schedule,
                            });
                        }
                    }
                }
            }
            else
            {
                foreach (var schedule in schedules.Cast<ViewSchedule>())
                {
                    string discipline = DetermineDiscipline(schedule.Name);

                    zaaglijstenLijst.Add(new Zaaglijst
                    {
                        Name = schedule.Name,
                        ProjectNummer = projectNumber,
                        Schedule = schedule,
                        Discipline = discipline
                    });
                }
            }

            var gesorteerdeZaaglijsten = zaaglijstenLijst.OrderBy(z => z.Name).ToList();
            Zaaglijsten = new ObservableCollection<Zaaglijst>(gesorteerdeZaaglijsten);
        }

        private string DetermineDiscipline(string scheduleName)
        {
            if (scheduleName.Contains("MV"))
            {
                return "MV";
            }
            if (scheduleName.Contains("HWA"))
            {
                return "HWA";
            }
            if (scheduleName.Contains("VWA"))
            {
                return "VWA";
            }
            return string.Empty;
        }

        private void OpslaanButton_Click(object sender, RoutedEventArgs e)
        {
            string dynamicFilePath = Path.Combine(@"T:\Data\!Zaagmachine\z_NIET VERWIJDEREN\Zaaglijst Data Revit", $"{_projectNumber} zaaglijstdata {_projectName}.txt");

            try
            {
                using (var writer = new StreamWriter(dynamicFilePath))
                {
                    foreach (var zaaglijst in Zaaglijsten)
                    {
                        writer.WriteLine($"{zaaglijst.Name},{zaaglijst.Verdieping},{zaaglijst.Discipline},{zaaglijst.KavelNummer},{zaaglijst.ProjectNummer}"); // Save in the correct order
                    }
                }
                MessageBox.Show("Zaaglijsten zijn opgeslagen!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Er is een fout opgetreden tijdens het opslaan: {ex.Message}");
            }
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                SelectedPath = DefaultExportPath
            };

            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
            {
                FilePathTextBox.Text = dialog.SelectedPath;
                FilePathTextBox.Foreground = Brushes.Black;
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            //SaveZaaglijsten(_sharedDataFilePath, _projectNumber, _projectName);

            SelectedSchedules = Zaaglijsten.Where(z => z.IsSelected).Select(z => z.Schedule).ToList();
            if (!SelectedSchedules.Any())
            {
                MessageBox.Show("Geen zaaglijsten geselecteerd voor export.");
                return;
            }

            if (string.IsNullOrEmpty(FilePathTextBox.Text) || !IsValidPath(FilePathTextBox.Text))
            {
                MessageBox.Show("Ongeldig exportpad. Geef een geldig pad op.");
                return;
            }

            try
            {
                foreach (var schedule in SelectedSchedules)
                {
                    var zaaglijst = Zaaglijsten.FirstOrDefault(z => z.Schedule == schedule);
                    ExportZaaglijst.ExportScheduleToCSV(schedule, FilePathTextBox.Text, zaaglijst);
                }
                MessageBox.Show("Geselecteerde zaaglijsten succesvol geëxporteerd!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Er is een fout opgetreden tijdens het exporteren: {ex.Message}");
            }
        }

        private void FilePathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FilePathTextBox.Text == DefaultExportPath)
            {
                FilePathTextBox.Foreground = Brushes.Gray;
            }
            else
            {
                FilePathTextBox.Foreground = Brushes.Black;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!this.DialogResult.HasValue)
            {
                this.DialogResult = false;
            }
        }

        private void SelectOrDeselectAll(bool select)
        {
            foreach (var zaaglijst in Zaaglijsten)
            {
                zaaglijst.IsSelected = select;
            }
        }

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
            catch
            {
                return false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class Zaaglijst : INotifyPropertyChanged
    {
        private bool isSelected;
        private string discipline;
        private string kavelNummer;
        private string verdieping;
        private string projectNummer;

        public string Name { get; set; }
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; OnPropertyChanged("IsSelected"); }
        }
        public string Discipline
        {
            get { return discipline; }
            set { discipline = value; OnPropertyChanged("Discipline"); }
        }
        public string KavelNummer
        {
            get { return kavelNummer; }
            set { kavelNummer = value; OnPropertyChanged("KavelNummer"); }
        }
        public string Verdieping
        {
            get { return verdieping; }
            set { verdieping = value; OnPropertyChanged("Verdieping"); }
        }
        public string ProjectNummer
        {
            get { return projectNummer; }
            set { projectNummer = value; OnPropertyChanged("ProjectNummer"); }
        }

        public ViewSchedule Schedule { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
