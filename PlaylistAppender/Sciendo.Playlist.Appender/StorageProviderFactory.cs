using Sciendo.Common.IO;
using Sciendo.Common.IO.MTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Playlist.Appender
{
    public static class StorageProviderFactory 
    {
        public static IStorage GetStorageProvider(string rootPath)
        {
            if (MtpPathInterpreter.IsMtpDevice(rootPath))
                return new MtpStorage();
            return new FsStorage();
        }
    }
}
