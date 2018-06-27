using Sciendo.Common.IO;
using Sciendo.Mixx.DataAccess;
using Sciendo.Playlists;
using TagLib;

namespace Sciendo.Playlist.Mixx.Processor
{
    public class MixxxProcessorFactory
    {
        public IMixxxProcessor GetProcessor(ProcessingType processingType, 
            IDataHandler dataHandler, IFile file)
        {
            switch (processingType)
            {
                case ProcessingType.PushToMixxx:
                    return new MixxxPushProcessor(dataHandler, file);
                case ProcessingType.PullFromMixxx:
                    return new MixxxPullProcessor(dataHandler,file);
                default:
                    return null;
            }
        }
    }
}
