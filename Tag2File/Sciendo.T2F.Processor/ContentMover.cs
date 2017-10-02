using System;
using System.IO;
using Sciendo.Common.IO;

namespace Sciendo.T2F.Processor
{
    public class ContentMover:ContentWriterBase
    {
        public ContentMover(IDirectoryEnumerator directoryEnumerator, IFileEnumerator fileEnumerator) : base(directoryEnumerator, fileEnumerator)
        {
        }

        public override void Do(string fromPath, string toPath)
        {
            DoOperation(fromPath, toPath, File.Move);
        }
    }
}
