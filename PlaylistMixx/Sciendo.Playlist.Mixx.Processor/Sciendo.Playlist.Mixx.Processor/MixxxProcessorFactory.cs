using Sciendo.Common.IO;
using Sciendo.Mixx.DataAccess;
using Sciendo.Playlists;
using TagLib;

namespace Sciendo.Playlist.Mixx.Processor
{
    public class MixxxProcessorFactory
    {
        public IMixxxProcessor GetProcessor(ProcessingType processingType, 
            IDataHandler dataHandler, 
            IFileReader<string> textFileReader, 
            IFileReader<TagLib.File> tagFileReader, 
            IFileWriter textFileWriter)
        {
            switch (processingType)
            {
                case ProcessingType.PushToMixxx:
                    return new MixxxPushProcessor(dataHandler,textFileReader);
                case ProcessingType.PullFromMixxx:
                    return new MixxxPullProcessor(dataHandler,tagFileReader,textFileWriter);
                default:
                    return null;
            }
        }
    }
}
