using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sciendo.Common.IO;
using Sciendo.Mixx.DataAccess;
using Sciendo.Mixx.DataAccess.Domain;
using Sciendo.Playlists;
using TagLib;

namespace Sciendo.Playlist.Mixx.Processor
{
    public class MixxxPullProcessor:IMixxxProcessor
    {
        private readonly IDataHandler _dataHandler;
        private readonly IFile _file;

        public MixxxPullProcessor(IDataHandler dataHandler, IFile file)
        {
            _dataHandler = dataHandler;
            _file = file;
        }
        public void Start(string playlistFileName, IMap<IEnumerable<PlaylistItem>, IEnumerable<MixxxPlaylistTrack>> mapper)
        {
            if(string.IsNullOrEmpty(playlistFileName))
                throw new ArgumentNullException(nameof(playlistFileName));
            var playlistItems = _dataHandler.Get(Path.GetFileNameWithoutExtension(playlistFileName),mapper);
            var playlistHandler = PlaylistHandlerFactory.GetHandler(Path.GetExtension(playlistFileName));
            var playlistContents = playlistHandler.SetPlaylistItems(_file, playlistItems.ToArray());
            _file.WriteAllText(playlistFileName, playlistContents.Replace("\0", ""));
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<MixxxProcessorProgressEventHandler> MixxxPlaylistDeleted;
        public event EventHandler<MixxxProcessorProgressEventHandler> MixxxPlaylistCreated;
    }
}
