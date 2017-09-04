using System;

namespace Sciendo.Love2Playlist.Processor
{
    public class CollectLoveEventArgs:EventArgs
    {
        public int CurrentPage { get; private set; }

        public int TotalPages { get; private set; }

        public CollectLoveEventArgs(int currentPage, int totalPages)
        {
            CurrentPage = currentPage;
            TotalPages = totalPages;
        }
    }
}