using System.Collections.Generic;
using Sciendo.Common.IO;
using TagLib;

namespace Sciendo.Playlist.Handler.Contracts
{
    public interface IPlaylistHandler
    {
        PlaylistItem[] GetPlaylistItems(string playlistContents);

        string SetPlaylistItems(IFileReader<Tag> tagFileReader, PlaylistItem[] playlistItems, string rootFolderPath);
    }
}
