using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using AlbumCreator.Common.Enums;
using AlbumCreator.Common.Interfaces;

namespace AlbumCreator.Models
{
    public class EditablePage : ModelBase, IPage
    {
        public ObservableCollection<EditablePicture> Pictures { get; set; }
        IEnumerable<IPicture> IPage.Pictures => Pictures.Cast<IPicture>();

        private AlbumLayoutType _layout;
        public AlbumLayoutType Layout
        {
            get { return _layout; }
            set { _layout = value; LayoutUpdate(); OnPropertyChanged(nameof(Layout)); }
        }

        public EditablePage()
        {
            Pictures = new ObservableCollection<EditablePicture>();
        }

        private void LayoutUpdate()
        {
            for (int number = 0; number < Pictures.Count; number++)
            {
                var picture = Pictures.ElementAtOrDefault(number);
                if (picture == null) return;

                switch (Layout)
                {
                    case AlbumLayoutType.Layout1:
                        picture.Type = PictureType.SmallLandscape;
                        break;

                    case AlbumLayoutType.Layout2:
                        if (number < 2) picture.Type = PictureType.SmallLandscape;
                        else picture.Type = PictureType.Landscape;
                        break;

                    case AlbumLayoutType.Layout3:
                        if (number > 0) picture.Type = PictureType.SmallLandscape;
                        picture.Type = PictureType.Landscape;
                        break;

                    case AlbumLayoutType.Layout4:
                        if (number == 0 || number == 3) picture.Type = PictureType.SmallLandscape;
                        else picture.Type = PictureType.Portrait;
                        break;

                    case AlbumLayoutType.Layout5:
                        if (number == 0 || number == 3) picture.Type = PictureType.Portrait;
                        else picture.Type = PictureType.SmallLandscape;
                        break;

                    case AlbumLayoutType.Layout6:
                        if (number < 2) picture.Type = PictureType.SmallLandscape;
                        else picture.Type = PictureType.Portrait;
                        break;

                    case AlbumLayoutType.Layout7:
                        if (number < 2) picture.Type = PictureType.Portrait;
                        else picture.Type = PictureType.SmallLandscape;
                        break;

                    default:
                        picture.Type = PictureType.SmallLandscape;
                        break;
                }
            }
        }

        public void Reset(IPage page)
        {
            Pictures.Clear();
            foreach (var picture in page.Pictures)
            {
                var editablePicture = new EditablePicture();
                editablePicture.Reset(picture);
                Pictures.Add(editablePicture);
            }
            Layout = page.Layout;
        }
    }
}
