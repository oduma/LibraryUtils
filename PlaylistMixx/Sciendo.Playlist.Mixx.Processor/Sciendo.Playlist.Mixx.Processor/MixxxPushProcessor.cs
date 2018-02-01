using System;
using System.IO;
using Sciendo.Common.IO;
using Sciendo.Common.Music.Contracts;
using Sciendo.Mixx.DataAccess;
using Sciendo.Playlists;

namespace Sciendo.Playlist.Mixx.Processor
{
    public class MixxxPushProcessor:IMixxxProcessor,IPostProcessor
    {
        private readonly IDataHandler _dataHandler;
        private readonly IFileReader<string> _fileReader;

        public MixxxPushProcessor(IDataHandler dataHandler, 
            IFileReader<string> fileReader)
        {
            _dataHandler = dataHandler;
            _fileReader = fileReader;
        }
        public void Start(string playlistFileName)
        {
            if(string.IsNullOrEmpty(playlistFileName))
                throw new ArgumentNullException(nameof(playlistFileName));
            PushPlaylistContentsToMixxx(_fileReader.Read(playlistFileName), playlistFileName);
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<MixxxProcessorProgressEventHandler> MixxxPlaylistDeleted;
        public event EventHandler<MixxxProcessorProgressEventHandler> MixxxPlaylistCreated;
        public bool Process(object messageBody, string messageName)
        {
            var playlistContents = string.Empty;
            try
            {
                playlistContents = (string)messageBody;
                if (!string.IsNullOrEmpty(playlistContents))
                    return PushPlaylistContentsToMixxx(playlistContents, messageName);
                return false;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        private bool PushPlaylistContentsToMixxx(string playlistContents, string playlistName)
        {
            var playlistHandler = PlaylistHandlerFactory.GetHandler(Path.GetExtension(playlistName));
            var playlistItems = playlistHandler.GetPlaylistItems(playlistContents);
            if (_dataHandler.Delete(Path.GetFileNameWithoutExtension(playlistName)))
            {
                MixxxPlaylistDeleted?.Invoke(this,
                    new MixxxProcessorProgressEventHandler(Path.GetFileNameWithoutExtension(playlistName)));
            }
            if (_dataHandler.Create(Path.GetFileNameWithoutExtension(playlistName), playlistItems))
            {
                MixxxPlaylistCreated?.Invoke(this,
                    new MixxxProcessorProgressEventHandler(Path.GetFileNameWithoutExtension(playlistName)));
            }
            return true;
        }
    }
}
