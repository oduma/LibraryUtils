using Sciendo.Playlists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Playlist.Appender
{
    public interface IPlaylistTransformer
    {
        IPlaylistTransformer ChangeTheRootPath(PlaylistItem currentPlaylistItem, string fromRootPath, string toRootPath);
    }
}
