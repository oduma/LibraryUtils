using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AE.DataTypes
{
    public class RelationBandArtist
    {
        [Name(":START_ID(Artist)")]
        public Guid BandId { get; set; }

        [Name(":END_ID(Artist)")]
        public Guid ArtistId { get; set; }
    }
}
