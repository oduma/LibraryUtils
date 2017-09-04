using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sciendo.Common.Serialization;
using Sciendo.Love2Playlist.Processor.DataTypes;

namespace Sciendo.Love2Playlist.Processor
{
    public class Persister:IPersister
    {
        public string LoveFile { get; }
        public string PlaylistFile { get; }
        public void SaveToLoveFile(List<LoveTrack> loveTracks,string userName, int pageNumber)
        {
            Serializer.SerializeToFile(loveTracks,string.Format( "{0} - {1} - {2}",pageNumber, userName, LoveFile));
        }

        public void SaveToPlaylistFile()
        {
            throw new NotImplementedException();
        }
    }
}
