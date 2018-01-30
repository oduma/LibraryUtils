using Sciendo.Playlists.M3U;
using Sciendo.Playlists.XSPF;

namespace Sciendo.Playlists
{
    public static class PlaylistHandlerFactory
    {
        private const string XSPFExtension = "xspf";
        private const string M3UExtension = "m3u";
        public static IPlaylistHandler GetHandler(string playlistExtension)
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
