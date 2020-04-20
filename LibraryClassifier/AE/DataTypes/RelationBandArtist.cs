using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AE.DataTypes
{
    public class RelationBandArtist
    {
        public Guid BandId { get; set; }

        public Guid ArtistId { get; set; }
    }
}
