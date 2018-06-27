using Sciendo.Common.IO;
using TagLib;

namespace Sciendo.Playlists
{
    public interface IPlaylistHandler
    {
        PlaylistItem[] GetPlaylistItems(string playlistContents);

        string SetPlaylistItems(IFile file, PlaylistItem[] playlistItems, string rootFolderPath="");
    }
}
