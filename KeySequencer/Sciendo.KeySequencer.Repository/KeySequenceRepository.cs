using System.Xml.Serialization;

namespace Sciendo.KeySequencer.Repository
{
    [XmlRoot]
    public class KeySequenceRepository
    {
        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }
        [XmlArray(ElementName = "KeySequences")]
        [XmlArrayItem(ElementName="fromKey")]
        public KeySequenceItem[] KeySequences { get; set; }
    }
}
