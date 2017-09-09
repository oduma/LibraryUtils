using System;
using Newtonsoft.Json;

namespace Sciendo.Love2Playlist.Processor.DataTypes
{
    public class LoveDate
    {
        [JsonProperty("uts")]
        public double UnixTimeSeconds { get; set; }

        [JsonProperty("#text")]
        public DateTime Date { get; set; }

    }
}