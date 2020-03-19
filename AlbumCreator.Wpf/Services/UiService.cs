using System.Windows;
using System.Windows.Controls;

using AlbumCreator.Common.Interfaces;

namespace AlbumCreator.Services
{
    public class UiService : IUiService
    {
        private MainWindow _window;
        internal void Attach(MainWindow window)
        {
            _window = window;
        }

        public bool ShowConfirmWindow(string message, string title)
        {
            var messageBoxResult = MessageBox.Show(message, title, MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                return true;
            }
            return false;
        }

        public void ShowError(string error)
        {
            MessageBox.Show(error, Constants.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public string OpenFolderDialog()
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.ShowNewFolderButton = false;
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK) return dialog.SelectedPath;
            }
            return null;
        }

        public string SaveFileDialog()
        {
            using (var dialog = new System.Windows.Forms.SaveFileDialog())
            {
                dialog.DefaultExt = "album";
                dialog.Filter = Constants.FileDialogFilter;
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK) return dialog.FileName;
            }
            return null;
        }

        public string OpenFileDialog()
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.DefaultExt = "album";
                dialog.Filter = Constants.FileDialogFilter;
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK) return dialog.FileName;
            }
            return null;
        }

        public void ExpandAlbumItems()
        {
            foreach (var item in _window.AlbumItem.Items)
            {
                (item as TreeViewItem).IsExpanded = false;
            }
        }
    }
}
