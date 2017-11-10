using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Internal;
using NUnit.Framework;
using Sciendo.Common.Serialization;

namespace Sciendo.KeySequencer.Repository.Tests.Unit
{
    [TestFixture]
    public class SerializationTest
    {
        [Test]
        public void SerializeASequence()
        {
            KeySequenceRepository keySequenceRepository= new KeySequenceRepository();
            keySequenceRepository.Version = "1.0";
            keySequenceRepository.KeySequences= new KeySequenceItem[2];
            keySequenceRepository.KeySequences[0]= new KeySequenceItem();
            keySequenceRepository.KeySequences[0].Key = "A";
            keySequenceRepository.KeySequences[0].SimilarKey=new SimilarKey();
            keySequenceRepository.KeySequences[0].SimilarKey.Key = "B";
            keySequenceRepository.KeySequences[0].SimilarKey.Similarity=Similarity.Super;

            keySequenceRepository.KeySequences[1] = new KeySequenceItem();
            keySequenceRepository.KeySequences[1].Key = "A";
            keySequenceRepository.KeySequences[1].SimilarKey = new SimilarKey();
            keySequenceRepository.KeySequences[1].SimilarKey.Key = "C";
            keySequenceRepository.KeySequences[1].SimilarKey.Similarity = Similarity.High;

            Serializer.SerializeOneToFile(keySequenceRepository,Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"ttt.xml"));
        }
    }
}
