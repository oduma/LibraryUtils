using System;
using System.Collections.Generic;
using CsvHelper.Configuration.Attributes;

namespace LIE.DataTypes
{
    public class ArtistWithRoles
    {

        [Name("artistID:ID(Artist)")]
        public Guid ArtistId { get; set; }
        [Name("name")]
        public string Name { get; set; }
        [Name(":LABEL")]
        public List<ArtistRole> ArtistRoles { get; set; }
    }
}
