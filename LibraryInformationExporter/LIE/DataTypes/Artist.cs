using System;
using CsvHelper.Configuration.Attributes;

namespace LIE.DataTypes
{
    public class Artist
    {

        [Name("artistID:ID(Artist)")]
        public Guid ArtistId { get; set; }
        [Name("name")]
        public string Name { get; set; }
        [Name(":LABEL")]
        public ArtistType Type { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsComposer { get; set; }

    }
}
