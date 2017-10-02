using System;
using System.Collections.Generic;
using CommandLine;

namespace Sciendo.T2F
{
    public class Options
    {
        [ValueOption(0)]
        public string RootPath { get; set; }

        [ValueOption(1)]
        public ActionType ActionType { get; set; }

        [Option('i',"individual",DefaultValue="",HelpText="For files part of an individual album. Pattern can include:\r\n\t\t%aa=album artist\r\n\t\t%a=artist\r\n\t\t%l=album\r\n\t\t%t=title\r\n\t\t%n=track no")]
        public string IndividualPattern { get; set; }

        [Option('c', "collection", DefaultValue = "", HelpText = "For files part of a collection album. Pattern can include:\r\n\t\t%aa=album artist\r\n\t\t%a=artist\r\n\t\t%l=album\r\n\t\t%t=title\r\n\t\t%n=track no")]
        public string CollectionPattern { get; set; }

        public string GetHelpText()
        {
            return CommandLine.Text.HelpText.AutoBuild(this);
        }

    }
}
