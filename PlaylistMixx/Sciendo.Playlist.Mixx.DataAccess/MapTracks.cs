using System.Collections.Generic;
using Sciendo.Mixx.DataAccess.Domain;
using Sciendo.Playlists;

namespace Sciendo.Mixx.DataAccess
{
    public class MapTracks:IMap<IEnumerable<PlaylistItem>, IEnumerable<MixxPlaylistTrack>>
    {
        public IEnumerable<MixxPlaylistTrack> Transform(IEnumerable<PlaylistItem> fromDataType)
        {
            int index = 1;
            foreach (var fromDataTypeInstance in fromDataType)
            {
                yield return new MixxPlaylistTrack(fromDataTypeInstance.FileName, index++);
            }
        }

        public IEnumerable<PlaylistItem> Transform(IEnumerable<MixxPlaylistTrack> fromDataType)
        {
            foreach (var fromDataTypeInstance in fromDataType)
            {
                yield return new PlaylistItem {FileName = fromDataTypeInstance.FileName};
            }
        }
    }
}
