using CommandLine;

namespace Sciendo.T2F
{
    public class Options
    {
        [Value(0, Default = "",HelpText = "The path where the music files are located.")]
        public string RootPath { get; set; }

        [Value(1,Default=ActionType.Copy,HelpText = "Copy to copy the files, Move to move the files.")]
        public ActionType ActionType { get; set; }

        [Option('i',"individual",Default="",HelpText="For files part of an individual album. Pattern can include:\r\n\t\t%aa=album artist\r\n\t\t%a=artist\r\n\t\t%l=album\r\n\t\t%t=title\r\n\t\t%n=track no")]
        public string IndividualPattern { get; set; }

        [Option('c', "collection", Default = "", HelpText = "For files part of a collection album. Pattern can include:\r\n\t\t%aa=album artist\r\n\t\t%a=artist\r\n\t\t%l=album\r\n\t\t%t=title\r\n\t\t%n=track no")]
        public string CollectionPattern { get; set; }

    }
}
