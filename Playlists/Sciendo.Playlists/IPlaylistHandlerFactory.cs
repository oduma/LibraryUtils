namespace Sciendo.Playlists
{
    public interface IPlaylistHandlerFactory
    {
        IPlaylistHandler GetHandler(string playlistExtension);
    }
}
