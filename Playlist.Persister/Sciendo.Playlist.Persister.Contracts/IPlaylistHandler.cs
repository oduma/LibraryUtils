namespace Sciendo.Playlist.Handler.Contracts
{
    public interface IPlaylistHandler
    {
        PlaylistItem[] GetPlaylistItems(string playlistContents);

        string SetPlaylistItems(string playlistContents, PlaylistItem[] playlistItems);
    }
}
