using System.Collections.Generic;

using AlbumCreator.Common.Enums;

namespace AlbumCreator.Common.Interfaces
{
    public interface IPage
    {
        IEnumerable<IPicture> Pictures { get; }
        AlbumLayoutType Layout { get; }
        void Reset(IPage page);
    }
}
