using Sciendo.Common.IO;
using TagLib;

namespace Sciendo.Playlists
{
    public interface IPlaylistHandler
    {
        PlaylistItem[] GetPlaylistItems(string playlistContents);

        //PlaylistItem[] GetPlaylistItemsFromFile(string file);

        string SetPlaylistItems(IFileReader<TagLib.File> tagFileReader, PlaylistItem[] playlistItems, string rootFolderPath="");

        //void SetPlaylistItemsToFile(IFileReader<Tag> tagFileReader, PlaylistItem[] playlistItems, string rootFolderPath,
        //    string fileToSave);
    }
}
