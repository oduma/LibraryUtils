using CommandLine;

namespace Sciendo.PP
{
    internal class Options
    {
        [Option('s', "musicSource", DefaultValue = "", HelpText = "Root folder of the files when the playlist was created")]
        public string MusicSourceRoot { get; set; }
        [Option('c', "musicCurrent", DefaultValue = "", HelpText = "Root folder of the files now")]
        public string MusicCurrentRoot { get; set; }

        [ValueOption(0)]
        public string PlaylistsDirectory { get; set; }

        public string GetHelpText()
        {
            return CommandLine.Text.HelpText.AutoBuild(this);
        }
    }
}