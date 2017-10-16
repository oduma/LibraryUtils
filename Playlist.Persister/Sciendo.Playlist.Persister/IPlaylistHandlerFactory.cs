using Sciendo.Playlist.Handler.Contracts;

namespace Sciendo.Playlist.Persister
{
    public interface IPlaylistAnaliserFactory
    {
        IPlaylistHandler GetAnaliser(string playlistExtension);
    }
}
