using System;
using System.Collections.Generic;
using Sciendo.Mixx.DataAccess;
using Sciendo.Mixx.DataAccess.Domain;
using Sciendo.Playlists;

namespace Sciendo.Playlist.Mixx.Processor
{
    public interface IMixxxProcessor
    {
        void Start(string playlistFileName, IMap<IEnumerable<PlaylistItem>,IEnumerable<MixxxPlaylistTrack>> mapper);

        void Stop();

        event EventHandler<MixxxProcessorProgressEventHandler> MixxxPlaylistDeleted;

        event EventHandler<MixxxProcessorProgressEventHandler> MixxxPlaylistCreated;
    }
}
