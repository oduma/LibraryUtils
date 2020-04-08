using Sciendo.ArtistClassifier.Contracts.DataTypes;
using System.Collections.Generic;

namespace LIC.DataTypes
{
    public class TrackByArtist : TrackWithFile
    {
        public ArtistType Type { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsComposer { get; set; }
    }
}