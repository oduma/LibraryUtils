using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Common.IO
{
    public class FileEnumerator:IFileEnumerator
    {
        public IEnumerable<string> Get(string path, SearchOption searchOption, string[] extensions=null)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            if (!Directory.Exists(path))
                throw new ArgumentException("Folder does not exist.", nameof(path));
            DirectoryRead?.Invoke(this, new DirectoryReadEventArgs(path));
            if (extensions == null)
            {
                ExtensionsRead?.Invoke(this, new ExtensionsReadEventArgs("*.*"));
                foreach (var file in Directory.EnumerateFiles(path, "*.*", searchOption))
                {
                    yield return file;
                }
            }
            else
            {
                foreach (var extension in extensions)
                {
                    ExtensionsRead?.Invoke(this, new ExtensionsReadEventArgs(extension));
                    foreach (var file in Directory.EnumerateFiles(path, "*.*", searchOption)
                        .Where(s => s.EndsWith(extension)))
                    {
                        yield return file;
                    }

                }
            }
        }

        public event EventHandler<DirectoryReadEventArgs> DirectoryRead;
        public event EventHandler<ExtensionsReadEventArgs> ExtensionsRead;
    }
}
