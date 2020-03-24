using AlbumCreator.Common.Enums;
using AlbumCreator.Common.Interfaces;
using AlbumCreator.Common.Utils;
using System.Drawing;
using System.Windows.Media;

namespace AlbumCreator.Models
{
    public class EditablePicture : IPicture
    {
        private string _source;
        public string Source { get => _source; set { _source = value; SourceChanged(); } }
        public string Name { get; set; }
        public PictureType Type { get; set; }
        public ImageSource Preview { get; private set; }

        public void Reset(IPicture picture)
        {
            Source = picture.Source;
            Name = picture.Name;
            Type = picture.Type;
        }

        private void SourceChanged()
        {
            var image = Image.FromFile(Source);
            Preview = ImageUtils.ImageSourceFromBitmap(
                ImageUtils.ResizeImage(image, 400, (int)(400 * ((float)image.Height / image.Width))));
        }
    }
}
