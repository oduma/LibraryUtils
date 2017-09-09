using System;
using System.Collections.Generic;
using Sciendo.Love2Playlist.Processor.DataTypes;

namespace Sciendo.Love2Playlist.Processor
{
    public interface IPlaylistCreator
    {
        string AddToPlaylist(IList<LoveTrack> loveTracks);
    }
}