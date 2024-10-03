using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.WindowsAPICodePack.Shell;
using NijhofAddIn.Revit.Commands.Content;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Syncfusion.UI.Xaml.TreeView; // Zorg dat je de juiste Syncfusion namespace hebt
using Syncfusion.UI.Xaml.TreeView.Engine;

namespace NijhofAddIn.Revit.Core
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
        public ObservableCollection<FolderItem> Folders { get; set; }

        public FamilySelectionWindow(List<string> familyFiles, UIApplication uiApp)
        {
            InitializeComponent();
            _uiApp = uiApp;
            _rootFolder = @"F:\Stabiplan\Custom\Families";
            _allFamilyItems = new List<FamilyItem>();
            Folders = new ObservableCollection<FolderItem>();
            PopulateTreeView();
            this.Loaded += OnWindowLoaded; // Laad de bestanden nadat het venster is geopend

            _placeFamilyEventHandler = new PlaceFamilyEventHandler();
            _placeFamilyEvent = ExternalEvent.Create(_placeFamilyEventHandler);

            _loadFamilyEventHandler = new LoadFamilyEventHandler();
            _loadFamilyEvent = ExternalEvent.Create(_loadFamilyEventHandler);

            DataContext = this; // Stel de DataContext in voor databinding
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
                    var folderItem = new FolderItem
                    {
                        Name = Path.GetFileName(directory),
                        Path = directory
                    };

                    // Voeg een dummy-item toe om lazy loading te ondersteunen
                    if (Directory.GetDirectories(directory).Any())
                    {
                        folderItem.SubFolders.Add(new FolderItem { Name = "Loading..." });
                    }

                    Folders.Add(folderItem);
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
                    bitmapImage.DecodePixelWidth = large ? 256 : 128; // Gebruik hogere resolutie voor grote afbeeldingen
                    bitmapImage.DecodePixelHeight = large ? 256 : 128;
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

        // Toevoeging van de folderTreeView_SelectionChanged event handler
        private void folderTreeView_SelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            // Haal het geselecteerde item op
            var selectedItem = e.AddedItems.FirstOrDefault();

            if (selectedItem != null)
            {
                var folderItem = selectedItem as FolderItem;
                if (folderItem != null)
                {
                    MessageBox.Show($"Geselecteerd: {folderItem.Name}");

                    // Filter de bestanden op basis van het geselecteerde pad
                    FilterFilesByFolder(folderItem.Path);
                }
            }
        }

        private void FilterFilesByFolder(string folderPath)
        {
            // Dit is een voorbeeld van hoe je de bestanden kunt filteren op basis van het geselecteerde pad
            var filteredItems = _allFamilyItems.Where(item => item.FilePath.StartsWith(folderPath)).ToList();

            // Toon de gefilterde items in je ListView of andere interface
            listViewFamilies.ItemsSource = filteredItems;
            listViewImages.ItemsSource = filteredItems;
        }
    }

    public class FamilyItem
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string Type { get; set; }
        public BitmapImage Image { get; set; }
    }

    public class FolderItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public ObservableCollection<FolderItem> SubFolders { get; set; }

        public FolderItem()
        {
            SubFolders = new ObservableCollection<FolderItem>();
        }
    }
}
