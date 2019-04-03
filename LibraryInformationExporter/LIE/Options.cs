using CommandLine;

namespace LIE
{
    public class Options
    {
        [Option('r', "rescan", Default = false, HelpText = "Rescan the library or use a previous obtained non processed file?")]
        public bool Rescan { get; set; }

        [Option('p', "process", Default = false, HelpText = "Process the file again or use a pre-processed set of files?")]
        public bool Process { get; set; }

        //[Option('o', "possible", Default = false, HelpText = "Extract the tracks with possible featured artists from tracks or used a previous file?")]
        //public bool PossibleFeatured { get; set; }

        //[Option('n', "nonFeaturedRelations", Default = false, HelpText = "Calculate relations exclude from features")]
        //public bool RelationsOnly { get; set; }

        //[Option('f', "featured", Default = false, HelpText = "Add featured artists to the artists and relate them to tracks")]
        //public bool Featured { get; set; }

        

    }
}
