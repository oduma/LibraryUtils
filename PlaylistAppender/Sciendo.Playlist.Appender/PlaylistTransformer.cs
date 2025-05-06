using Sciendo.Playlists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Playlist.Appender
{
    public class PlaylistTransformer : IPlaylistTransformer
    {
        public IPlaylistTransformer ChangeTheRootPath(PlaylistItem currentPlaylistItem, string fromRootPath, string toRootPath)
        {
            if(string.IsNullOrEmpty(fromRootPath))
            {
                if (toRootPath[toRootPath.Length - 1] == '\\')
                    currentPlaylistItem.FileName = $"{toRootPath}{currentPlaylistItem.FileName}";
                else
                    currentPlaylistItem.FileName = $"{toRootPath}\\{currentPlaylistItem.FileName}";
                return this;
            }
            currentPlaylistItem.FileName= currentPlaylistItem.FileName
                .ToLower().Replace(fromRootPath.ToLower(), toRootPath.ToLower());
            return this;
        }
    }
}
