using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sciendo.Common.Serialization;
using Sciendo.Love2Playlist.Processor.DataTypes;

namespace Sciendo.Love2Playlist.Processor
{
    public class Persister:IPersister
    {
        public string LoveFile { get; private set; }
        public string PlaylistFile { get; private set; }

        

        public Persister(string loveFile, string playlistFile)
        {
            LoveFile = loveFile;
            PlaylistFile = playlistFile;

        }
        public void SaveToLoveFile(List<LoveTrack> loveTracks,string userName, int pageNumber)
        {
            Serializer.SerializeToFile(loveTracks,string.Format( "{0} - {1} - {2}",pageNumber, userName, LoveFile));
        }

        public void SaveToPlaylistFile(string playlistContent)
        {
            File.AppendAllText(PlaylistFile,playlistContent);
        }

        public void Dispose()
        {
        }
    }
}
