using System.IO;
using System.Xml.Serialization;
using Sciendo.Common.IO;
using TagLib;

namespace Sciendo.Playlists.XSPF
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

        public Track()
        {
            
        }

        public Track(IFileReader<TagLib.File> tagFileReader, string file, string rootFolderPath)
        {
            var filePath = (string.IsNullOrEmpty(rootFolderPath))
                ? file
                : $"{rootFolderPath}{Path.DirectorySeparatorChar}{file}";
            var tagFile = tagFileReader.Read(filePath);
            var tag = tagFile.Tag;
            Duration = tagFile.Properties.Duration.TotalMilliseconds;
            Album = tag.Album;
            Creator = tag.FirstPerformer;
            Title = tag.Title;
            Location = "file:///" + file.Replace(@"\","/");
        }
    }
}