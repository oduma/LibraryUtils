using Newtonsoft.Json;

namespace Sciendo.Love2Playlist.Processor.DataTypes
{
    public class Artist
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("mbid")]
        public string Mbid { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}