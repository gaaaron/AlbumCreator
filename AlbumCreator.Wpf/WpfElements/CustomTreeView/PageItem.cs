using System.Collections.Specialized;
using System.Windows.Controls;

using AlbumCreator.Models;

namespace AlbumCreator.WpfElements.CustomTreeView
{
    public class PageItem : TreeViewItem
    {
        private EditablePage _model;
        public EditablePage Model
        {
            get { return _model; }
            set
            {
                _model = value;
                _model.Pictures.CollectionChanged += PicturesOnCollectionChanged;
            }
        }

        private void PicturesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var newItem in notifyCollectionChangedEventArgs.NewItems)
                {
                    InsertPicture(notifyCollectionChangedEventArgs.NewStartingIndex, newItem as EditablePicture);
                }

            }
            else if (notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var oldItem in notifyCollectionChangedEventArgs.OldItems)
                {
                    RemovePicture(oldItem as EditablePicture);
                }
            }
            else if (notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Reset)
            {
                Items.Clear();
            }
        }

        public void RemovePicture(EditablePicture picture)
        {
            var pictureItem = GetPictureItem(picture);
            Items.Remove(pictureItem);
        }

        public void InsertPicture(int index, EditablePicture picture)
        {
            var name = picture.Name.Length > 30 ? picture.Name.Substring(0, 30) + "..." : picture.Name;
            Items.Insert(index, new PictureItem { Model = picture, IsSelected = true, Header = name });
        }

        public void AddPicture(EditablePicture picture)
        {
            var name = picture.Name.Length > 30 ? picture.Name.Substring(0, 30) + "..." : picture.Name;
            Items.Add(new PictureItem { Model = picture, IsSelected = true, Header = name });
        }

        private PictureItem GetPictureItem(EditablePicture model)
        {
            foreach (var item in Items)
            {
                var pictureItem = item as PictureItem;
                if (pictureItem?.Model == model) return pictureItem;
            }
            return null;
        }
    }
}