using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Sciendo.Love2Playlist.Processor.DataTypes
{
    public class LovePage
    {
        [JsonProperty("@attr")]
        public AdditionalAttributes AdditionalAttributes { get; set; }

        [JsonProperty("track")]
        public LoveTrack[] LoveTracks{ get; set; }
    }
}