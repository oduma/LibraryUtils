using System;
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
