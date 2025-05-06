namespace Sciendo.Playlist.Appender
{
    public interface IPlaylistAppender
    {
        void Append(string fromPath, string fromMusicRootPath, string toPath, string toMusicRootPath);
    }
}
