using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AE.DataTypes
{
    public class OutputArtistNode: ArtistNode
    {
        [Name("wikiPage")]

        public string WikiPage { get; set; }

        [Name("artistInCollection")]
        public bool ArtistInCollection { get; set; }

        [Name("manualValidated")]
        public bool ManualValidated { get; set; }

        public OutputArtistNode()
        {

        }

        public OutputArtistNode(ArtistNode artist)
        {
            Name = artist.Name;
            ArtistLabel = artist.ArtistLabel;
            ArtistId = artist.ArtistId;
            WikiPage = string.Empty;
            ManualValidated = false;
            ArtistInCollection = true;
        }
    }
}
