using System.IO;

namespace Sciendo.Common.IO
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
