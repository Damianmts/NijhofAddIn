﻿using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NijhofAddIn.Revit.Core
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

            DialogResult = true;
            Close();
        }
    }
}