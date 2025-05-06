using Sciendo.Common.IO;
using Sciendo.Playlists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Playlist.Appender
{
    public interface IPlaylistReader
    {
        PlaylistItem[] Read(string playlistFileName, PlaylistType playlistType);

        IStorage Storage { get; }
    }
}
