using Sciendo.Playlists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Playlist.Appender
{
    public interface IPlaylistsCombiner
    {
        IEnumerable<PlaylistItem> Append(IEnumerable<PlaylistItem> toPlaylist, IEnumerable<PlaylistItem> fromPlaylist);
    }
}
