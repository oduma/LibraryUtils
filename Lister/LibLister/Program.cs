using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sciendo.Common.IO;
using Sciendo.Common.Serialization;
using Sciendo.Library.Lister;

namespace LibLister
{
    class Program
    {
        static void Main(string[] args)
        {
            Options options = new Options();
            var result = CommandLine.Parser.Default.ParseArguments(args, options);
            if (result)
            {
                LibraryParser libraryParser = new LibraryParser(options.Root,options.MusicExtensions.Split(';'),options.Include, options.IncludeSize, new FileEnumerator(), new DirectoryEnumerator());
                libraryParser.ItemParsed += LibraryParser_ItemParsed;
                Serializer.SerializeToFile(libraryParser.ParseLibrary(),options.OutputFile);
            }

        }

        private static void LibraryParser_ItemParsed(object sender, ItemParsedEventArgs e)
        {
            Console.WriteLine(e.ItemParsedMessage);
        }
    }
}
