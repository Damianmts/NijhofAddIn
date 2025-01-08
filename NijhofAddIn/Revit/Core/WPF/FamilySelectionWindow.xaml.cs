using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using NijhofAddIn.Revit.Commands.Tools.Content;
using System.Security.Cryptography;
using System.Text;

namespace NijhofAddIn.Revit.Core.WPF
{
    public partial class FamilySelectionWindow : Window
    {
        private readonly UIApplication _uiApp;
        private readonly string _rootFolder;
        private readonly string _cacheDirectory;
        private List<FamilyItem> _allFamilyItems;
        private ExternalEvent _placeFamilyEvent;
        private PlaceFamilyEventHandler _placeFamilyEventHandler;
        private ExternalEvent _loadFamilyEvent;
        private LoadFamilyEventHandler _loadFamilyEventHandler;
        private CancellationTokenSource _cancellationTokenSource;
        private const int BATCH_SIZE = 25;
        private bool _isHighQuality = false;

        public FamilySelectionWindow(List<string> familyFiles, UIApplication uiApp)
        {
            // Zet minimale threads
            ThreadPool.SetMinThreads(32, 32);

            InitializeComponent();
            _uiApp = uiApp;
            _rootFolder = @"F:\Stabiplan\Custom\Families";
            _allFamilyItems = new List<FamilyItem>();
            _cancellationTokenSource = new CancellationTokenSource();

            // Initialiseer cache directory
            _cacheDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "NijhofAddIn",
                "ThumbnailCache"
            );
            Directory.CreateDirectory(_cacheDirectory);

            PopulateTreeView();
            this.Loaded += OnWindowLoaded;

            _placeFamilyEventHandler = new PlaceFamilyEventHandler();
            _placeFamilyEvent = ExternalEvent.Create(_placeFamilyEventHandler);

            _loadFamilyEventHandler = new LoadFamilyEventHandler();
            _loadFamilyEvent = ExternalEvent.Create(_loadFamilyEventHandler);

            // Zet initieel de lage kwaliteit knop op disabled (geel)
            LowQualityButton.IsEnabled = false;
            HighQualityButton.IsEnabled = true;
        }

        private async void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => LoadAllFiles());

            FamiliesListView.ItemsSource = _allFamilyItems;
            FamiliesListView.Visibility = System.Windows.Visibility.Collapsed;
            ImagesListView.Visibility = System.Windows.Visibility.Visible;

            _isHighQuality = false; // Start with low quality

            var progressWindow = new ProgressWindowWPF
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            try
            {
                progressWindow.Show();
                progressWindow.UpdateStatusText("Afbeeldingen laden");
                await LoadThumbnailsAsync(progressWindow);
                ImagesListView.ItemsSource = _allFamilyItems;
            }
            finally
            {
                progressWindow.Close();
            }
        }

        private string GetCacheFilePath(string familyFilePath, bool isHighQuality)
        {
            // Genereer een unieke hash gebaseerd op het bestandspad en kwaliteit
            using (var md5 = MD5.Create())
            {
                string hashInput = familyFilePath + isHighQuality.ToString();
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(hashInput));
                string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return Path.Combine(_cacheDirectory, $"{hash}.png");
            }
        }

        private async Task LoadThumbnailsAsync(ProgressWindowWPF progressWindow)
        {
            var batches = _allFamilyItems
                .Select((item, index) => new { Item = item, Index = index })
                .GroupBy(x => x.Index / BATCH_SIZE)
                .Select(g => g.Select(x => x.Item).ToList())
                .ToList();

            for (int batchIndex = 0; batchIndex < batches.Count; batchIndex++)
            {
                if (_cancellationTokenSource.Token.IsCancellationRequested)
                    break;

                var batch = batches[batchIndex];

                await Task.Run(() =>
                {
                    Parallel.ForEach(batch, new ParallelOptions { CancellationToken = _cancellationTokenSource.Token }, item =>
                    {
                        item.Image = GetThumbnail(item.FilePath, _isHighQuality);
                    });
                });

                int progress = ((batchIndex + 1) * 100) / batches.Count;
                progressWindow.UpdateProgress(progress);
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
                    item.Items.Add(null);
                    item.Expanded += Folder_Expanded;
                    FolderTreeView.Items.Add(item);
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
                    _allFamilyItems.Add(new FamilyItem
                    {
                        Name = Path.GetFileName(filePath),
                        FilePath = filePath,
                        Type = "Family File"
                    });
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
            string cacheFilePath = GetCacheFilePath(filePath, large);

            // Controleer eerst de cache
            if (File.Exists(cacheFilePath))
            {
                try
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = new Uri(cacheFilePath);
                    image.DecodePixelWidth = large ? 256 : 64;
                    image.DecodePixelHeight = large ? 256 : 64;
                    image.EndInit();
                    image.Freeze();
                    return image;
                }
                catch
                {
                    // Bij een fout in het laden van de cache, verwijder het cache bestand
                    try { File.Delete(cacheFilePath); } catch { }
                }
            }

            // Als cache niet bestaat of ongeldig is, genereer nieuwe thumbnail
            try
            {
                using (ShellObject shellObject = ShellObject.FromParsingName(filePath))
                {
                    var bitmapSource = large ?
                        shellObject.Thumbnail.ExtraLargeBitmapSource :
                        shellObject.Thumbnail.LargeBitmapSource;

                    if (bitmapSource == null) return null;

                    // Converteer en sla op in cache
                    var bitmap = ConvertToBitmapImage(bitmapSource, large ? 256 : 64);
                    SaveToCache(bitmap, cacheFilePath);
                    return bitmap;
                }
            }
            catch
            {
                return null;
            }
        }

        private void SaveToCache(BitmapImage bitmap, string cacheFilePath)
        {
            try
            {
                using (var fileStream = new FileStream(cacheFilePath, FileMode.Create))
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmap));
                    encoder.Save(fileStream);
                }
            }
            catch { }
        }

        private BitmapImage ConvertToBitmapImage(BitmapSource source, int size)
        {
            using (var ms = new MemoryStream())
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(source));
                encoder.Save(ms);
                ms.Seek(0, SeekOrigin.Begin);

                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.DecodePixelWidth = size;
                image.DecodePixelHeight = size;
                image.EndInit();
                image.Freeze();
                return image;
            }
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
                        subItem.Items.Add(null);
                        subItem.Expanded += Folder_Expanded;
                        item.Items.Add(subItem);
                    }
                }
                catch { }
            }
        }

        private void FolderTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem selectedItem = FolderTreeView.SelectedItem as TreeViewItem;
            if (selectedItem != null)
            {
                string fullPath = (string)selectedItem.Tag;
                FilterFilesByFolder(fullPath);
            }
        }

        private void FilterFilesByFolder(string folderPath)
        {
            var filteredItems = _allFamilyItems.Where(item => item.FilePath.StartsWith(folderPath)).ToList();
            FamiliesListView.ItemsSource = filteredItems;
            ImagesListView.ItemsSource = filteredItems;
        }

        private async void QualityButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                // Reset beide buttons
                LowQualityButton.IsEnabled = true;
                HighQualityButton.IsEnabled = true;

                // Zet de geklikte button op disabled (voor de gele kleur)
                clickedButton.IsEnabled = false;

                // Update de kwaliteitsinstelling
                _isHighQuality = (clickedButton == HighQualityButton);

                // Wis alle bestaande afbeeldingen
                foreach (var item in _allFamilyItems)
                {
                    item.Image = null;
                }

                // Toon voortgangsvenster en herlaad thumbnails
                var progressWindow = new ProgressWindowWPF
                {
                    Owner = this,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                try
                {
                    progressWindow.Show();
                    progressWindow.UpdateStatusText(_isHighQuality ?
                        "Hoge kwaliteit afbeeldingen laden..." :
                        "Lage kwaliteit afbeeldingen laden...");

                    // Annuleer eventuele bestaande laadoperaties
                    _cancellationTokenSource.Cancel();
                    _cancellationTokenSource = new CancellationTokenSource();

                    await LoadThumbnailsAsync(progressWindow);

                    // Ververs de ListView
                    ImagesListView.Items.Refresh();
                }
                finally
                {
                    progressWindow.Close();
                }
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> familyPaths = new List<string>();

            if (FamiliesListView.Visibility == System.Windows.Visibility.Visible)
            {
                foreach (FamilyItem item in FamiliesListView.SelectedItems)
                {
                    familyPaths.Add(item.FilePath);
                }
            }
            else if (ImagesListView.Visibility == System.Windows.Visibility.Visible)
            {
                foreach (FamilyItem item in ImagesListView.SelectedItems)
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

        private void PlaceButton_Click(object sender, RoutedEventArgs e)
        {
            FamilyItem selectedItem = null;

            if (FamiliesListView.Visibility == System.Windows.Visibility.Visible && FamiliesListView.SelectedItems.Count > 0)
            {
                selectedItem = (FamilyItem)FamiliesListView.SelectedItems[0];
            }
            else if (ImagesListView.Visibility == System.Windows.Visibility.Visible && ImagesListView.SelectedItems.Count > 0)
            {
                selectedItem = (FamilyItem)ImagesListView.SelectedItems[0];
            }

            if (selectedItem == null)
            {
                MessageBox.Show("Selecteer een familie om te plaatsen.", "Geen selectie", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            List<string> familyPaths = new List<string> { selectedItem.FilePath };
            _loadFamilyEventHandler.SetFamiliesToLoad(familyPaths);
            _loadFamilyEvent.Raise();

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Tick += (s, args) =>
            {
                Family loadedFamily = GetLoadedFamily(selectedItem.FilePath);
                if (loadedFamily != null)
                {
                    dispatcherTimer.Stop();
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

            return null;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SearchBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string searchText = SearchBox.Text.ToLower();
            var filteredItems = _allFamilyItems.Where(item => item.Name.ToLower().Contains(searchText)).ToList();
            FamiliesListView.ItemsSource = filteredItems;
            ImagesListView.ItemsSource = filteredItems;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchPlaceholder.Visibility = string.IsNullOrWhiteSpace(SearchBox.Text)
                ? System.Windows.Visibility.Visible
                : System.Windows.Visibility.Collapsed;
        }

        protected override void OnClosed(EventArgs e)
        {
            _cancellationTokenSource.Cancel();

            // Optioneel: verouderde cache bestanden opruimen
            CleanupOldCache();

            GC.Collect();
            base.OnClosed(e);
        }

        private void CleanupOldCache()
        {
            try
            {
                // Verwijder cache bestanden ouder dan 30 dagen
                var cutoffDate = DateTime.Now.AddDays(-30);
                foreach (var file in Directory.GetFiles(_cacheDirectory, "*.png"))
                {
                    if (File.GetLastAccessTime(file) < cutoffDate)
                    {
                        try { File.Delete(file); } catch { }
                    }
                }
            }
            catch { }
        }

        public class FamilyItem
        {
            public string Name { get; set; }
            public string FilePath { get; set; }
            public string Type { get; set; }
            public BitmapImage Image { get; set; }
        }
    }
}