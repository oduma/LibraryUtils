using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Playlist.Mixx.Processor
{
    public interface IMixxxProcessor
    {
        void Start(string playlistFileName);

        void Stop();

        event EventHandler<MixxxProcessorProgressEventHandler> MixxxPlaylistDeleted;

        event EventHandler<MixxxProcessorProgressEventHandler> MixxxPlaylistCreated;
    }
}
