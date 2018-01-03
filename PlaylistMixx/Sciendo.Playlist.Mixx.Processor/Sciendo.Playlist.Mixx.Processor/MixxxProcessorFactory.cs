﻿using Sciendo.Common.IO;
using Sciendo.Mixx.DataAccess;
using Sciendo.Playlists;
using TagLib;

namespace Sciendo.Playlist.Mixx.Processor
{
    public class MixxxProcessorFactory
    {
        public IMixxxProcessor GetProcessor(ProcessingType processingType, 
            IDataHandler dataHandler, 
            IPlaylistHandlerFactory playlistHandlerFactory, 
            IFileReader<string> textFileReader, 
            IFileReader<Tag> tagFileReader, 
            IFileWriter textFileWriter)
        {
            switch (processingType)
            {
                case ProcessingType.PushToMixxx:
                    return new MixxxPushProcessor(dataHandler,playlistHandlerFactory,textFileReader);
                case ProcessingType.PullFromMixxx:
                    return new MixxxPullProcessor(dataHandler,playlistHandlerFactory,tagFileReader,textFileWriter);
                default:
                    return null;
            }
        }
    }
}