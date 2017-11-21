using System.Xml.Serialization;

namespace Sciendo.Playlists.XSPF
{
    [XmlRoot(ElementName = "playlist", Namespace= "http://xspf.org/ns/0/")]
    public class Playlist
    {
        [XmlAttribute(AttributeName = "version")]
        public int Version { get; set; }

        [XmlArray(ElementName="trackList")]
        [XmlArrayItem(ElementName = "track")]
        public Track[] Tracklist { get; set; }
    }
}
