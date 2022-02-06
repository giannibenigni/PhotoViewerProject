using System;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace PhotoViewer.Utils
{
    public class PathToThumbnailConverter : IValueConverter
    {
        public int DecodeWidth
        {
            get;
            set;
        }

        public PathToThumbnailConverter()
        {
            DecodeWidth = 200;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var path = value as string;

            if (!string.IsNullOrEmpty(path))
            {



                BitmapImage bi = new BitmapImage();

                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.None;
                bi.DecodePixelWidth = DecodeWidth;
                bi.UriSource = new Uri(path);
                bi.EndInit();

                return bi;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
