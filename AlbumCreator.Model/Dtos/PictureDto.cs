using AlbumCreator.Common.Enums;
using AlbumCreator.Common.Interfaces;

namespace AlbumCreator.Model.Dtos
{
    class PictureDto : IPicture
    {
        public PictureDto(){}
        public PictureDto(IPicture picture)
        {
            Reset(picture);
        }

        public string Source { get; set; }
        public string Name { get; set; }
        public PictureType Type { get; set; }

        public void Reset(IPicture picture)
        {
            Source = picture.Source;
            Name = picture.Name;
            Type = picture.Type;
        }
    }
}
