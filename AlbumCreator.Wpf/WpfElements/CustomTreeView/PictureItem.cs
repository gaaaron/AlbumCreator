using System.Windows.Controls;

using AlbumCreator.Models;

namespace AlbumCreator.WpfElements.CustomTreeView
{
    public class PictureItem : TreeViewItem
    {
        public EditablePicture Model { get; set; }
    }
}
