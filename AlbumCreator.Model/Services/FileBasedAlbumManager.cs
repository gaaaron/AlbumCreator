using Newtonsoft.Json;
using System.IO;

using AlbumCreator.Common.Interfaces;
using AlbumCreator.Model.Dtos;

namespace AlbumCreator.Model.Services
{
    public class FileBasedAlbumManager : IAlbumManager
    {
        private IUiService _uiService;

        public FileBasedAlbumManager(IUiService uiService)
        {
            _uiService = uiService;
        }

        public void SaveAlbum(IAlbum album)
        {
            var albumDto = new AlbumDto();
            albumDto.Reset(album);

            var serializedAlbum = JsonConvert.SerializeObject(albumDto);
            var fileName = _uiService.SaveFileDialog();
            if (!string.IsNullOrEmpty(fileName))
                File.WriteAllText(fileName, serializedAlbum);
        }

        public IAlbum OpenAlbum()
        {
            var fileName = _uiService.OpenFileDialog();
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            var serializedAlbum = File.ReadAllText(fileName);
            var album = JsonConvert.DeserializeObject<AlbumDto>(serializedAlbum);

            return album;
        }
    }
}
