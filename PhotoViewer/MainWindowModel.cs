using PhotoViewer.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PhotoViewer
{
    public class MainWindowModel : Base
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
            foreach (var file in directory.GetFiles().Where(a => a.Extension.Contains(".jpg") || a.Extension.Contains(".jpeg")))
            {
                FolderItems.Add(new FileWrap
                {
                    Path = file.FullName,
                    Name = file.Name,
                    Metadata = new MetadataWrap(file.FullName),
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

        private BitmapSource CreateBitmapSource(Uri path)
        {
            BitmapImage bmpImage = new BitmapImage();
            bmpImage.BeginInit();
            bmpImage.UriSource = path;
            bmpImage.EndInit();
            return bmpImage;
        }

        private BitmapSource CreateThumbnail(Uri path)
        {
            BitmapImage bmpImage = new BitmapImage();
            bmpImage.BeginInit();
            bmpImage.UriSource = path;
            bmpImage.DecodePixelWidth = 120;
            bmpImage.EndInit();
            //if (bmpImage.Width > bmpImage.Height)
            //else
            //    bmpImage.DecodePixelHeight = 120;

            return bmpImage;
        }

       

    }

    public class Base : INotifyPropertyChanged
    {
        #region Event

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion
    }

}
