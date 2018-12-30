using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIE
{
    public class ArtistWithRoles
    {
        public string Name { get; set; }

        public string ArtistId { get; set; }

        public List<ArtistRole> ArtistRoles { get; set; }

        public string ProcessedName { get; set; }
    }
}
