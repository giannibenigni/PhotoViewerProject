using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PhotoViewer.Utils
{
    public class FileWrap
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public MetadataWrap Metadata { get; set; }
    }
}
