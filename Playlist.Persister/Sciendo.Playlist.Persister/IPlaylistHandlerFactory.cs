using Sciendo.Playlist.Handler.Contracts;

namespace Sciendo.Playlist.Persister
{
    public interface IPlaylistHandlerFactory
    {
        IPlaylistHandler GetHandler(string playlistExtension);
    }
}
