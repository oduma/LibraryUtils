using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sciendo.Love2Playlist.Processor.DataTypes;

namespace Sciendo.Love2Playlist.Processor
{
    public class Coordinator: ICoordinator
    {
        private readonly ILoveProvider _loveProvider;
        private readonly IPersister _persister;
        public event EventHandler<CollectLoveEventArgs> CollectedLove;
        public event EventHandler<SaveLoveEventArgs> SavedLove;  
        public Coordinator(ILoveProvider loveProvider,IPersister persister)
        {
            _loveProvider = loveProvider;
            _persister = persister;
        }
        public void GetLovedAndPersistPlaylist()
        {
            int currentLovedPage = 1;
            int maxLovedPages = 0;
            List<LoveTrack> loveTracksBatch= new List<LoveTrack>();
            do
            {
                var lovePage = _loveProvider.GetPage(currentLovedPage);
                if (maxLovedPages == 0)
                    maxLovedPages = lovePage.AdditionalAttributes.TotalPages;
                var loveTracks = lovePage.LoveTracks;
                CollectedLove?.Invoke(this, new CollectLoveEventArgs(currentLovedPage, maxLovedPages));
                loveTracksBatch.AddRange(loveTracks);
                _persister.SaveToLoveFile(loveTracks);
                SavedLove?.Invoke(this,new SaveLoveEventArgs(currentLovedPage++,_persister.LoveFile));
            } while (currentLovedPage<maxLovedPages);
        }
    }
}
