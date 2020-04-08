using Sciendo.ArtistClassifier.Contracts.DataTypes;
using System.Collections.Generic;

namespace LIC.DataTypes
{
    public class TrackWithArtists : TrackWithFile
    {
        public List<Artist> Artists { get; set; }
    }
}