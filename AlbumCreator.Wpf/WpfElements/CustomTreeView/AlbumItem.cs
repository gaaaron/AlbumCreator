using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

using AlbumCreator.Models;

namespace AlbumCreator.WpfElements.CustomTreeView
{
    public class AlbumItem : TreeView
    {

        // Dependency Property
        public static readonly DependencyProperty EditableAlbumProperty =
             DependencyProperty.Register("EditableAlbum", typeof(EditableAlbum), typeof(AlbumItem), new FrameworkPropertyMetadata(OnEditableAlbumPropertyChanged));

        // .NET Property wrapper
        public EditableAlbum EditableAlbum
        {
            get { return (EditableAlbum)GetValue(EditableAlbumProperty); }
            set { SetValue(EditableAlbumProperty, value); }
        }

        private static void OnEditableAlbumPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var control = source as AlbumItem;
            var model = (EditableAlbum)e.NewValue;
            // Put some update logic here...
            control.OnSetAlbumModelChanged(model);
        }

        private void OnSetAlbumModelChanged(EditableAlbum model)
        {
            model.Pages.CollectionChanged += PagesOnCollectionChanged;
        }

        private void PagesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var newItem in notifyCollectionChangedEventArgs.NewItems)
                {
                    InsertPage(notifyCollectionChangedEventArgs.NewStartingIndex, newItem as EditablePage);
                }

            }
            else if (notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var oldItem in notifyCollectionChangedEventArgs.OldItems)
                {
                    RemovePage(oldItem as EditablePage);
                }
            }
            else if (notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Reset)
            {
                Items.Clear();
            }
        }

        private void RemovePage(EditablePage page)
        {
            var pageItem = GetPageItem(page);
            Items.Remove(pageItem);

            RefreshNames();
        }

        private void InsertPage(int index, EditablePage page)
        {
            var pageItem = new PageItem { Model = page, IsSelected = true };
            Items.Insert(index, pageItem);

            if (page.Pictures.Count > 0)
            {
                foreach (var picture in page.Pictures)
                {
                    pageItem.AddPicture(picture);
                }
            }

            RefreshNames();
        }

        private PageItem GetPageItem(EditablePage model)
        {
            foreach (var item in Items)
            {
                var pageItem = item as PageItem;
                if (pageItem?.Model == model) return pageItem;
            }
            return null;
        }

        private void RefreshNames()
        {
            int index = 1;
            foreach (var item in Items)
            {
                (item as TreeViewItem).Header = index.ToString().PadLeft(4, ' ') + ".";
                index++;
            }
        }
    }
}
