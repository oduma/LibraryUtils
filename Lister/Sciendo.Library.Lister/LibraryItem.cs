using System.Xml.Serialization;

namespace Sciendo.Library.Lister
{
    public class LibraryItem
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public ItemType ItemType { get; set; }

        [XmlAttribute]
        public long Size { get; set; }

        public override string ToString()
        {
            return $"{ItemType}\t{Name} - {Size}";
        }
    }
}
