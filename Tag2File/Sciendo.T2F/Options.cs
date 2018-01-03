using CommandLine;

namespace Sciendo.T2F
{
    public class Options
    {
        [Value(0, Default = "",HelpText = "The path where the music files are located.")]
        public string RootPath { get; set; }
    }
}
