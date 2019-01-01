using System;

namespace LIE.DataTypes
{
    public class FileWithTags
    {
        public Guid TrackId { get; set; }
        public string FilePath { get; set; }
        public string Artists { get; set; }
        public string Composers { get; set; }
        public string Album { get; set; }
        public string Track { get; set; }
        public string Year { get; set; }
        public string AlbumArtists { get; set; }
        public string Title { get; set; }
    }
}
