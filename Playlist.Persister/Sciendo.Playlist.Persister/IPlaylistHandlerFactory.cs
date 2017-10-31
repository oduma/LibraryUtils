using Sciendo.Playlists;

namespace Sciendo.Playlist.Persister
{
    public interface IPlaylistHandlerFactory
    {
        IPlaylistHandler GetHandler(string playlistExtension);
    }
}
