using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WPFNetFrame48.Helpers;

namespace WPFNetFrame48.ViewModels
{
    public class TreeViewViewModel : Observable
    {
        private readonly Dictionary<string, ObservableCollection<FileManager>> _cache = new();
        private readonly string _cacheDirectory;
        private readonly string _structureCacheFile;
        private bool _isInitializing;

        public bool IsInitializing
        {
            get => _isInitializing;
            set
            {
                _isInitializing = value;
                OnPropertyChanged(nameof(IsInitializing));
            }
        }

        private int _initializingProgress;

        public int InitializingProgress
        {
            get => _initializingProgress;
            set
            {
                _initializingProgress = value;
                OnPropertyChanged(nameof(InitializingProgress));
            }
        }

        private int _totalFiles;
        private int _processedFiles;
        private CancellationTokenSource _cancellationTokenSource;
        private const int BatchSize = 25;

        public ObservableCollection<FileManager> RootFiles { get; private set; }
        public ObservableCollection<FileManager> SelectedFolderContent { get; private set; } = new();

        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        private int _loadingProgress;

        public int LoadingProgress
        {
            get => _loadingProgress;
            set
            {
                _loadingProgress = value;
                OnPropertyChanged(nameof(LoadingProgress));
            }
        }

        private FileManager _selectedFolder;

        public FileManager SelectedFolder
        {
            get => _selectedFolder;
            set
            {
                if (_selectedFolder != value)
                {
                    _selectedFolder = value;
                    OnPropertyChanged(nameof(SelectedFolder));
                    if (value != null)
                    {
                        LoadSelectedFolderContentAsync().ConfigureAwait(false);
                    }
                }
            }
        }

        public TreeViewViewModel()
        {
            RootFiles = new ObservableCollection<FileManager>();
            _cancellationTokenSource = new CancellationTokenSource();

            _cacheDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "NijhofAddIn",
                "ThumbnailCache"
            );
            Directory.CreateDirectory(_cacheDirectory);

            // Pad voor de gecachte mappenstructuur
            _structureCacheFile = Path.Combine(_cacheDirectory, "directory_structure.json");
        }

        public async Task InitializeViewAsync()
        {
            IsInitializing = true;
            try
            {
                // Eerst proberen de cache te laden
                await LoadCachedStructureAsync();

                // Dan op de achtergrond de echte structuur laden en vergelijken
                await Task.Run(async () =>
                {
                    var directories = LoadDirectories(@"F:\Stabiplan\Custom\Families");
                    await UpdateStructureAsync(directories);
                });

                // Start het laden van de thumbnails
                await InitializeAsync();

                // Laad alle bestanden van het eerste level na initialisatie
                if (RootFiles.Any())
                {
                    // Alle bestanden van het eerste level verzamelen
                    var allFirstLevelFiles = new List<FileManager>();
                    foreach (var rootFile in RootFiles)
                    {
                        if (rootFile.SubFiles.Any())
                        {
                            var files = rootFile.SubFiles.Where(f => !f.SubFiles.Any()).ToList();
                            allFirstLevelFiles.AddRange(files);
                        }
                    }

                    // Bestanden toevoegen aan SelectedFolderContent
                    foreach (var file in allFirstLevelFiles)
                    {
                        SelectedFolderContent.Add(file);
                    }

                    // Thumbnails laden in batches
                    var batches = allFirstLevelFiles
                        .Select((item, index) => new { Item = item, Index = index })
                        .GroupBy(x => x.Index / BatchSize)
                        .Select(g => g.Select(x => x.Item).ToList())
                        .ToList();

                    for (int batchIndex = 0; batchIndex < batches.Count; batchIndex++)
                    {
                        if (_cancellationTokenSource.Token.IsCancellationRequested)
                            break;

                        var batch = batches[batchIndex];
                        await LoadThumbnailsForBatchAsync(batch);
                    }
                }
            }
            finally
            {
                IsInitializing = false;
            }
        }

        private async Task LoadCachedStructureAsync()
        {
            try
            {
                if (File.Exists(_structureCacheFile))
                {
                    using (var stream = File.OpenRead(_structureCacheFile))
                    {
                        var options = new JsonSerializerOptions
                        {
                            Converters = { new FileManagerJsonConverter() }
                        };

                        var cachedStructure =
                            await JsonSerializer.DeserializeAsync<ObservableCollection<FileManager>>(stream, options);

                        if (cachedStructure != null)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                RootFiles.Clear();
                                foreach (var item in cachedStructure)
                                {
                                    RootFiles.Add(item);
                                }
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading cache: {ex.Message}");
            }
        }

        private async Task UpdateStructureAsync(ObservableCollection<FileManager> newStructure)
        {
            var structureChanged = false;

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (!CompareStructures(RootFiles, newStructure))
                {
                    RootFiles.Clear();
                    foreach (var item in newStructure)
                    {
                        RootFiles.Add(item);
                    }

                    structureChanged = true;
                }
            });

            if (structureChanged)
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Converters = { new FileManagerJsonConverter() }
                    };

                    using (var stream = File.Create(_structureCacheFile))
                    {
                        await JsonSerializer.SerializeAsync(stream, newStructure, options);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error saving cache: {ex.Message}");
                }
            }
        }

        private bool CompareStructures(IEnumerable<FileManager> current, IEnumerable<FileManager> newStructure)
        {
            if (current == null || newStructure == null)
                return false;

            if (current.Count() != newStructure.Count())
                return false;

            var currentList = current.ToList();
            var newList = newStructure.ToList();

            for (int i = 0; i < currentList.Count; i++)
            {
                if (currentList[i].ItemName != newList[i].ItemName ||
                    currentList[i].FilePath != newList[i].FilePath)
                    return false;

                if (!CompareStructures(currentList[i].SubFiles, newList[i].SubFiles))
                    return false;
            }

            return true;
        }

        private async Task InitializeAsync()
        {
            try
            {
                IsInitializing = true;
                InitializingProgress = 0;

                // Tel eerst het totaal aantal bestanden
                _totalFiles = 0;
                _processedFiles = 0;
                foreach (var rootFile in RootFiles)
                {
                    if (rootFile.SubFiles.Any())
                    {
                        var files = await GetAllFilesAsync(rootFile);
                        _totalFiles += files.Count;
                    }
                }

                // Laad nu alle thumbnails
                foreach (var rootFile in RootFiles)
                {
                    if (rootFile.SubFiles.Any())
                    {
                        var allFiles = await GetAllFilesAsync(rootFile);
                        foreach (var file in allFiles)
                        {
                            file.Thumbnail = GetThumbnail(file.FilePath, true);
                            _processedFiles++;
                            InitializingProgress = (_processedFiles * 100) / _totalFiles;
                        }
                    }
                }
            }
            finally
            {
                IsInitializing = false;
                InitializingProgress = 100;
            }
        }

        private async Task LoadDirectoriesAsync(string path)
        {
            var directories = await Task.Run(() => LoadDirectories(path));
            Application.Current.Dispatcher.Invoke(() =>
            {
                RootFiles.Clear();
                foreach (var directory in directories)
                {
                    RootFiles.Add(directory);
                }
            });
        }

        private ObservableCollection<FileManager> LoadDirectories(string path)
        {
            if (_cache.TryGetValue(path, out var cachedDirectories))
                return cachedDirectories;

            var items = new ObservableCollection<FileManager>();

            if (!Directory.Exists(path)) return items;

            try
            {
                foreach (var directory in Directory.GetDirectories(path))
                {
                    var node = new FileManager
                    {
                        ItemName = Path.GetFileName(directory),
                        FilePath = directory,
                        SubFiles = LoadDirectories(directory),
                        IsFilesLoaded = true
                    };
                    items.Add(node);
                }

                foreach (var file in Directory.GetFiles(path))
                {
                    var fileNode = new FileManager
                    {
                        ItemName = Path.GetFileName(file),
                        FilePath = file,
                        IsFilesLoaded = true
                    };
                    items.Add(fileNode);
                }

                _cache[path] = items;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading directory {path}: {ex.Message}");
            }

            return items;
        }

        private async Task LoadSelectedFolderContentAsync()
        {
            try
            {
                IsLoading = true;
                LoadingProgress = 0;

                SelectedFolderContent.Clear();

                if (SelectedFolder != null)
                {
                    var allFiles = await GetAllFilesAsync(SelectedFolder);

                    // Voeg eerst alle bestanden toe aan de view
                    foreach (var file in allFiles)
                    {
                        SelectedFolderContent.Add(file);
                    }

                    // Daarna laden we de thumbnails in batches
                    var batches = allFiles
                        .Select((item, index) => new { Item = item, Index = index })
                        .GroupBy(x => x.Index / BatchSize)
                        .Select(g => g.Select(x => x.Item).ToList())
                        .ToList();

                    for (int batchIndex = 0; batchIndex < batches.Count; batchIndex++)
                    {
                        if (_cancellationTokenSource.Token.IsCancellationRequested)
                            break;

                        var batch = batches[batchIndex];
                        await LoadThumbnailsForBatchAsync(batch);
                        LoadingProgress = ((batchIndex + 1) * 100) / batches.Count;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading folder content: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
                LoadingProgress = 100;
            }
        }

        private async Task LoadThumbnailsForBatchAsync(List<FileManager> batch)
        {
            await Task.Run(() =>
            {
                Parallel.ForEach(batch, new ParallelOptions
                {
                    CancellationToken = _cancellationTokenSource.Token,
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                },
                    file =>
                    {
                        if (file.Thumbnail == null)
                        {
                            file.Thumbnail = GetThumbnail(file.FilePath, true);
                        }
                    });
            });
        }

        private async Task<List<FileManager>> GetAllFilesAsync(FileManager folder)
        {
            var files = new List<FileManager>();

            void CollectFiles(FileManager currentFolder)
            {
                foreach (var item in currentFolder.SubFiles)
                {
                    if (!item.SubFiles.Any()) // This is a file
                    {
                        files.Add(item);
                    }
                    else // This is a folder
                    {
                        CollectFiles(item);
                    }
                }
            }

            await Task.Run(() => CollectFiles(folder));
            return files;
        }

        private BitmapImage GetThumbnail(string filePath, bool isHighQuality = true)
        {
            string cacheFilePath = GetCacheFilePath(filePath, isHighQuality);

            if (File.Exists(cacheFilePath))
            {
                try
                {
                    return LoadBitmapFromFile(cacheFilePath, isHighQuality);
                }
                catch
                {
                    try
                    {
                        File.Delete(cacheFilePath);
                    }
                    catch
                    {
                    }
                }
            }

            return GenerateAndCacheThumbnail(filePath, isHighQuality, cacheFilePath);
        }

        private string GetCacheFilePath(string filePath, bool isHighQuality)
        {
            using var md5 = MD5.Create();
            string hashInput = filePath + isHighQuality.ToString();
            byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(hashInput));
            string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            return Path.Combine(_cacheDirectory, $"{hash}.png");
        }

        private BitmapImage LoadBitmapFromFile(string filePath, bool isHighQuality)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(filePath);
            image.DecodePixelWidth = isHighQuality ? 256 : 64;
            image.DecodePixelHeight = isHighQuality ? 256 : 64;
            image.EndInit();
            image.Freeze();
            return image;
        }

        private BitmapImage GenerateAndCacheThumbnail(string filePath, bool isHighQuality, string cacheFilePath)
        {
            try
            {
                using ShellObject shellObject = ShellObject.FromParsingName(filePath);
                var bitmapSource = isHighQuality
                    ? shellObject.Thumbnail.ExtraLargeBitmapSource
                    : shellObject.Thumbnail.LargeBitmapSource;

                if (bitmapSource == null) return null;

                var bitmap = ConvertToBitmapImage(bitmapSource, isHighQuality ? 256 : 64);
                SaveToCache(bitmap, cacheFilePath);
                return bitmap;
            }
            catch
            {
                return null;
            }
        }

        private BitmapImage ConvertToBitmapImage(BitmapSource source, int size)
        {
            using var ms = new MemoryStream();
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

        private void SaveToCache(BitmapImage bitmap, string cacheFilePath)
        {
            try
            {
                using var fileStream = new FileStream(cacheFilePath, FileMode.Create);
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(fileStream);
            }
            catch
            {
            }
        }
    }

    public class FileManager : INotifyPropertyChanged
    {
        public FileManager()
        {
            SubFiles = new ObservableCollection<FileManager>();
        }

        public string ItemName { get; set; }
        public string FilePath { get; set; }
        public ObservableCollection<FileManager> SubFiles { get; set; }
        public bool IsFilesLoaded { get; set; }

        private BitmapImage _thumbnail;

        public BitmapImage Thumbnail
        {
            get => _thumbnail;
            set
            {
                _thumbnail = value;
                OnPropertyChanged(nameof(Thumbnail));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class FileManagerJsonConverter : JsonConverter<FileManager>
    {
        public override FileManager Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                var root = doc.RootElement;
                var fileManager = new FileManager
                {
                    ItemName = root.GetProperty("ItemName").GetString(),
                    FilePath = root.GetProperty("FilePath").GetString(),
                    IsFilesLoaded = root.GetProperty("IsFilesLoaded").GetBoolean()
                };

                if (root.TryGetProperty("SubFiles", out JsonElement subFiles))
                {
                    foreach (var subFile in subFiles.EnumerateArray())
                    {
                        var subFileManager = JsonSerializer.Deserialize<FileManager>(subFile.GetRawText(), options);
                        fileManager.SubFiles.Add(subFileManager);
                    }
                }

                return fileManager;
            }
        }

        public override void Write(Utf8JsonWriter writer, FileManager value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("ItemName", value.ItemName);
            writer.WriteString("FilePath", value.FilePath);
            writer.WriteBoolean("IsFilesLoaded", value.IsFilesLoaded);
            writer.WritePropertyName("SubFiles");
            JsonSerializer.Serialize(writer, value.SubFiles, options);
            writer.WriteEndObject();
        }
    }
}