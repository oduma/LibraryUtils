using System;
using System.Linq;
using CsvHelper.Configuration.Attributes;

namespace LIC.DataTypes
{
    public class ArtistNode
    {

        [Name("artistID:ID(Artist)")]
        public Guid ArtistId { get; set; }
        [Name("name")]
        public string Name { get; set; }
        [Name(":LABEL")]
        public string ArtistLabel { get; set; }

    }
}
