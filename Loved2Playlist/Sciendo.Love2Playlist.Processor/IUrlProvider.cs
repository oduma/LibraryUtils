using System;

namespace Sciendo.Love2Playlist.Processor
{
    public interface IUrlProvider
    {
        Uri GetUrl(int pageNumber);
    }
}