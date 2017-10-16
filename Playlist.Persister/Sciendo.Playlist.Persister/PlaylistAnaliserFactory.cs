using Sciendo.Playlist.Handler.Contracts;
using Sciendo.Playlist.Handler.XSPF;

namespace Sciendo.Playlist.Persister
{
    public class PlaylistAnaliserFactory:IPlaylistAnaliserFactory
    {
        private const string XSPFExtension = "xspf";
        public IPlaylistHandler GetAnaliser(string playlistExtension)
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
