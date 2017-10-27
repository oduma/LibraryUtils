using System.Runtime.Remoting.Messaging;
using Sciendo.Playlist.Handler.Contracts;
using Sciendo.Playlist.Handler.M3U;
using Sciendo.Playlist.Handler.XSPF;

namespace Sciendo.Playlist.Persister
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
