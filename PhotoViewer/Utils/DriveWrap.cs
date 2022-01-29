using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoViewer.Utils
{
    public class DriveWrap
    {
        public string Name { get; set; }

        public List<ItemWrap> Children { get; set; } = new List<ItemWrap>();
    }
}
