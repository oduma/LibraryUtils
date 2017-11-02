using CommandLine;
using Sciendo.Playlist.Persister;

namespace Sciendo.PP
{
    internal class Options
    {
        [Option('s', "musicSource", DefaultValue = "", HelpText = "Root folder of the files when the playlist was created")]
        public string MusicSourceRoot { get; set; }
        [Option('c', "musicCurrent", DefaultValue = "", HelpText = "Root folder of the files now")]
        public string MusicCurrentRoot { get; set; }

        [Option('t', "targetType", DefaultValue = PlaylistType.M3U, HelpText = "Target Type of the playlist")]
        public PlaylistType TargetPlaylistType { get; set; }

        [Option('d', "deviceType", DefaultValue = DeviceType.Mobile, HelpText = "Device Type of the playlist")]
        public DeviceType DeviceType { get; set; }

        [ValueOption(0)]
        public string PlaylistsPath { get; set; }

        public string GetHelpText()
        {
            return CommandLine.Text.HelpText.AutoBuild(this);
        }
    }
}