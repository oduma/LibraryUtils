using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace Sciendo.Common.Music.Tagging
{
    public static class TagReader
    {
        public static TagLib.File ReadTag(MemoryStream fs, string path)
        {
            var fsa = new StreamFileAbstraction(path, fs, fs);
            try
            {
                TagLib.File file = TagLib.File.Create(fsa);
                return file;
            }
            catch (CorruptFileException cex)
            {
                Console.WriteLine(cex);
                return null;
            }

        }
    }
}
