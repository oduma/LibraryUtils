using CommandLine;
using Sciendo.Playlist.Appender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Pap
{
    internal class Options
    {

        [Option('t', "targetRootMusicFolder", Default = "", HelpText = "Root Path of the Music in the target playlist")]
        public string TargetPlaylistMusicRootPath { get; set; }

        [Option('s', "sourceRootMusicFolder", Default = "", HelpText = "RootPath of the Music in the source playlist")]
        public string SourcePlaylistMusicRootPath { get; set; }

        [Value(0, Default = "", HelpText = "Path to the input playlist(s).")]
        public string InPlaylistsPath { get; set; }

        [Value(1, Default = "", HelpText = "Path to the output playlist(s).")]
        public string OutPlaylistsPath { get; set; }


    }
}
