using System;

namespace Sciendo.Playlist.Persister
{
    public class ProgressEventArgs:EventArgs
    {
        public string Path { get; private set; }

        public int ExpectedItemsToProcess { get; private set; }

        public ProgressEventArgs(string path, int itemsCount)
        {
            Path = path;
            ExpectedItemsToProcess = itemsCount;
        }
    }
}