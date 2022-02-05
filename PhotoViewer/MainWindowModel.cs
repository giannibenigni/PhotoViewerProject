using PhotoViewer.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PhotoViewer
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        private int zoomLevel = 160;
        #region Properties

        public List<DriveWrap> HardDisks { get; set; } = new List<DriveWrap>();

        public List<FileWrap> FolderItems { get; set; } = new List<FileWrap>();

        public int ZoomLevel
        {
            get => zoomLevel;
            set
            {
                zoomLevel = value;
                OnPropertyChanged(nameof(ZoomLevel));
            }
        }
        #endregion

        #region Commands

        public RelayCommand ItemSelectedCommand => new RelayCommand(o => true, a => ItemSelected(a as ItemWrap));

        #endregion


        public MainWindowModel()
        {

        }

        public void ItemSelected(ItemWrap item)
        {
            if (item == null)
                return;

            FolderItems = new List<FileWrap>();

            var directory = new DirectoryInfo(item.Path);
            foreach (var file in directory.GetFiles("*.jpg"))
            {
                FolderItems.Add(new FileWrap
                {
                    Image = BitmapFrame.Create(new Uri(file.FullName)),
                    Name = file.Name,
                });
            }
            OnPropertyChanged(nameof(FolderItems));
        }

        public void Start()
        {
            Task.Run(() =>
            {
                foreach (string s in Directory.GetLogicalDrives())
                {
                    var disk = new DriveWrap();
                    disk.Name = s;

                    HardDisks.Add(disk);

                    OnPropertyChanged(nameof(HardDisks));

                    Task.Run(() => GetFolders(s, disk.Children));
                }
            });
        }

        public void GetFolders(string path, List<ItemWrap> list)
        {
            try
            {
                foreach (var item in Directory.GetDirectories(path))
                {
                    var folder = new ItemWrap();
                    folder.Name = Path.GetFileName(item);
                    folder.Path = item;

                    list.Add(folder);

                    OnPropertyChanged(nameof(HardDisks));

                    GetFolders(item, folder.Children);
                }
            }
            catch
            {

            }
        }

        public void GetFiles(string path, List<ItemWrap> list)
        {
            try
            {
                foreach (var item in Directory.GetFiles(path))
                {
                    var file = new ItemWrap();
                    file.Name = Path.GetFileName(item);

                    list.Add(file);
                }
            }
            catch
            {
            }
        }

        public Size GetThumbnailSize(Image original)
        {
            // Maximum size of any dimension.
            const int maxPixels = 40;

            // Width and height.
            int originalWidth = original.Width;
            int originalHeight = original.Height;

            // Compute best factor to scale entire image based on larger dimension.
            double factor;
            if (originalWidth > originalHeight)
            {
                factor = (double)maxPixels / originalWidth;
            }
            else
            {
                factor = (double)maxPixels / originalHeight;
            }

            // Return thumbnail size.
            return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
        }

        public BitmapImage ImageFromBuffer(Byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            image.Freeze();
            return image;
        }


        #region Event

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

    }
}
