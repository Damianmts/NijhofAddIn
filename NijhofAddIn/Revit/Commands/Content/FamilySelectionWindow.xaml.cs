using Autodesk.Revit.UI;
using Microsoft.WindowsAPICodePack.Shell;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using ricaun.Revit.UI;
using ricaun.Revit.UI.StatusBar;
using ricaun.Revit.UI.StatusBar.Utils;

namespace NijhofAddIn.Revit.Commands.Content
{
    public partial class FamilySelectionWindow : Window
    {
        private readonly UIApplication _uiApp;
        private readonly List<string> _familyFiles;

        public List<string> SelectedFamilies { get; private set; }

        public FamilySelectionWindow(List<string> familyFiles, UIApplication uiApp)
        {
            InitializeComponent();
            _uiApp = uiApp;
            _familyFiles = familyFiles;
            this.Loaded += OnWindowLoaded;
        }

        private async void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            await LoadFamilyDataAsync();
        }

        private async Task LoadFamilyDataAsync()
        {
            List<FamilyItem> familyItems = new List<FamilyItem>();

            foreach (var filePath in _familyFiles)
            {
                var name = System.IO.Path.GetFileName(filePath);
                BitmapImage image = null;

                try
                {
                    image = await Task.Run(() => GetThumbnail(filePath));
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Error loading thumbnail: {ex.Message}");
                }

                familyItems.Add(new FamilyItem
                {
                    Name = name,
                    FilePath = filePath,
                    Image = image
                });
            }

            listViewFamilies.ItemsSource = familyItems;
        }

        private BitmapImage GetThumbnail(string filePath)
        {
            using (ShellObject shellObject = ShellObject.FromParsingName(filePath))
            {
                var bitmapSource = shellObject.Thumbnail.SmallBitmapSource;
                BitmapImage bitmapImage = new BitmapImage();
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                using (MemoryStream stream = new MemoryStream())
                {
                    encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    encoder.Save(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = stream;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                }
                return bitmapImage;
            }
        }

        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            SelectedFamilies = listViewFamilies.SelectedItems.Cast<FamilyItem>().Select(i => i.FilePath).ToList();
            this.DialogResult = true;
            this.Close();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }

    public class FamilyItem
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public BitmapImage Image { get; set; }
    }
}
