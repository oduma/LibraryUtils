using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sciendo.Common.IO;
using TagLib;

namespace Sciendo.Playlists.M3U
{
    public class M3UHandler: IPlaylistHandler
    {
        private const string PlaylistMarker = "#EXTM3U";
        private const string TrackMarker = "#EXTINF";
        private const string TrackTagMarker = ":";


        public PlaylistItem[] GetPlaylistItems(string playlistContents)
        {
            var playlist = DeserializeM3UPlaylist(playlistContents);

            return
                playlist.Select(t => new PlaylistItem { FileName = t.Location.Replace("file:///", "").Replace("/", "\\") })
                    .ToArray();

        }

        public string SetPlaylistItems(IFileReader<Tag> tagFileReader, PlaylistItem[] playlistItems, string rootFolderPath)
        {
            var playlist =  new Track[playlistItems.Length];
            for (int i = 0; i < playlistItems.Length; i++)
            {
                playlist[i] = new Track(tagFileReader, playlistItems[i].FileName, rootFolderPath);
            }

            return SerializeM3UPlaylist(playlist);

        }

        private IEnumerable<Track> DeserializeM3UPlaylist(string playlistContents)
        {
            string[] playlistContentsLines = playlistContents.Split(new[] {Environment.NewLine},
                StringSplitOptions.RemoveEmptyEntries);
            Track newTrack = null;
            foreach (string playlistContentsLine in playlistContentsLines)
            {
                if(playlistContentsLine.StartsWith(PlaylistMarker))
                    continue;
                if (playlistContentsLine.StartsWith($"{TrackMarker}{TrackTagMarker}"))
                {
                    newTrack= new Track(playlistContentsLine.Replace($"{TrackMarker}{TrackTagMarker}",""));
                    continue;
                }
                if (playlistContentsLine.StartsWith(TrackMarker))
                {
                    newTrack=new Track();
                    continue;
                }
                if (newTrack != null)
                {
                    newTrack.Location = playlistContentsLine;
                    yield return newTrack;
                }
            }
        }

        private string SerializeTrack(Track track)
        {
            string trackTagMarker = (track.TrackHasTag()) ? TrackTagMarker : string.Empty;
            return $"{TrackMarker}{trackTagMarker}{track}{Environment.NewLine}";
        }

        private string SerializeM3UPlaylist(IEnumerable<Track> playlist)
        {
            var playlistContent = new StringBuilder($"{PlaylistMarker}{Environment.NewLine}");
            foreach (var track in playlist)
            {
                playlistContent.Append(SerializeTrack(track));
            }

            return playlistContent.ToString();
        }
    }
}
