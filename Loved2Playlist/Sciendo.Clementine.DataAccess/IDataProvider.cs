using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sciendo.Clementine.DataAccess.DataTypes;

namespace Sciendo.Clementine.DataAccess
{
    public interface IDataProvider: IDisposable
    {
        void Load();

        DateTime LastRefresh { get; }

        ClementineTrack[] AllTracks { get; }
    }
}
