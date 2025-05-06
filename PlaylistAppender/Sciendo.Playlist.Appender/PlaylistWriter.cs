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
    public class PlaylistWriter : IPlaylistWriter
    {
        private readonly IStorage _storage;
        private readonly string _targetDirectory;

        public PlaylistWriter(IStorage storage, string targetDirectory )
        {
            _targetDirectory = targetDirectory;
            _storage = storage;
        }
        public void Write(string fileName, PlaylistType playlistType, IEnumerable<PlaylistItem> playlist)
        {
            var playlistHandler = PlaylistHandlerFactory.GetHandler(playlistType.ToString().ToLower());

            var newPlaylistRawContent = playlistHandler.SetPlaylistItems(_storage.File, playlist.ToArray(), "");
            _storage.File.WriteAllText(fileName, newPlaylistRawContent);
        }
    }
}
