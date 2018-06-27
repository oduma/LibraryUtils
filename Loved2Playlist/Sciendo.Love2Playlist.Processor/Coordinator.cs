using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sciendo.Clementine.DataAccess;
using Sciendo.Common.IO;
using Sciendo.Love2Playlist.Processor.DataTypes;
using Sciendo.Playlists;
using TagLib;

namespace Sciendo.Love2Playlist.Processor
{
    public class Coordinator: ICoordinator
    {
        private readonly ILoveProvider _loveProvider;
        private readonly IDataProvider _dataProvider;
        private readonly IPlaylistHandler _playlistHandler;
        private readonly IFile _file;
        private readonly string _userName;
        private readonly string _rootFileName;
        public event EventHandler<CollectLoveEventArgs> CollectedLove;
        public event EventHandler<SaveLoveEventArgs> SavedLove;
        public event EventHandler<SavePlaylistEventArgs> SavedPlaylist;

        private string PlaylistFile => $"playlist-{_userName}-{_rootFileName}.m3u";

        private int _totalPlaylistFiles; 
        public Coordinator(ILoveProvider loveProvider,IDataProvider dataProvider, IPlaylistHandler playlistHandler,IFile file, string userName, string rootFileName)
        {
            _loveProvider = loveProvider;
            _dataProvider = dataProvider;
            _playlistHandler = playlistHandler;
            _file = file;
            _userName = userName;
            _rootFileName = rootFileName;
        }
        public void GetLovedAndPersistPlaylist()
        {
            _totalPlaylistFiles = 0;
            var currentLovedPage = 1;
            var maxLovedPages = 0;

            List<LoveTrack> loveTracks= new List<LoveTrack>();
            do
            {
                var lovePage = _loveProvider.GetPage(currentLovedPage);
                if (maxLovedPages == 0)
                    maxLovedPages = lovePage.AdditionalAttributes.TotalPages;
                var lovePageTracks = lovePage.LoveTracks;
                loveTracks.AddRange(lovePageTracks);
                CollectedLove?.Invoke(this, new CollectLoveEventArgs(currentLovedPage++, maxLovedPages));
            } while (currentLovedPage <= maxLovedPages);

            var playlistContents = _playlistHandler.SetPlaylistItems(_file,
                GetFilesForLoveTracks(loveTracks).ToArray());
            _file.WriteAllText(PlaylistFile,playlistContents);
            SavedPlaylist?.Invoke(this, new SavePlaylistEventArgs(_totalPlaylistFiles, PlaylistFile));
        }

        private IEnumerable<PlaylistItem> GetFilesForLoveTracks(IList<LoveTrack> loveTracks)
        {
            if (_dataProvider.LastRefresh.AddMinutes(5) < DateTime.Now)
            {
                _dataProvider.Load();
            }

            foreach (var loveTrack in loveTracks)
            {
                var bestTrack = _dataProvider.AllTracks.FilterAndRank(loveTrack).OrderBy(t => t.Rank).LastOrDefault();
                if (bestTrack != null)
                {
                    _totalPlaylistFiles++;
                    var urlDecoded = HttpUtility.UrlDecode(bestTrack.FileName);
                    if (urlDecoded != null)
                        yield return new PlaylistItem
                        {
                            FileName = urlDecoded.Replace("file:///", "").Replace(@"/", @"\")
                        };
                }
            }
        }

    }

}
