using System;
using System.Windows.Media.Imaging;

namespace PhotoViewer.Utils
{
    public class MetadataWrap
    {
        #region Variables

        private readonly BitmapMetadata metadata;

        #endregion

        #region Properties

        public uint? Width
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif/subifd:{uint=40962}");
                if (val == null)
                {
                    return null;
                }
                if (val.GetType() == typeof(uint))
                {
                    return (uint?)val;
                }
                return Convert.ToUInt32(val);
            }
        }

        public uint? Height
        {
            get
            {
                var val = QueryMetadata("/app1/ifd/exif/subifd:{uint=40963}");
                if (val == null)
                {
                    return null;
                }
                if (val.GetType() == typeof(uint))
                {
                    return (uint?)val;
                }
                return Convert.ToUInt32(val);
            }
        }


        public string Dimensions => $"{Width} x {Height}";

        #endregion

        public MetadataWrap(string url)
        {
            metadata = (BitmapMetadata)BitmapFrame.Create(new Uri(url), BitmapCreateOptions.DelayCreation, BitmapCacheOption.None).Metadata;
        }

        private object QueryMetadata(string query)
        {
            if (metadata.ContainsQuery(query))
                return metadata.GetQuery(query);
            return null;
        }
    }
}
