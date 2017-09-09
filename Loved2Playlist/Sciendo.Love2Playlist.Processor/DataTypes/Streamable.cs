using Newtonsoft.Json;

namespace Sciendo.Love2Playlist.Processor.DataTypes
{
    public class Streamable
    {
        [JsonProperty("#text")]
        public string Text { get; set; }
        [JsonProperty("fulltrack")]
        public string FullTrack { get; set; }
    }
}
