using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Internal;
using NUnit.Framework;
using Rhino.Mocks;
using Sciendo.Common.IO;
using Sciendo.Mixx.DataAccess;
using Sciendo.Playlist.Mixx.Processor;

namespace Sciendo.PMP.Tests.Unit
{
    [TestFixture]
    public class PostProcessorTests
    {
        [Test]
        public void PostProcessorWithNonStringParameter()
        {
            MixxxPushProcessor processor = new MixxxPushProcessor(new DataHandler(), new FSFile());
            Assert.False(processor.Process((object) new byte[] {1, 2, 3, 4}, "somename"));
        }

    }
}
