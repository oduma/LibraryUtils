using Sciendo.Playlist.Handler.XSPF;
using Sciendo.Playlists.M3U;

namespace Sciendo.Playlists
{
    public class PlaylistHandlerFactory:IPlaylistHandlerFactory
    {
        private const string XSPFExtension = "xspf";
        private const string M3UExtension = "m3u";
        public IPlaylistHandler GetHandler(string playlistExtension)
        {
            switch (playlistExtension.Trim().Replace(".", "").ToLower())
            {
                case XSPFExtension:
                {
                    return new XSPFHandler();
                }
                case M3UExtension:
                {
                    return new M3UHandler();
                }
                default:
                    return null;
            }
        }
    }
}
