using System.Collections.Generic;
using System.Linq;

using AlbumCreator.Common.Interfaces;

namespace AlbumCreator.Model.Dtos
{
    class AlbumDto : IAlbum
    {
        public AlbumDto()
        {
            Pages = new List<PageDto>();
        }

        public AlbumDto(IAlbum album) : this()
        {
            Reset(album);
        }

        public List<PageDto> Pages { get; set; }
        IEnumerable<IPage> IAlbum.Pages => Pages.Cast<IPage>();

        public void Reset(IAlbum album)
        {
            Pages.Clear();
            Pages.AddRange(album.Pages.Select(p => new PageDto(p)));
        }
    }
}
