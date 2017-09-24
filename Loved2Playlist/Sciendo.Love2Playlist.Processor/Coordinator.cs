using System;
using System.Collections.Generic;
using System.Linq;
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
            int totalLoveReceived = 0;
            int totalPlaylist = 0;
            _persister.SaveToPlaylistFile($"#EXTM3U{Environment.NewLine}");
            do
            {
                var lovePage = _loveProvider.GetPage(currentLovedPage);
                if (maxLovedPages == 0)
                    maxLovedPages = lovePage.AdditionalAttributes.TotalPages;
                var loveTracks = lovePage.LoveTracks;
                CollectedLove?.Invoke(this, new CollectLoveEventArgs(currentLovedPage, maxLovedPages));
                _persister.SaveToLoveFile(loveTracks.ToList(), lovePage.AdditionalAttributes.UserName, lovePage.AdditionalAttributes.PageNumber);
                totalLoveReceived += loveTracks.Length;
                SavedLove?.Invoke(this, new SaveLoveEventArgs(currentLovedPage++, totalLoveReceived,_persister.LoveFile));
                var playlistFragment = _playlistCreator.AddToPlaylist(loveTracks);
                _persister.SaveToPlaylistFile(playlistFragment.PlaylistContent);
                totalPlaylist += playlistFragment.PlaylistFiles;
                SavedPlaylist?.Invoke(this, new SavePlaylistEventArgs(totalPlaylist,_persister.PlaylistFile));
            } while (currentLovedPage <= maxLovedPages);
        }
    }
}
