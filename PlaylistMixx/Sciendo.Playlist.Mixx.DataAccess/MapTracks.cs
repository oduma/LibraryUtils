using System.Collections.Generic;
using Sciendo.Mixx.DataAccess.Domain;
using Sciendo.Playlists;

namespace Sciendo.Mixx.DataAccess
{
    public class MapTracks:IMap<IEnumerable<PlaylistItem>, IEnumerable<MixxxPlaylistTrack>>
    {
        public IEnumerable<MixxxPlaylistTrack> Transform(IEnumerable<PlaylistItem> fromDataType)
        {
            int index = 1;
            foreach (var fromDataTypeInstance in fromDataType)
            {
                yield return new MixxxPlaylistTrack(fromDataTypeInstance.FileName, index++);
            }
        }

        public IEnumerable<PlaylistItem> Transform(IEnumerable<MixxxPlaylistTrack> fromDataType)
        {
            foreach (var fromDataTypeInstance in fromDataType)
            {
                var tempFileName = fromDataTypeInstance.FileName.Replace(@"/", @"\");
                tempFileName = (tempFileName.StartsWith(@"\")) ? tempFileName.Substring(1) : tempFileName;
                
                yield return new PlaylistItem {FileName = tempFileName};
            }
        }
    }
}
