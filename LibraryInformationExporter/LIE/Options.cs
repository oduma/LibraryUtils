using CommandLine;

namespace LIE
{
    public class Options
    {
        [Option('r', "rescan", Default = false, HelpText = "Rescan the library or use a previous obtained non processed file?")]
        public bool Rescan { get; set; }

        [Option('p', "process", Default = false, HelpText = "Process the file again or use a pre-processed set of files?")]
        public bool Process { get; set; }

    }
}
