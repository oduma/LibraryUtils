using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sciendo.Common.IO;
using TagLib;
using File = System.IO.File;

namespace Sciendo.Common.Music.Tagging
{
    public class TagFileReader:IFileReader<Tag>, IFilesReader<Tag>
    {
        public Tag Read(string filePath)
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

        public IEnumerable<Tag> Read(IEnumerable<string> paths)
        {
            return paths.Select(Read).Where(tag => tag != null);
        }
    }
}
