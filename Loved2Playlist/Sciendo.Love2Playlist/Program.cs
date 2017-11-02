﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sciendo.Clementine.DataAccess;
using Sciendo.Common.IO;
using Sciendo.Common.Music.Tagging;
using Sciendo.Love2Playlist.Processor;
using Sciendo.Love2Playlist.Processor.Configuration;
using Sciendo.Playlists.M3U;

namespace Sciendo.Love2Playlist
{
    class Program
    {
        static void Main(string[] args)
        {
            LastFmConfigurationSection lastFmConfig = ConfigurationManager.GetSection("lastFm") as LastFmConfigurationSection;
            PlaylistConfigurationSection playlistConfig = ConfigurationManager.GetSection("playlist") as PlaylistConfigurationSection;

            ILoveProvider loveProvider = new LoveProvider(lastFmConfig,new LastFmProvider());
            using (
                var dataProvider =
                    new DataProvider($"Data Source={playlistConfig.ClementineDatabaseFile};version=3;"))
            {

                ICoordinator coordinator = new Coordinator(loveProvider, dataProvider, new M3UHandler(),
                    new TagFileReader(), new TextFileWriter(), lastFmConfig.User, playlistConfig.FileName);
                coordinator.CollectedLove += Coordinator_CollectedLove;
                coordinator.SavedLove += Coordinator_SavedLove;
                coordinator.SavedPlaylist += Coordinator_SavedPlaylist;
                coordinator.GetLovedAndPersistPlaylist();

            }
        }

        private static void Coordinator_SavedPlaylist(object sender, SavePlaylistEventArgs e)
        {
            Console.WriteLine("PLAYLIST Save: {0} Total till now: {1}",e.ToFile,e.TotalCummulated);
        }

        private static void Coordinator_SavedLove(object sender, SaveLoveEventArgs e)
        {
            Console.WriteLine("LOVE Save: {0} Total till now: {1}", e.ToFile, e.TotalCummulated);
        }

        private static void Coordinator_CollectedLove(object sender, CollectLoveEventArgs e)
        {
            Console.WriteLine("LOVE Collect: {0} out of {1}", e.CurrentPage, e.TotalPages);
        }
    }
}
