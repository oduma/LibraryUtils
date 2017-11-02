using System.IO;

namespace Sciendo.Common.IO
{
    public class ContentCopier:ContentWriterBase
    {
        public ContentCopier(IDirectoryEnumerator directoryEnumerator, IFileEnumerator fileEnumerator) : base(directoryEnumerator, fileEnumerator)
        {
        }

        public override void Do(string fromPath, string toPath)
        {
            DoOperation(fromPath,toPath,File.Copy);
        }
    }
}
