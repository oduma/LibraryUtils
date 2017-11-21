using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sciendo.Common.Serialization;

namespace Sciendo.KeySequencer.Repository
{
    public class KeyDataProvider:IKeyDataProvider
    {
        private readonly KeySequenceRepository _keySequenceRepository;
        private const string KeySequenceFileName="KeySequences.xml";

        public KeyDataProvider()
        {
            _keySequenceRepository =
                Serializer.DeserializeOneFromFile<KeySequenceRepository>(
                    Path.Combine($"{AppDomain.CurrentDomain.BaseDirectory}Data",
                        KeySequenceFileName));
        }
        public IEnumerable<string> GetAllKeys()
        {
            return _keySequenceRepository.KeySequences.Select(k => k.Key).Distinct().OrderBy(k=>k);
        }

        public IEnumerable<SimilarKey> GetAllKeySequencesFrom(string key)
        {
            return _keySequenceRepository.KeySequences.Where(
                k => string.Equals(k.Key, key, StringComparison.CurrentCultureIgnoreCase)).Select(k => k.SimilarKey).OrderBy(k=>(int)k.Similarity);
        }
    }
}
