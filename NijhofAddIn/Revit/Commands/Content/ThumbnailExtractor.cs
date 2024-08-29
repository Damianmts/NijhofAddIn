using Microsoft.WindowsAPICodePack.Shell;
using System.IO;
using System.Windows.Media.Imaging;

namespace NijhofAddIn.Revit.Commands.Content
{
    public static class ThumbnailExtractor
    {
        public static BitmapImage GetThumbnail(string filePath)
        {
            using (ShellObject shellObject = ShellObject.FromParsingName(filePath))
            {
                // Krijg een kleine thumbnail; je kunt ook grotere maten kiezen
                var bitmapSource = shellObject.Thumbnail.SmallBitmapSource;

                // Converteer BitmapSource naar BitmapImage
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
    }
}
