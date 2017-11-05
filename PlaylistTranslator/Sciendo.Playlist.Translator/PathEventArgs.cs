using System;

namespace Sciendo.Playlist.Translator
{
    public class PathEventArgs:EventArgs
    {
        public string Path { get; private set; }

        public PathEventArgs(string path)
        {
            Path = path;
        }
    }
}