using System;

namespace Sciendo.Playlist.Mixx.Processor
{
    public class MixxxProcessorProgressEventHandler:EventArgs
    {
        public string MixxxPlaylistName { get; private set; }

        public MixxxProcessorProgressEventHandler(string mixxxPlaylistName)
        {
            MixxxPlaylistName = mixxxPlaylistName;
        }
    }
}