using Sciendo.Playlists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Playlist.Appender
{
    public class PlaylistCombiner : IPlaylistsCombiner
    {
        public IEnumerable<PlaylistItem> Append(IEnumerable<PlaylistItem> toPlaylist, IEnumerable<PlaylistItem> fromPlaylist)
        {
            return toPlaylist.Union(fromPlaylist);
            //foreach(var playlistItem in toPlaylist )
            //{
            //    yield return playlistItem;
            //}
            //foreach(var playlistItem in fromPlaylist)
            //{
            //    yield return playlistItem;
            //}
        }
    }
}
