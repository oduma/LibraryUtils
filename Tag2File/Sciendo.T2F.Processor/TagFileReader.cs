using System;
using System.IO;
using TagLib;
using File = System.IO.File;

namespace Sciendo.T2F.Processor
{
    public class TagFileReader:IFileReader<Tag>
    {
        public Tag ReadFile(string filePath)
        {
            using (FileStream fs = File.OpenRead(filePath))
            {
                var fsa = new StreamFileAbstraction(filePath, fs, fs);
                try
                {
                    TagLib.File file = TagLib.File.Create(fsa);
                    return file.Tag;
                }
                catch (CorruptFileException cex)
                {
                    Console.WriteLine(cex);
                    return null;
                }
            }
        }
    }
}
