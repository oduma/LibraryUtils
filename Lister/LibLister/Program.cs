using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using Sciendo.Common.IO;
using Sciendo.Common.Serialization;
using Sciendo.Library.Lister;

namespace LibLister
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = CommandLine.Parser.Default.ParseArguments<Options>(args);
            if (result.Tag==ParserResultType.Parsed)
            {
                var options = ((Parsed<Options>) result).Value;
                LibraryParser libraryParser = new LibraryParser(options.Root,options.MusicExtensions.Split(';'),options.Include, options.IncludeSize, new FileEnumerator(), new DirectoryEnumerator());
                libraryParser.ItemParsed += LibraryParser_ItemParsed;
                Serializer.SerializeToFile(libraryParser.ParseLibrary(),options.OutputFile);
            }
            else
            {
                CommandLine.Text.HelpText.AutoBuild(result);
            }
        }

        private static void LibraryParser_ItemParsed(object sender, ItemParsedEventArgs e)
        {
            Console.WriteLine(e.ItemParsedMessage);
        }
    }
}
