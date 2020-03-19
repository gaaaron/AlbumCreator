namespace AlbumCreator.Common.Interfaces
{
    public interface IUiService
    {
        bool ShowConfirmWindow(string message, string title);
        void ShowError(string error);
        string OpenFolderDialog();
        string SaveFileDialog();
        string OpenFileDialog();
        void ExpandAlbumItems();
    }
}
