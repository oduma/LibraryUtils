using System;
using System.IO;
using NUnit.Framework;
using Sciendo.Common.Serialization;

namespace Sciendo.PlaylistsTests.Unit
{
    [TestFixture]
    public class DeserializationTests
    {
        [Test]
        public void DeserializingOk()
        {
            var playlistContent = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}\\Data\\80s.xspf");
            var playlist = Serializer.Deserialize<Playlists.XSPF.Playlist>(playlistContent);
            Assert.IsNotNull(playlist);

        }

        [Test]
        public void SerializeOk()
        {
            Playlists.XSPF.Playlist playlist= new Playlists.XSPF.Playlist();
            playlist.Version = 1;

            var playlistContent = Serializer.Serialize(playlist);

            Assert.IsNotNull(playlistContent);
        }
    }
}
