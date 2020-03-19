using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using AlbumCreator.Common.Interfaces;

namespace AlbumCreator.Models
{
    public class EditableAlbum : ModelBase, IAlbum
    {
        private IUiService _uiService;

        private EditablePage _selectedPage;
        public EditablePage SelectedPage
        {
            get { return _selectedPage; }
            set { _selectedPage = value; OnPropertyChanged(nameof(SelectedPage)); }
        }

        private EditablePicture _selectedPicture;
        public EditablePicture SelectedPicture
        {
            get { return _selectedPicture; }
            set { _selectedPicture = value; OnPropertyChanged(nameof(SelectedPicture)); }
        }

        public ObservableCollection<EditablePage> Pages { get; set; }
        IEnumerable<IPage> IAlbum.Pages => Pages.Cast<IPage>();

        public EditableAlbum(IUiService uiService)
        {
            _uiService = uiService;
            Pages = new ObservableCollection<EditablePage>();
        }

        public void Reset(bool withValidation)
        {
            if (withValidation)
            {
                if (Pages.Count > 0)
                {
                    var result = _uiService.ShowConfirmWindow(Constants.ConfirmAlbumReset, "");
                    if (result == false)
                    {
                        return;
                    }
                }
            }

            foreach (var page in Pages)
            {
                page.Pictures.Clear();
            }
            Pages.Clear();
        }

        public void RemoveSelected()
        {
            Pages.Remove(SelectedPage);
            SelectedPicture = null;
            SelectedPage = Pages.FirstOrDefault();
        }

        public void MoveDownPage()
        {
            var index = Pages.IndexOf(SelectedPage);
            if (index == Pages.Count - 1) return;

            Pages.Remove(SelectedPage);
            Pages.Insert(index + 1, SelectedPage);
        }

        public void MoveUpPage()
        {
            var index = Pages.IndexOf(SelectedPage);
            if (index == 0) return;

            Pages.Remove(SelectedPage);
            Pages.Insert(index - 1, SelectedPage);
        }

        public void NextPage()
        {
            var index = Pages.IndexOf(SelectedPage);

            if (index < Pages.Count - 1)
            {
                SelectedPage = Pages.ElementAt(index + 1);
                SelectedPicture = null;
            }
        }

        public void PrevPage()
        {
            var index = Pages.IndexOf(SelectedPage);

            if (index > 0)
            {
                SelectedPage = Pages.ElementAt(index - 1);
                SelectedPicture = null;
            }
        }

        public void Reset(IAlbum album)
        {
            Pages.Clear();
            foreach (var page in album.Pages)
            {
                var editablePage = new EditablePage();
                editablePage.Reset(page);
                Pages.Add(editablePage);
            }
        }
    }
}
