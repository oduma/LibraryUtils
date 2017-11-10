using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.KS.MVVM
{
    internal static class ExtensionMethods
    {
        internal static ObservableCollection<string> AddKeys(this ObservableCollection<string> inCollection, IEnumerable<string> fromKeys)
        {
            foreach (var fromKey in fromKeys)
            {
                inCollection.Add(fromKey);
            }
            return inCollection;
        }
    }
}
