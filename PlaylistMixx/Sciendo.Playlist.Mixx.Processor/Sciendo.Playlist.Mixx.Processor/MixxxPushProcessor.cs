using System;
using System.IO;
using Sciendo.Common.IO;
using Sciendo.Mixx.DataAccess;
using Sciendo.Playlists;

namespace Sciendo.Playlist.Mixx.Processor
{
    public class MixxxPushProcessor:IMixxxProcessor
    {
        private readonly IDataHandler _dataHandler;
        private readonly IPlaylistHandlerFactory _playlistHandlerFactory;
        private readonly IFileReader<string> _fileReader;

        public MixxxPushProcessor(IDataHandler dataHandler, IPlaylistHandlerFactory playlistHandlerFactory,IFileReader<string> fileReader)
        {
            _dataHandler = dataHandler;
            _playlistHandlerFactory = playlistHandlerFactory;
            _fileReader = fileReader;
        }
        public void Start(string playlistFileName)
        {
            if(string.IsNullOrEmpty(playlistFileName))
                throw new ArgumentNullException(nameof(playlistFileName));
            var playlistHandler = _playlistHandlerFactory.GetHandler(Path.GetExtension(playlistFileName));
            var playlistItems = playlistHandler.GetPlaylistItems(_fileReader.Read(playlistFileName));
            if (_dataHandler.Delete(Path.GetFileNameWithoutExtension(playlistFileName)))
            {
                MixxxPlaylistDeleted?.Invoke(this,
                    new MixxxProcessorProgressEventHandler(Path.GetFileNameWithoutExtension(playlistFileName)));
            }
            if (_dataHandler.Create(Path.GetFileNameWithoutExtension(playlistFileName), playlistItems))
            {
                MixxxPlaylistCreated?.Invoke(this,
                    new MixxxProcessorProgressEventHandler(Path.GetFileNameWithoutExtension(playlistFileName)));
            }
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<MixxxProcessorProgressEventHandler> MixxxPlaylistDeleted;
        public event EventHandler<MixxxProcessorProgressEventHandler> MixxxPlaylistCreated;
    }
}
