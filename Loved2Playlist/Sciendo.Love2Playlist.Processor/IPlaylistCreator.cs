using System;
using System.Collections.Generic;
using Sciendo.Love2Playlist.Processor.DataTypes;

namespace Sciendo.Love2Playlist.Processor
{
    public interface IPlaylistCreator
    {
        PartPlaylist AddToPlaylist(IList<LoveTrack> loveTracks);
    }
}