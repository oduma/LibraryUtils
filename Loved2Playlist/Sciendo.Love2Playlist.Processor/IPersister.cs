using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sciendo.Love2Playlist.Processor.DataTypes;

namespace Sciendo.Love2Playlist.Processor
{
    public interface IPersister
    {
        string LoveFile { get; }

        string PlaylistFile { get; }

        void SaveToLoveFile(List<LoveTrack> loveTracks, string userName, int pageNumber);

        void SaveToPlaylistFile();
    }
}
