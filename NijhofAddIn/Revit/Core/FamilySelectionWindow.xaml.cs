using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace NijhofAddIn.Revit.Commands.Content
{
    public partial class FamilySelectionWindow : Window
    {
        private readonly UIApplication _uiApp;
        private readonly string _rootFolder;
        private List<FamilyItem> _allFamilyItems;
        private ExternalEvent _placeFamilyEvent;
        private PlaceFamilyEventHandler _placeFamilyEventHandler;
        private ExternalEvent _loadFamilyEvent;
        private LoadFamilyEventHandler _loadFamilyEventHandler;

        public FamilySelectionWindow(List<string> familyFiles, UIApplication uiApp)
        {
            InitializeComponent();
            _uiApp = uiApp;
            _rootFolder = @"F:\Stabiplan\Custom\Families";
            _allFamilyItems = new List<FamilyItem>();
            PopulateTreeView();
            this.Loaded += OnWindowLoaded; // Laad de bestanden nadat het venster is geopend

            _placeFamilyEventHandler = new PlaceFamilyEventHandler();
            _placeFamilyEvent = ExternalEvent.Create(_placeFamilyEventHandler);

            _loadFamilyEventHandler = new LoadFamilyEventHandler();
            _loadFamilyEvent = ExternalEvent.Create(_loadFamilyEventHandler);
        }

        private async void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Laad de bestanden asynchroon nadat het venster is getoond
            await Task.Run(() => LoadAllFiles());
            listViewFamilies.ItemsSource = _allFamilyItems;

            // Zet standaard de view op afbeeldingenweergave
            comboBoxViewMode.SelectedIndex = 1;
            listViewFamilies.Visibility = System.Windows.Visibility.Collapsed;
            listViewImages.Visibility = System.Windows.Visibility.Visible;

            // Optimaliseer thumbnails voor afbeeldingenweergave direct na het laden
            OptimizeThumbnailsForImages();
            listViewImages.ItemsSource = _allFamilyItems;
        }

        private void OptimizeThumbnailsForImages()
        {
            foreach (var item in _allFamilyItems)
            {
                item.Image = GetThumbnail(item.FilePath, true); // Grote afbeelding voor de afbeeldingenweergave
            }
        }

        private void PopulateTreeView()
        {
            try
            {
                foreach (string directory in Directory.GetDirectories(_rootFolder))
                {
                    TreeViewItem item = new TreeViewItem
                    {
                        Header = Path.GetFileName(directory),
                        Tag = directory
                    };
                    item.Items.Add(null); // Placeholder voor lazy loading
                    item.Expanded += Folder_Expanded;
                    folderTreeView.Items.Add(item);
                }
            }
            catch { }
        }

        private void LoadAllFiles()
        {
            _allFamilyItems = new List<FamilyItem>();
            LoadFilesRecursively(_rootFolder);
        }

        private void LoadFilesRecursively(string folderPath)
        {
            try
            {
                foreach (string filePath in Directory.GetFiles(folderPath, "*.rfa"))
                {
                    var thumbnail = GetThumbnail(filePath, false);
                    if (thumbnail != null)
                    {
                        _allFamilyItems.Add(new FamilyItem
                        {
                            Name = Path.GetFileName(filePath),
                            FilePath = filePath,
                            Type = "Family File",
                            Image = thumbnail // Kleine afbeelding voor de lijstweergave
                        });
                    }
                }

                foreach (string subDir in Directory.GetDirectories(folderPath))
                {
                    LoadFilesRecursively(subDir);
                }
            }
            catch { }
        }

        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            if (item.Items.Count == 1 && item.Items[0] == null)
            {
                item.Items.Clear();
                string fullPath = (string)item.Tag;
                try
                {
                    foreach (string directory in Directory.GetDirectories(fullPath))
                    {
                        TreeViewItem subItem = new TreeViewItem
                        {
                            Header = Path.GetFileName(directory),
                            Tag = directory
                        };
                        subItem.Items.Add(null); // Placeholder voor lazy loading
                        subItem.Expanded += Folder_Expanded;
                        item.Items.Add(subItem);
                    }
                }
                catch { }
            }
        }

        private void folderTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem selectedItem = folderTreeView.SelectedItem as TreeViewItem;
            if (selectedItem != null)
            {
                string fullPath = (string)selectedItem.Tag;
                FilterFilesByFolder(fullPath);
            }
        }

        private void FilterFilesByFolder(string folderPath)
        {
            var filteredItems = _allFamilyItems.Where(item => item.FilePath.StartsWith(folderPath)).ToList();
            listViewFamilies.ItemsSource = filteredItems;
            listViewImages.ItemsSource = filteredItems;
        }

        private BitmapImage GetThumbnail(string filePath, bool large)
        {
            using (ShellObject shellObject = ShellObject.FromParsingName(filePath))
            {
                var bitmapSource = large ? shellObject.Thumbnail.LargeBitmapSource : shellObject.Thumbnail.MediumBitmapSource;
                if (bitmapSource == null) return null; // Vermijd het toevoegen van lege items

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
                    if (large)
                    {
                        bitmapImage.DecodePixelWidth = 256; // Verhoog de resolutie voor de afbeeldingenweergave
                        bitmapImage.DecodePixelHeight = 256;
                    }
                    else
                    {
                        bitmapImage.DecodePixelWidth = 128; // Gebruik een iets grotere resolutie voor de lijstweergave
                        bitmapImage.DecodePixelHeight = 128;
                    }
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                }
                return bitmapImage;
            }
        }

        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            List<string> familyPaths = new List<string>();

            if (listViewFamilies.Visibility == System.Windows.Visibility.Visible)
            {
                foreach (FamilyItem item in listViewFamilies.SelectedItems)
                {
                    familyPaths.Add(item.FilePath);
                }
            }
            else if (listViewImages.Visibility == System.Windows.Visibility.Visible)
            {
                foreach (FamilyItem item in listViewImages.SelectedItems)
                {
                    familyPaths.Add(item.FilePath);
                }
            }

            if (familyPaths.Count == 0)
            {
                MessageBox.Show("Selecteer een familie om te laden.", "Geen selectie", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _loadFamilyEventHandler.SetFamiliesToLoad(familyPaths);
            _loadFamilyEvent.Raise();
        }

        private void buttonPlace_Click(object sender, RoutedEventArgs e)
        {
            FamilyItem selectedItem = null;

            if (listViewFamilies.Visibility == System.Windows.Visibility.Visible && listViewFamilies.SelectedItems.Count > 0)
            {
                selectedItem = (FamilyItem)listViewFamilies.SelectedItems[0];
            }
            else if (listViewImages.Visibility == System.Windows.Visibility.Visible && listViewImages.SelectedItems.Count > 0)
            {
                selectedItem = (FamilyItem)listViewImages.SelectedItems[0];
            }

            if (selectedItem == null)
            {
                MessageBox.Show("Selecteer een familie om te plaatsen.", "Geen selectie", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 1. Laad de familie
            List<string> familyPaths = new List<string> { selectedItem.FilePath };
            _loadFamilyEventHandler.SetFamiliesToLoad(familyPaths);
            _loadFamilyEvent.Raise();

            // Controleer continu of de familie is geladen en start dan de plaatsing
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1); // Controleer elke seconde
            dispatcherTimer.Tick += (s, args) =>
            {
                // Controleer of de familie is geladen
                Family loadedFamily = GetLoadedFamily(selectedItem.FilePath);
                if (loadedFamily != null)
                {
                    dispatcherTimer.Stop();  // Stop de timer zodra de familie is geladen
                                             // 2. Plaats de familie met PostRequestForElementTypePlacement
                    _placeFamilyEventHandler.SetFamilyToPlace(loadedFamily);
                    _placeFamilyEvent.Raise();
                }
            };
            dispatcherTimer.Start();
        }

        private Family GetLoadedFamily(string familyPath)
        {
            string familyName = Path.GetFileNameWithoutExtension(familyPath);
            Document doc = _uiApp.ActiveUIDocument.Document;

            foreach (Family family in new FilteredElementCollector(doc).OfClass(typeof(Family)).ToElements())
            {
                if (family.Name.Equals(familyName))
                {
                    return family;
                }
            }

            return null; // Familie is niet gevonden
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Sluit het huidige venster
        }

        private void comboBoxViewMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewFamilies == null || listViewImages == null)
            {
                return;
            }

            if (comboBoxViewMode.SelectedIndex == 0) // "Lijst" optie geselecteerd
            {
                listViewFamilies.Visibility = System.Windows.Visibility.Visible;
                listViewImages.Visibility = System.Windows.Visibility.Collapsed;
            }
            else if (comboBoxViewMode.SelectedIndex == 1) // "Afbeeldingen" optie geselecteerd
            {
                OptimizeThumbnailsForImages(); // Zorg ervoor dat thumbnails van hoge kwaliteit worden gebruikt
                listViewFamilies.Visibility = System.Windows.Visibility.Collapsed;
                listViewImages.Visibility = System.Windows.Visibility.Visible;
                listViewImages.ItemsSource = null;
                listViewImages.ItemsSource = _allFamilyItems; // Herlaad de items om de grote afbeeldingen weer te geven
            }
        }

        private void SearchBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string searchText = searchBox.Text.ToLower();
            var filteredItems = _allFamilyItems.Where(item => item.Name.ToLower().Contains(searchText)).ToList();
            listViewFamilies.ItemsSource = filteredItems;
            listViewImages.ItemsSource = filteredItems;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            placeholderText.Visibility = string.IsNullOrWhiteSpace(searchBox.Text) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }
    }

    public class FamilyItem
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string Type { get; set; }
        public BitmapImage Image { get; set; }
    }
}