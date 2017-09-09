using Newtonsoft.Json;

namespace Sciendo.Love2Playlist.Processor.DataTypes
{
    public class AdditionalAttributes
    {
        [JsonProperty("page")]
        public int PageNumber { get; set; }

        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }

        [JsonProperty("perPage")]
        public int PerPage { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("user")]
        public string UserName { get; set; }
    }
}