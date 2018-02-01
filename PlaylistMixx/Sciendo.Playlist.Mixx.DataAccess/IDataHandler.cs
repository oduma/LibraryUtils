using System;
using System.Collections.Generic;
using Sciendo.Mixx.DataAccess.Domain;
using Sciendo.Playlists;

namespace Sciendo.Mixx.DataAccess
{
    public interface IDataHandler:IDisposable
    {
        bool Create(string name, IEnumerable<PlaylistItem>  playlistItems, IMap<IEnumerable<PlaylistItem>, IEnumerable<MixxxPlaylistTrack>> mapper);

        IEnumerable<PlaylistItem> Get(string name, IMap<IEnumerable<PlaylistItem>, IEnumerable<MixxxPlaylistTrack>> mapper);

        bool Delete(string name);
    }
}
