using System.Linq;
using Sciendo.Common.IO;
using Sciendo.Common.Serialization;
using Sciendo.Playlists;
using Sciendo.Playlists.XSPF;
using TagLib;

namespace Sciendo.Playlist.Handler.XSPF
{
    public class XSPFHandler:IPlaylistHandler
    {

        public PlaylistItem[] GetPlaylistItems(string playlistContents)
        {
            
            var playlist =Serializer.Deserialize<Playlists.XSPF.Playlist>(playlistContents);

            return
                playlist?.Tracklist.Select(t => new PlaylistItem {FileName = t.Location.Replace("file:///", "").Replace("/","\\")})
                    .ToArray();
        }

        public string SetPlaylistItems(IFileReader<Tag> tagFileReader, PlaylistItem[] playlistItems, string rootFolderPath)
        {
            var playlist = new Playlists.XSPF.Playlist {Version = 1, Tracklist = new Track[playlistItems.Length]};
            for (int i=0; i<playlistItems.Length;i++)
            {
                playlist.Tracklist[i] = new Track(tagFileReader, playlistItems[i].FileName, rootFolderPath);
            }

            return Serializer.Serialize(playlist);
        }
    }
}
