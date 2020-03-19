using System.Drawing;
using System.Windows.Media;

using AlbumCreator.Common.Utils;

namespace AlbumCreator.Models
{
    public class PrevImage
    {
        public string Source { get; set; }
        public ImageSource Preview { get; set; }

        public PrevImage(string source)
        {
            Source = source;

            var image = Image.FromFile(source);
            Preview = ImageUtils.ImageSourceFromBitmap(
                ImageUtils.ResizeImage(image, 100, (int)(100 * ((float)image.Height / image.Width))));
        }
    }
}
