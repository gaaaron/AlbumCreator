using System.Collections.Generic;

namespace AlbumCreator.Common.Interfaces
{
    public interface IAlbum
    {
        IEnumerable<IPage> Pages { get;}
        void Reset(IAlbum album);
    }
}
