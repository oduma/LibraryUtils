using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Mixx.DataAccess
{
    public interface IMap<T, T1>
    {
        T1 Transform(T fromDataType);

        T Transform(T1 fromDataType);
    }
}
