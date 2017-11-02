using System;

namespace Sciendo.Common.IO
{
    public class ExtensionsReadEventArgs : EventArgs
    {
        public string Extension { get; private set; }

        public ExtensionsReadEventArgs(string extension)
        {
            Extension = extension;
        }
    }
}
