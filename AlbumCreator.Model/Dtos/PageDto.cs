using System.Collections.Generic;
using System.Linq;

using AlbumCreator.Common.Enums;
using AlbumCreator.Common.Interfaces;

namespace AlbumCreator.Model.Dtos
{
    class PageDto : IPage
    {
        public PageDto()
        {
            Pictures = new List<PictureDto>();
        }

        public PageDto(IPage page) : this()
        {
            Reset(page);
        }

        public List<PictureDto> Pictures { get; }
        IEnumerable<IPicture> IPage.Pictures => Pictures.Cast<IPicture>();

        public AlbumLayoutType Layout { get; set; }

        public void Reset(IPage page)
        {
            Pictures.Clear();
            Pictures.AddRange(page.Pictures.Select(p => new PictureDto(p)));
            Layout = page.Layout;
        }
    }
}
