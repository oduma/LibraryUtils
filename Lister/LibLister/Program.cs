using System;
using CommandLine;
using Sciendo.Common.IO;
using Sciendo.Common.IO.MTP;
using Sciendo.Common.Serialization;
using Sciendo.Library.Lister;

namespace LibLister
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args);
            if (result.Tag==ParserResultType.Parsed)
            {
                var options = ((Parsed<Options>) result).Value;
                IStorage storage = GetStorage(options.Root);
                LibraryParser libraryParser = new LibraryParser(options.Root,options.MusicExtensions.Split(';'),options.Include, 
                    options.IncludeSize, storage.Directory);
                libraryParser.ItemParsed += LibraryParser_ItemParsed;
                Serializer.SerializeToFile(libraryParser.ParseLibrary(),options.OutputFile);
            }
            else
            {
                CommandLine.Text.HelpText.AutoBuild(result);
            }
        }

        private static IStorage GetStorage(string path)
        {
            if(MtpPathInterpreter.IsMtpDevice(path))
                return new MtpStorage();
            return new FsStorage();
        }

        private static void LibraryParser_ItemParsed(object sender, ItemParsedEventArgs e)
        {
            Console.WriteLine(e.ItemParsedMessage);
        }
    }
}
