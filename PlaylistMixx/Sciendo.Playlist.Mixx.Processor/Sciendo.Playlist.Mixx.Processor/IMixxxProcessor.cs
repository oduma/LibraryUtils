using System;

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
