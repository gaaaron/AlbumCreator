using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using AlbumCreator.Common.Enums;
using AlbumCreator.Common.Interfaces;
using AlbumCreator.Common.Utils;
using AlbumCreator.Models;
using AlbumCreator.WpfElements;
using AlbumCreator.WpfElements.CustomTreeView;

namespace AlbumCreator.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IUiService _uiService;
        private readonly IAlbumManager _albumManager;

        public ObservableCollection<PrevImage> ImageList { get; set; }

        public EditableAlbum Album { get; set; }

        private PrevImage _selectedImage; 
        public PrevImage SelectedImage
        {
            get { return _selectedImage; }
            set
            {
                _selectedImage = value;
                RefreshCommands();
                OnPropertyChanged(nameof(SelectedImage));
            }
        }

        #region Commands

        public RelayCommand OpenCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand NextPageCommand { get; set; }
        public RelayCommand PrevPageCommand { get; set; }

        public RelayCommand CreateAlbumCommand { get; set; }
        public RelayCommand AddPageCommand { get; set; }
        public RelayCommand RemovePageCommand { get; set; }
        public RelayCommand MoveDownPageCommand { get; set; }
        public RelayCommand MoveUpPageCommand { get; set; }
        public RelayCommand AddPictureCommand { get; set; }
        public RelayCommand RemovePictureCommand { get; set; }
        public RelayCommand MoveDownPictureCommand { get; set; }
        public RelayCommand MoveUpPictureCommand { get; set; }

        public RelayCommand OpenFolderCommand { get; set; }

        public RelayCommand ChangeLayout1 { get; set; }
        public RelayCommand ChangeLayout2 { get; set; }
        public RelayCommand ChangeLayout3 { get; set; }
        public RelayCommand ChangeLayout4 { get; set; }
        public RelayCommand ChangeLayout5 { get; set; }
        public RelayCommand ChangeLayout6 { get; set; }
        public RelayCommand ChangeLayout7 { get; set; }
        public RelayCommand PasteCommand { get; set; }
        public RelayCommand CropCommand { get; set; }
        public RelayCommand TreeViewSelectedItemChangedCommand { get; set; }

        #endregion

        public MainViewModel(IUiService uiService, IAlbumManager albumManager)
        {
            _uiService = uiService;
            _albumManager = albumManager;

            Album = new EditableAlbum(uiService);
            ImageList = new ObservableCollection<PrevImage>();

            CreateCommands();

            Album.PropertyChanged += Album_PropertyChanged;
        }

        private void CreateCommands()
        {
            CreateAlbumCommand = new RelayCommand(CreateNewAlbum, () => true);

            AddPageCommand = RegisterCommand(AddPage, () => true);
            RemovePageCommand = RegisterCommand(RemovePage, () => Album.SelectedPage != null);
            MoveDownPageCommand = RegisterCommand(Album.MoveDownPage, CheckMovePage);
            MoveUpPageCommand = RegisterCommand(Album.MoveUpPage, CheckMovePage);

            AddPictureCommand = RegisterCommand(AddPicture, CheckAddPicture);
            RemovePictureCommand = RegisterCommand(RemovePicture, () => Album.SelectedPicture != null);
            MoveDownPictureCommand = RegisterCommand(MoveDownPicture, CheckMovePicture);
            MoveUpPictureCommand = RegisterCommand(MoveUpPicture, CheckMovePicture);

            OpenFolderCommand = RegisterCommand(OpenImageFolder, () => true);

            ChangeLayout1 = RegisterCommand(() => Album.SelectedPage.Layout = AlbumLayoutType.Layout1, () => Album.SelectedPage != null);
            ChangeLayout2 = RegisterCommand(() => Album.SelectedPage.Layout = AlbumLayoutType.Layout2, () => Album.SelectedPage != null);
            ChangeLayout3 = RegisterCommand(() => Album.SelectedPage.Layout = AlbumLayoutType.Layout3, () => Album.SelectedPage != null);
            ChangeLayout4 = RegisterCommand(() => Album.SelectedPage.Layout = AlbumLayoutType.Layout4, () => Album.SelectedPage != null);
            ChangeLayout5 = RegisterCommand(() => Album.SelectedPage.Layout = AlbumLayoutType.Layout5, () => Album.SelectedPage != null);
            ChangeLayout6 = RegisterCommand(() => Album.SelectedPage.Layout = AlbumLayoutType.Layout6, () => Album.SelectedPage != null);
            ChangeLayout7 = RegisterCommand(() => Album.SelectedPage.Layout = AlbumLayoutType.Layout7, () => Album.SelectedPage != null);

            SaveCommand = RegisterCommand(() => _albumManager.SaveAlbum(Album), () => Album.Pages.Count > 0);
            OpenCommand = RegisterCommand(OpenAlbum, () => true);
            PasteCommand = RegisterCommand(PasteAction, () => Album.SelectedPage != null);
            CropCommand = RegisterCommand(CropAction, () => Album.SelectedPicture != null);

            NextPageCommand = RegisterCommand(Album.NextPage, () => Album.SelectedPage != null);
            PrevPageCommand = RegisterCommand(Album.PrevPage, () => Album.SelectedPage != null);
            TreeViewSelectedItemChangedCommand = RegisterCommand(Album.NextPage, () => true);
        }

        private void PasteAction()
        {
            if (Clipboard.ContainsFileDropList())
            {
                var fileList = Clipboard.GetFileDropList();

                foreach (var file in fileList)
                {
                    var name = Path.GetFileName(file);
                    Album.SelectedPage.Pictures.Add( new EditablePicture { Source = file, Name = name });
                }
            }
        }

        private void CropAction()
        {
            Album.SelectedPage.Pictures.Remove(Album.SelectedPicture);
            Clipboard.SetFileDropList(new StringCollection{ Album.SelectedPicture.Source});

            Album.SelectedPicture = null;
        }

        private async void OpenImageFolder()
        {
            var source = _uiService.OpenFolderDialog();
            if (string.IsNullOrEmpty(source))
                return;

            var prevImageList = new List<PrevImage>();
            await Task.Factory.StartNew(() => {
                var filters = new[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp" };
                var files = FileUtils.GetFilesFrom(source, filters, true);
                foreach (var image in files.Select(x => new Models.PrevImage(x)))
                {
                    prevImageList.Add(image);
                }
            });

            if (!prevImageList.Any())
            {
                _uiService.ShowError(Constants.MissingImage);
                return;
            }
            else
            {
                ImageList.Clear();
                foreach (var image in prevImageList)
                {
                    ImageList.Add(image);
                }
                RefreshCommands();
            }
        }

        #region Album

        private void CreateNewAlbum()
        {
            Album.Reset(true);
            AddPageCommand.RaiseCanExecuteChanged();
        }

        private void Album_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Album.SelectedPage):
                case nameof(Album.SelectedPicture):
                    RefreshCommands();
                    break;
            }
        }

        private void OpenAlbum()
        {
            var newAlbum = _albumManager.OpenAlbum();
            if (newAlbum != null)
            {
                Album.Reset(newAlbum);
            }
        }

        #endregion

        #region Page

        public void PageSelected(PageItem pageItem)
        {
            Album.SelectedPage = pageItem.Model;
            if (!Album.SelectedPage.Pictures.Contains(Album.SelectedPicture))
            {
                Album.SelectedPicture = null;
            }

            _uiService.ExpandAlbumItems();

            pageItem.IsExpanded = true;

            RefreshCommands();
        }

        private void AddPage()
        {
            var pageModel = new EditablePage{Layout = AlbumLayoutType.Layout1};
            if (Album.SelectedPage != null)
            {
                var index = Album.Pages.IndexOf(Album.SelectedPage);
                Album.Pages.Insert(index + 1, pageModel);
            }
            else
            {
                Album.Pages.Add(pageModel);
            }
        }

        private void RemovePage()
        {
            Album.RemoveSelected();
            RefreshCommands();
        }

        private bool CheckMovePage()
        {
            if (Album.SelectedPage != null)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Picture

        public void PictureSelected(PictureItem pictureItem)
        {
            Album.SelectedPage = ((pictureItem as TreeViewItem).Parent as PageItem).Model;
            Album.SelectedPicture = (pictureItem as PictureItem).Model;

            RefreshCommands();
        }

        private bool CheckAddPicture()
        {
            if (Album.SelectedPage != null && SelectedImage != null)
            {
                return true;
            }
            return false;
        }

        private void AddPicture()
        {
            var name = Path.GetFileName(SelectedImage.Source);
            if (Album.SelectedPicture != null)
            {
                var index = Album.SelectedPage.Pictures.IndexOf(Album.SelectedPicture);
                Album.SelectedPage.Pictures.Insert(index + 1, new EditablePicture { Source = SelectedImage.Source, Name = name});
            }
            else
            {
                Album.SelectedPage.Pictures.Add(new EditablePicture{ Source = SelectedImage.Source, Name = name });
            }
        }

        private void RemovePicture()
        {
            Album.SelectedPage.Pictures.Remove(Album.SelectedPicture);
            Album.SelectedPicture = null;

            RefreshCommands();
        }

        private bool CheckMovePicture()
        {
            if (Album.SelectedPicture == null)
            {
                return false;
            }
            return true;
        }

        public void MoveDownPicture()
        {
            var selected = Album.SelectedPicture;
            var index = Album.SelectedPage.Pictures.IndexOf(selected);
            if (index == Album.SelectedPage.Pictures.Count - 1) return;

            Album.SelectedPage.Pictures.Remove(selected);
            Album.SelectedPage.Pictures.Insert(index + 1, selected);
        }

        public void MoveUpPicture()
        {
            var selected = Album.SelectedPicture;
            var index = Album.SelectedPage.Pictures.IndexOf(selected);
            if (index == 0) return;

            Album.SelectedPage.Pictures.Remove(selected);
            Album.SelectedPage.Pictures.Insert(index - 1, selected);
        }

        #endregion

        internal void TreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is PageItem)
            {
                PageSelected(e.NewValue as PageItem);
            }
            else if (e.NewValue is PictureItem)
            {
                PictureSelected(e.NewValue as PictureItem);
                Album.SelectedPage = ((e.NewValue as TreeViewItem).Parent as PageItem).Model;
                Album.SelectedPicture = (e.NewValue as PictureItem).Model;

                RefreshCommands();
            }
        }
    }
}
