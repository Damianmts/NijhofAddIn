using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace NijhofAddIn.Revit.Core.WPF
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
