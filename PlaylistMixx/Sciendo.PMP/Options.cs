using CommandLine;
using Sciendo.Playlist.Mixx.Processor;

namespace Sciendo.PMP
{
    public class Options
    {
        [Value(0,Default="",HelpText = "Path to the playlist file.")]
        public string PlaylistFileName { get; set; }

        [Value(1, Default = Playlist.Mixx.Processor.ProcessingType.PullFromMixxx,HelpText="PullFromMixxx or PushToMixxx")]
        public ProcessingType ProcessingType { get; set; }
    }
}
