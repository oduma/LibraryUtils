using System;

namespace Sciendo.Love2Playlist.Processor.DataTypes
{
    public class Artist
    {
        public string Name { get; set; }
        public Guid MBID { get; set; }
        public Uri Url { get; set; }
    }
}