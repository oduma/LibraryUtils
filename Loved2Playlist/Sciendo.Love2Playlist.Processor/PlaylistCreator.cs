using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sciendo.Clementine.DataAccess;
using Sciendo.Love2Playlist.Processor.DataTypes;

namespace Sciendo.Love2Playlist.Processor
{
    public class PlaylistCreator:IPlaylistCreator
    {
        private readonly IDataProvider _dataProvider;

        public PlaylistCreator(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }
        public string AddToPlaylist(IList<LoveTrack> loveTracks)
        {
            if (_dataProvider.LastRefresh.AddMinutes(5) > DateTime.Now)
            {
                _dataProvider.Load();
            }
            return string.Join(Environment.NewLine + "#EXTINF",_dataProvider.AllTracks.Where(
                    t =>
                        loveTracks.Any(
                            l => String.Equals(l.Name, t.Title, StringComparison.CurrentCultureIgnoreCase) && String.Equals(l.Artist.Name, t.Artist, StringComparison.CurrentCultureIgnoreCase)))
                .Select(t => t.FilePath));
        }
    }
}
