using System;
using System.IO;
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
            KeySequenceRepository keySequenceRepository = new KeySequenceRepository
            {
                Version = "1.0",
                KeySequences = new KeySequenceItem[2]
            };
            keySequenceRepository.KeySequences[0] = new KeySequenceItem
            {
                Key = "A",
                SimilarKey = new SimilarKey
                {
                    Key = "B",
                    Similarity = Similarity.Super
                }
            };

            keySequenceRepository.KeySequences[1] = new KeySequenceItem
            {
                Key = "A",
                SimilarKey = new SimilarKey
                {
                    Key = "C",
                    Similarity = Similarity.High
                }
            };

            Serializer.SerializeOneToFile(keySequenceRepository,Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"ttt.xml"));
        }
    }
}
