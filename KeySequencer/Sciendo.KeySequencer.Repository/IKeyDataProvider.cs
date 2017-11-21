using System.Collections.Generic;

namespace Sciendo.KeySequencer.Repository
{
    public interface IKeyDataProvider
    {
        IEnumerable<string> GetAllKeys();

        IEnumerable<SimilarKey> GetAllKeySequencesFrom(string key);
    }
}
