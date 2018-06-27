using System;
using System.IO;
using Sciendo.Common.IO;
using TagLib;

namespace Sciendo.Common.Music.Tagging
{
    public static class FsFileExtensions
    {
        public static TagLib.File ReadTag(this IFile fsFile, string path)
        {
            using (MemoryStream fs = new MemoryStream(fsFile.Read(path)))
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
}
