using PhotoViewer.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoViewer
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        #region Properties

        public List<DriveWrap> HardDisks { get; set; } = new List<DriveWrap>();

        #endregion


        public MainWindowModel()
        {

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

                    list.Add(folder);

                    OnPropertyChanged(nameof(HardDisks));

                    GetFolders(item, folder.Children);
                    GetFiles(item, folder.Children);
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
                    file.IsFile = true;

                    list.Add(file);
                }
            }
            catch
            {
            }
        }

        #region Event

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

    }
}
