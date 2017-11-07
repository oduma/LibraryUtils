using CommandLine;
using Sciendo.Playlist.Persister;

namespace Sciendo.PP
{
    internal class Options
    {
        [Option('s', "musicSource",Default="",HelpText = "Root folder of the files when the playlist was created")]
        public string MusicSourceRoot { get; set; }
        [Option('c', "musicCurrent", Default = "", HelpText = "Root folder of the files now")]
        public string MusicCurrentRoot { get; set; }

        [Option('t', "targetType", Default = PlaylistType.M3U, HelpText = "Target Type of the playlist")]
        public PlaylistType TargetPlaylistType { get; set; }

        [Option('d', "deviceType", Default = DeviceType.Mobile, HelpText = "Device Type of the playlist")]
        public DeviceType DeviceType { get; set; }

        [Value(0, Default="",HelpText="Path to the playlist(s).")]
        public string PlaylistsPath { get; set; }

    }
}