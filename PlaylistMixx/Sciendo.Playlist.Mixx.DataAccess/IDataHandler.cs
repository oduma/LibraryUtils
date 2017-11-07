using System;
using System.Collections.Generic;
using Sciendo.Playlists;

namespace Sciendo.Mixx.DataAccess
{
    public interface IDataHandler:IDisposable
    {
        bool Create(string name, IEnumerable<PlaylistItem>  playlistItems);

        IEnumerable<PlaylistItem> Get(string name);

        bool Delete(string name);
    }
}
