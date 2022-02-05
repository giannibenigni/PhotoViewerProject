using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoViewer.Utils
{
    public class ItemWrap
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public List<ItemWrap> Children { get; set; } = new List<ItemWrap>();
    }
}
