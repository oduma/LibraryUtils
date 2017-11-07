using CommandLine;
using Sciendo.Library.Lister;

namespace LibLister
{
    public class Options
    {
        [Option('r', "root", Default = ".", HelpText = "Path to the root folder.", Required = true)]
        public string Root { get; set; }

        [Option('m', "musicext", Default = ".mp3;.ogg;.flac;.m4a", Required = true, HelpText = "Extensions for the music files separated by semicolon")]
        public string MusicExtensions { get; set; }

        [Option('i', "include", Default = Scope.MusicOnly, Required = true, HelpText = "Can be one of the following: All, MusicOnly")]
        public Scope Include { get; set; }

        [Option('s', "size", Default = false, Required = false, HelpText = "true to include size, false to not")]
        public bool IncludeSize { get; set; }

        [Option('o', "output", Default = "lib.xml", Required = true, HelpText = "Name of the file for the list of the library.")]
        public string OutputFile { get; set; }
    }
}
