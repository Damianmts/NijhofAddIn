﻿using Autodesk.Revit.UI;
using Microsoft.WindowsAPICodePack.Shell;
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

        public List<string> SelectedFamilies { get; private set; }

        public FamilySelectionWindow(List<string> familyFiles, UIApplication uiApp)
        {
            InitializeComponent();
            _uiApp = uiApp;
            _rootFolder = @"F:\Stabiplan\Custom\Families";
            _allFamilyItems = new List<FamilyItem>();
            PopulateTreeView();
            this.Loaded += OnWindowLoaded; // Laad de bestanden nadat het venster is geopend
        }

        private async void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Laad de bestanden asynchroon nadat het venster is getoond
            await Task.Run(() => LoadAllFiles());
            listViewFamilies.ItemsSource = _allFamilyItems;
            itemsControlImages.ItemsSource = _allFamilyItems;
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
            itemsControlImages.ItemsSource = filteredItems;
        }

        private BitmapImage GetThumbnail(string filePath, bool large)
        {
            using (ShellObject shellObject = ShellObject.FromParsingName(filePath))
            {
                var bitmapSource = large ? shellObject.Thumbnail.LargeBitmapSource : shellObject.Thumbnail.SmallBitmapSource;
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
                        bitmapImage.DecodePixelWidth = 256; // Grotere resolutie voor de afbeeldingenweergave
                        bitmapImage.DecodePixelHeight = 256;
                    }
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                }
                return bitmapImage;
            }
        }

        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            SelectedFamilies = new List<string>();
            foreach (FamilyItem item in listViewFamilies.SelectedItems)
            {
                SelectedFamilies.Add(item.FilePath);
            }
            this.DialogResult = true;
            this.Close();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        // Nieuw toegevoegd: Event handler voor het wisselen van weergave
        private void comboBoxViewMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Controleer of de ListView objecten niet null zijn voordat je ze gebruikt
            if (listViewFamilies == null || itemsControlImages == null)
            {
                return; // Verlaat de methode als een van de objecten null is
            }

            if (comboBoxViewMode.SelectedIndex == 0) // "Lijst" optie geselecteerd
            {
                listViewFamilies.Visibility = Visibility.Visible;
                itemsControlImages.Visibility = Visibility.Collapsed;
            }
            else if (comboBoxViewMode.SelectedIndex == 1) // "Afbeeldingen" optie geselecteerd
            {
                // Update de thumbnails naar grote versies
                foreach (var item in _allFamilyItems)
                {
                    item.Image = GetThumbnail(item.FilePath, true); // Grote afbeelding voor de afbeeldingenweergave
                }
                listViewFamilies.Visibility = Visibility.Collapsed;
                itemsControlImages.Visibility = Visibility.Visible;
                itemsControlImages.ItemsSource = null;
                itemsControlImages.ItemsSource = _allFamilyItems; // Herlaad de items om de grote afbeeldingen weer te geven
            }
        }

        // Nieuw toegevoegd: Event handler voor de zoekfunctie
        private void SearchBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string searchText = searchBox.Text.ToLower();
            var filteredItems = _allFamilyItems.Where(item => item.Name.ToLower().Contains(searchText)).ToList();
            listViewFamilies.ItemsSource = filteredItems;
            itemsControlImages.ItemsSource = filteredItems;
        }

        // Nieuw toegevoegd: Placeholder logica voor de zoekbalk
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            placeholderText.Visibility = string.IsNullOrWhiteSpace(searchBox.Text) ? Visibility.Visible : Visibility.Collapsed;
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
