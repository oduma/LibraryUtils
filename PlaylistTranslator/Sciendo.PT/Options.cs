using CommandLine;

namespace Sciendo.PT
{
    public class Options
    {
        [Option('s', "source", Default = "", HelpText = "Path to the original playlist(s).")]
        public string Source { get; set; }

        [Option('d', "destination", Default = "", HelpText = "Path to the where the destination playlist(s) will be created.")]
        public string Destination { get; set; }
    }
}
