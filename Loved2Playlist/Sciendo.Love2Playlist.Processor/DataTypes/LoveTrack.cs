using System;
using System.Collections.Generic;

namespace Sciendo.Love2Playlist.Processor.DataTypes
{
    public class LoveTrack
    {
        public string Name { get; set; }
        public Guid MBID { get; set; }
        public Uri Url { get; set; }
        public DateTime Date { get; set; }
        public Artist Artist { get; set; }
        public IEnumerable<Image> Images { get; set; }
        public Streamable Streamable { get; set; }
    }
}
