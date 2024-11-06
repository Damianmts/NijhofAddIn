using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;

namespace NijhofAddIn.Revit.Core.WPF
{
    public class ScheduleItem : INotifyPropertyChanged
    {
        private bool _isSelected;
        public string Name { get; set; }
        public ViewSchedule Schedule { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public partial class MateriaallijstWPF : Window
    {
        public List<ScheduleItem> ScheduleItems { get; private set; }
        public List<ViewSchedule> SelectedSchedules { get; private set; }
        private bool isCancelled = true; // Flag to track if the operation was cancelled

        public MateriaallijstWPF(ICollection<Element> schedules)
        {
            InitializeComponent();
            ScheduleItems = new List<ScheduleItem>();

            foreach (var element in schedules)
            {
                if (element is ViewSchedule schedule)
                {
                    ScheduleItems.Add(new ScheduleItem { Name = schedule.Name, Schedule = schedule });
                }
            }

            ScheduleListBox.ItemsSource = ScheduleItems;

            this.Closing += MateriaallijstWPF_Closing;
        }

        private void MateriaallijstWPF_Closing(object sender, CancelEventArgs e)
        {
            if (isCancelled)
            {
                this.DialogResult = false; // Prevent the "cancelled by user" message
            }
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PathTextBox.Text = dialog.SelectedPath;
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedSchedules = ScheduleItems.Where(item => item.IsSelected).Select(item => item.Schedule).ToList();
            if (SelectedSchedules == null || !SelectedSchedules.Any())
            {
                System.Windows.MessageBox.Show("No schedules selected for export.");
                return;
            }

            string path = PathTextBox.Text;
            if (string.IsNullOrEmpty(path))
            {
                System.Windows.MessageBox.Show("Please select a folder to save the files.");
                return;
            }

            isCancelled = false; // Export was successful, not cancelled
            DialogResult = true;
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // This will trigger the Closing event
        }
    }
}
