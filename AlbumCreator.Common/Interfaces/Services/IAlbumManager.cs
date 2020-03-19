namespace AlbumCreator.Common.Interfaces
{
    public interface IAlbumManager
    {
        void SaveAlbum(IAlbum album);
        IAlbum OpenAlbum();
    }
}
