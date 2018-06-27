using System.IO;
using System.Linq;
using Sciendo.Common.IO;
using Sciendo.Common.Music.Tagging;
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

        public string SetPlaylistItems(IFile file, PlaylistItem[] playlistItems, string rootFolderPath)
        {
            var playlist = new Playlist {Version = 1, Tracklist = new Track[playlistItems.Length]};
            for (int i=0; i<playlistItems.Length;i++)
            {
                var filePath = (string.IsNullOrEmpty(rootFolderPath))
                    ? playlistItems[i].FileName
                    : $"{rootFolderPath}{Path.DirectorySeparatorChar}{playlistItems[i].FileName}";

                var tagFile = file.ReadTag(filePath);
                if(tagFile!=null)
                    playlist.Tracklist[i] = new Track(tagFile,filePath);
            }

            return Serializer.Serialize(playlist);
        }
    }
}
