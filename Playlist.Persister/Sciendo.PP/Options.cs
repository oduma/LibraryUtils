using CommandLine;
using Sciendo.Playlist.Persister;

namespace Sciendo.PP
{
    internal class Options
    {
        [Option('t', "targetType", Default = PlaylistType.M3U, HelpText = "Target Type of the playlist")]
        public PlaylistType TargetPlaylistType { get; set; }

        [Value(0, Default="",HelpText="Path to the input playlist(s).")]
        public string InPlaylistsPath { get; set; }

        [Value(1, Default = "", HelpText = "Path to the output playlist(s).")]
        public string OutPlaylistsPath { get; set; }


    }
}