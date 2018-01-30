using System.Linq;
using Sciendo.Common.IO;
using Sciendo.Common.Serialization;
using TagLib;

namespace Sciendo.Playlists.XSPF
{
    public class XSPFHandler:IPlaylistHandler
    {

        public PlaylistItem[] GetPlaylistItems(string playlistContents)
        {
            
            var playlist =Serializer.Deserialize<Playlist>(playlistContents);

            return
                playlist?.Tracklist.Select(t => new PlaylistItem {FileName = t.Location.Replace("file:///", "").Replace("/","\\")})
                    .ToArray();
        }

        public string SetPlaylistItems(IFileReader<TagLib.File> tagFileReader, PlaylistItem[] playlistItems, string rootFolderPath)
        {
            var playlist = new Playlist {Version = 1, Tracklist = new Track[playlistItems.Length]};
            for (int i=0; i<playlistItems.Length;i++)
            {
                playlist.Tracklist[i] = new Track(tagFileReader, playlistItems[i].FileName, rootFolderPath);
            }

            return Serializer.Serialize(playlist);
        }
    }
}
