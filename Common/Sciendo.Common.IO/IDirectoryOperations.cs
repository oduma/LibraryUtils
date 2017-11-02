using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Common.IO
{
    public interface IDirectoryOperations
    {
        bool DirectoryExists(string path);

        string SafeCreate(string path);
    }
}
