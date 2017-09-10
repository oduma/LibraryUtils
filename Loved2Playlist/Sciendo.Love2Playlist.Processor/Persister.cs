using System.Collections.Generic;
using System.IO;
using Sciendo.Common.Serialization;
using Sciendo.Love2Playlist.Processor.DataTypes;

namespace Sciendo.Love2Playlist.Processor
{
    public class Persister:IPersister
    {
        public string LoveFile => $"{_pageNumber} - {_userName} - {_rootFileName}";
        public string PlaylistFile { get; private set; }

        private readonly string _rootFileName;
        private int _pageNumber;
        private readonly string _userName;


        public Persister(string rootFileName, string userName)
        {
            _rootFileName = rootFileName;
            _pageNumber = 0;
            _userName = userName;
        }
        public void SaveToLoveFile(List<LoveTrack> loveTracks,string userName, int pageNumber)
        {
            _pageNumber = pageNumber;
            Serializer.SerializeToFile(loveTracks,LoveFile);
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
