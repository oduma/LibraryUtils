using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sciendo.Clementine.DataAccess;
using Sciendo.Love2Playlist.Processor.DataTypes;

namespace Sciendo.Love2Playlist.Processor
{
    public class Coordinator: ICoordinator
    {
        private readonly ILoveProvider _loveProvider;
        private readonly IPersister _persister;
        private readonly IPlaylistCreator _playlistCreator;
        public event EventHandler<CollectLoveEventArgs> CollectedLove;
        public event EventHandler<SaveLoveEventArgs> SavedLove;
        public event EventHandler<SavePlaylistEventArgs> SavedPlaylist; 
        public Coordinator(ILoveProvider loveProvider,IPersister persister,IPlaylistCreator playlistCreator)
        {
            _loveProvider = loveProvider;
            _persister = persister;
            _playlistCreator = playlistCreator;
        }
        public void GetLovedAndPersistPlaylist()
        {
            int currentLovedPage = 1;
            int maxLovedPages = 0;
            _persister.SaveToPlaylistFile("#EXTM3U");
            do
            {
                var lovePage = _loveProvider.GetPage(currentLovedPage);
                if (maxLovedPages == 0)
                    maxLovedPages = lovePage.AdditionalAttributes.TotalPages;
                var loveTracks = lovePage.LoveTracks;
                CollectedLove?.Invoke(this, new CollectLoveEventArgs(currentLovedPage, maxLovedPages));
                _persister.SaveToLoveFile(loveTracks.ToList(), lovePage.AdditionalAttributes.UserName, lovePage.AdditionalAttributes.PageNumber);
                SavedLove?.Invoke(this, new SaveLoveEventArgs(currentLovedPage++, _persister.LoveFile));
                var playlistFragment = _playlistCreator.AddToPlaylist(loveTracks);
                _persister.SaveToPlaylistFile(playlistFragment);
                SavedPlaylist?.Invoke(this, new SavePlaylistEventArgs(_persister.PlaylistFile));
            } while (currentLovedPage < maxLovedPages);
        }
    }
}
