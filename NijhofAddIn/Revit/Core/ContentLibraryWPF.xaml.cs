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
    public partial class ContentLibraryWPF : Window
    {
        public ContentLibraryWPF()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzUwOTM3MUAzMjM3MmUzMDJlMzBUTlFnU05NSXpIMGE0TDkrN09HcW9RbUtiRzJuSXR2R0huS21Cc1R5UjFjPQ==");

            InitializeComponent();

            string imagePath = "pack://application:,,,/NijhofAddIn;component/Resources/error-32.png";

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imagePath, UriKind.Absolute); // Absolute URI
            bitmap.EndInit();

            WindowIcon.Source = bitmap;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Controleer of de muisklik in de bovenste 40 pixels is
            if (e.GetPosition(this).Y <= 40)
            {
                DragMove();
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
