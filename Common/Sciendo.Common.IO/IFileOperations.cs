using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Common.IO
{
    public interface IFileOperations
    {
        bool FileExists(string path);

        string ReadAllText(string path);

        void WriteAllText(string path, string text);
    }
}
