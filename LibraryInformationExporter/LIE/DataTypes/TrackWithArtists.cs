using System.Collections.Generic;

namespace LIE.DataTypes
{
    public class TrackWithArtists:TrackWithFile
    {
        public List<Artist> Artists { get; set; } 
    }
}