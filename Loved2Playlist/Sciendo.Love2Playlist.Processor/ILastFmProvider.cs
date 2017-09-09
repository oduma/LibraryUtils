using System;

namespace Sciendo.Love2Playlist.Processor
{
    public interface ILastFmProvider
    {
        string GetLastFmContent(Uri lastFmUri);
    }
}