using System.Xml.Serialization;

namespace Sciendo.KeySequencer.Repository
{
    [XmlRoot]
    public class KeySequenceItem
    {
        [XmlAttribute(AttributeName = "name")]
        public string Key { get; set; }

        [XmlElement(ElementName = "toKey")]
        public SimilarKey SimilarKey { get; set; }
    }
}