using Sciendo.Playlists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Playlist.Appender
{
    public interface IPlaylistWriter
    {
        void Write(string fileName, PlaylistType playlistType, IEnumerable<PlaylistItem> playlist);
    }
}
