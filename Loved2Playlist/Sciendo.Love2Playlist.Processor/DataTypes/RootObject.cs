using Newtonsoft.Json;

namespace Sciendo.Love2Playlist.Processor.DataTypes
{
    public class RootObject
    {
        [JsonProperty("lovedtracks")]
        public LovePage LovePage { get; set; }
    }
}
