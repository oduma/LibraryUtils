using Sciendo.Playlist.Handler.Contracts;
using Sciendo.Playlist.Handler.XSPF;

namespace Sciendo.Playlist.Persister
{
    public class PlaylistHandlerFactory:IPlaylistHandlerFactory
    {
        private const string XSPFExtension = "xspf";
        public IPlaylistHandler GetHandler(string playlistExtension)
        {
            switch (playlistExtension.Trim().Replace(".", "").ToLower())
            {
                case XSPFExtension:
                {
                    return new XSPFHandler();
                }
                default:
                    return null;
            }
        }
    }
}
