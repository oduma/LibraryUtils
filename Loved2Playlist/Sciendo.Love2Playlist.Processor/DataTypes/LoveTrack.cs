using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Sciendo.Love2Playlist.Processor.DataTypes
{
    
    public class LoveTrack
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("mbid")]
        public string MBID { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("date")]
        public LoveDate Date { get; set; }
        [JsonProperty("artist")]
        public Artist Artist { get; set; }
        [JsonProperty("image")]
        [XmlIgnore]
        public IEnumerable<Image> Images { get; set; }
        [JsonProperty("streamable")]
        public Streamable Streamable { get; set; }
    }
}
