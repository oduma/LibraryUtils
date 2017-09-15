using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Sciendo.Clementine.DataAccess;
using Sciendo.Clementine.DataAccess.DataTypes;
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
        public PartPlaylist AddToPlaylist(IList<LoveTrack> loveTracks)
        {
            if (_dataProvider.LastRefresh.AddMinutes(5) < DateTime.Now)
            {
                _dataProvider.Load();
            }
            var foundFiles = GetFilesForLoveTracks(loveTracks).ToArray();
            return new PartPlaylist
            {
                PlaylistContent = $"{Environment.NewLine}#EXTINF{Environment.NewLine}{string.Join(Environment.NewLine + "#EXTINF" + Environment.NewLine, foundFiles)}",
                PlaylistFiles = foundFiles.Length
            };
        }

        private IEnumerable<string> GetFilesForLoveTracks(IList<LoveTrack> loveTracks)
        {
            foreach (var loveTrack in loveTracks)
            {
                yield return TryGetFileForLoveTrack(loveTrack);
            }
        }

        private string TryGetFileForLoveTrack(LoveTrack loveTrack)
        {
            var bestTrack = _dataProvider.AllTracks.FilterAndRank(loveTrack).OrderBy(t => t.Rank).LastOrDefault();
            if (bestTrack == null)
                return $"File Not Found for {loveTrack.Name} - {loveTrack.Artist.Name}";
            return GetFileFromTrack(bestTrack);
        }

        private string GetFileFromTrack(RankedFile bestTrack)
        {
            return HttpUtility.UrlDecode(bestTrack.FileName).Replace("file:///", "").Replace(@"/", @"\");
        }
    }
}
