using System;

namespace Sciendo.Love2Playlist.Processor
{
    public interface ICoordinator
    {
        void GetLovedAndPersistPlaylist();
        event EventHandler<CollectLoveEventArgs> CollectedLove;
        event EventHandler<SaveLoveEventArgs> SavedLove;
        event EventHandler<SavePlaylistEventArgs> SavedPlaylist;

    }
}
