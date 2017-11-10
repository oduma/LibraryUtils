using System.Xml.Serialization;

namespace Sciendo.KeySequencer.Repository
{
    [XmlRoot]
    public class SimilarKey
    {
        [XmlAttribute(AttributeName = "name")]
        public string Key { get; set; }

        [XmlAttribute(AttributeName = "similarity")]
        public Similarity Similarity { get; set; }

    }
}