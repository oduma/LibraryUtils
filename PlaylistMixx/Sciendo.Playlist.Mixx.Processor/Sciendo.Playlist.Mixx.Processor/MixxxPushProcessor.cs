using System;
using System.Collections.Generic;
using System.IO;
using Sciendo.Common.IO;
using Sciendo.Common.Music.Contracts;
using Sciendo.Mixx.DataAccess;
using Sciendo.Mixx.DataAccess.Domain;
using Sciendo.Playlists;

namespace Sciendo.Playlist.Mixx.Processor
{
    public class MixxxPushProcessor:IMixxxProcessor,IPostProcessor
    {
        private readonly IDataHandler _dataHandler;
        private readonly IFile _file;

        public MixxxPushProcessor(IDataHandler dataHandler,IFile file)
        {
            _dataHandler = dataHandler;
            _file = file;
        }
        public void Start(string playlistFileName, IMap<IEnumerable<PlaylistItem>,IEnumerable<MixxxPlaylistTrack>> mapper)
        {
            if(string.IsNullOrEmpty(playlistFileName))
                throw new ArgumentNullException(nameof(playlistFileName));
            PushPlaylistContentsToMixxx(_file.ReadAllText(playlistFileName), playlistFileName, new MapTracks());
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<MixxxProcessorProgressEventHandler> MixxxPlaylistDeleted;
        public event EventHandler<MixxxProcessorProgressEventHandler> MixxxPlaylistCreated;
        public bool Process(object messageBody, string messageName)
        {
            var mapper = new MapTracks();
            var playlistContents = string.Empty;
            try
            {
                playlistContents = (string)messageBody;
                return !string.IsNullOrEmpty(playlistContents) 
                    ? PushPlaylistContentsToMixxx(playlistContents, messageName, mapper) 
                    : PushPlaylistContentsToMixxx(playlistContents,messageName, mapper,true);
            }
            catch
            {
                return false;
            }

        }

        private bool PushPlaylistContentsToMixxx(string playlistContents, string playlistName, IMap<IEnumerable<PlaylistItem>, IEnumerable<MixxxPlaylistTrack>> mapper, bool deleteOnly = false)
        {
            var playlistHandler = PlaylistHandlerFactory.GetHandler(Path.GetExtension(playlistName));
            var playlistItems = playlistHandler.GetPlaylistItems(playlistContents);
            if (_dataHandler.Delete(Path.GetFileNameWithoutExtension(playlistName)))
            {
                MixxxPlaylistDeleted?.Invoke(this,
                    new MixxxProcessorProgressEventHandler(Path.GetFileNameWithoutExtension(playlistName)));
            }
            if (!deleteOnly)
            {
                if (_dataHandler.Create(Path.GetFileNameWithoutExtension(playlistName), playlistItems,mapper))
                {
                    MixxxPlaylistCreated?.Invoke(this,
                        new MixxxProcessorProgressEventHandler(Path.GetFileNameWithoutExtension(playlistName)));
                }
            }
            return true;
        }
    }
}
