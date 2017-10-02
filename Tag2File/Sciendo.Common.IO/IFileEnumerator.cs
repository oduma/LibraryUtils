using System;
using System.Collections.Generic;
using System.IO;

namespace Sciendo.Common.IO
{
    public interface IFileEnumerator
    {
        IEnumerable<string> Get(string path, SearchOption searchOption, string[] extensions = null);

        event EventHandler<DirectoryReadEventArgs> DirectoryRead;

        event EventHandler<ExtensionsReadEventArgs> ExtensionsRead;

    }
}
