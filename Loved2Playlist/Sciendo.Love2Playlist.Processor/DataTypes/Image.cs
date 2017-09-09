using System;
using Newtonsoft.Json;

namespace Sciendo.Love2Playlist.Processor.DataTypes
{
    public class Image
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("size")]
        public Size Size { get; set; }
    }
}
