using System.Linq;
using Sciendo.Common.Serialization;
using Sciendo.Playlist.Handler.Contracts;

namespace Sciendo.Playlist.Handler.XSPF
{
    public class XSPFHandler:IPlaylistHandler
    {
        
        public PlaylistItem[] GetPlaylistItems(string playlistContents)
        {
            
            var playlist =Serializer.Deserialize<Playlist>(playlistContents);

            return
                playlist?.Tracklist.Select(t => new PlaylistItem {FileName = t.Location,
                        TransformedFileName = t.Location.Replace("file:///", "").Replace("/","\\")})
                    .ToArray();
        }

        public string SetPlaylistItems(string playlistContents, PlaylistItem[] playlistItems)
        {
            var playlist = Serializer.Deserialize<Playlist>(playlistContents);
            if (playlist != null)
            {
                foreach (var track in playlist.Tracklist)
                {
                    track.Location = playlistItems.FirstOrDefault(i => i.FileName == track.Location).TransformedFileName;
                }
            }

            return Serializer.Serialize(playlist);
        }
    }
}
