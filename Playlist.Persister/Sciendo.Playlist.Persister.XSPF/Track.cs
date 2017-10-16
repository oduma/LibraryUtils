using System.Xml.Serialization;

namespace Sciendo.Playlist.Handler.XSPF
{

    [XmlRoot("track")]
    public class Track
    {
        [XmlElement("location")]
        public string Location { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("creator")]
        public string Creator { get; set; }

        [XmlElement("album")]
        public string Album { get; set; }

        [XmlElement("duration")]
        public double Duration { get; set; }

        [XmlElement("image")]
        public string Image { get; set; }
    }
}