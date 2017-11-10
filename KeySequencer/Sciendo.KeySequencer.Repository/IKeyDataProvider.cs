using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.KeySequencer.Repository
{
    public interface IKeyDataProvider
    {
        IEnumerable<string> GetAllKeys();

        IEnumerable<SimilarKey> GetAllKeySequencesFrom(string key);
    }
}
