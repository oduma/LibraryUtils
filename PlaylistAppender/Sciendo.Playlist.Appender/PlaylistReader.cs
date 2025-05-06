using Sciendo.Common.IO;
using Sciendo.Playlists;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Playlist.Appender
{
    public class PlaylistReader : IPlaylistReader
    {
        private readonly IStorage _storage;

        public PlaylistReader(string rootPath, IStorage storage)
        {
            _storage = storage;
            
        }

        public IStorage Storage => _storage;

        public PlaylistItem[] Read(string playlistFileName, PlaylistType playlistType)
        {
            
            return PlaylistHandlerFactory.GetHandler(Path.GetExtension(playlistFileName))
                .GetPlaylistItems(_storage.File.ReadAllText(playlistFileName));
        }
    }
}
