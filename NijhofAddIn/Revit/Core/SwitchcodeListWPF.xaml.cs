using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace NijhofAddIn.Revit.Core
{
    public partial class SwitchcodeListWPF : Window
    {
        public SwitchcodeListWPF(Dictionary<string, int> switchCodeCounts)
        {
            InitializeComponent();

            var displayList = switchCodeCounts
                .Select(kvp => kvp.Value > 0 ? $"{kvp.Key} - {kvp.Value}" : kvp.Key)
                .ToList();

            Box.ItemsSource = displayList;
        }

        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}
