using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sciendo.Common.IO;
using Sciendo.Mixx.DataAccess;
using Sciendo.Playlists;
using TagLib;

namespace Sciendo.Playlist.Mixx.Processor
{
    public class MixxxPullProcessor:IMixxxProcessor
    {
        private readonly IDataHandler _dataHandler;
        private readonly IPlaylistHandlerFactory _playlistHandlerFactory;
        private readonly IFileReader<Tag> _tagFileReader;
        private readonly IFileWriter _textFileWriter;

        public MixxxPullProcessor(IDataHandler dataHandler, IPlaylistHandlerFactory playlistHandlerFactory,IFileReader<Tag> tagFileReader, IFileWriter textFileWriter)
        {
            _dataHandler = dataHandler;
            _playlistHandlerFactory = playlistHandlerFactory;
            _tagFileReader = tagFileReader;
            _textFileWriter = textFileWriter;
        }
        public void Start(string playlistFileName)
        {
            if(string.IsNullOrEmpty(playlistFileName))
                throw new ArgumentNullException(nameof(playlistFileName));
            var playlistItems = _dataHandler.Get(Path.GetFileNameWithoutExtension(playlistFileName));
            var playlistHandler = _playlistHandlerFactory.GetHandler(Path.GetExtension(playlistFileName));
            var playlistContents = playlistHandler.SetPlaylistItems(_tagFileReader, playlistItems.ToArray());
            _textFileWriter.Write(playlistContents,playlistFileName);
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<MixxxProcessorProgressEventHandler> MixxxPlaylistDeleted;
        public event EventHandler<MixxxProcessorProgressEventHandler> MixxxPlaylistCreated;
    }
}
