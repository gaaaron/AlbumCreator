using AlbumCreator.Common.Enums;

namespace AlbumCreator.Common.Interfaces
{
    public interface IPicture
    {
        string Source { get; set; }
        string Name { get; set; }
        PictureType Type { get; set; }
        void Reset(IPicture picture);
    }
}
