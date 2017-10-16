﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sciendo.Clementine.DataAccess;
using Sciendo.Love2Playlist.Processor;
using Sciendo.Love2Playlist.Processor.Configuration;

namespace Sciendo.Love2Playlist
{
    class Program
    {
        static void Main(string[] args)
        {
            LastFmConfigurationSection lastFmConfig = ConfigurationManager.GetSection("lastFm") as LastFmConfigurationSection;
            PlaylistConfigurationSection playlistConfig = ConfigurationManager.GetSection("playlist") as PlaylistConfigurationSection;

            ILoveProvider loveProvider = new LoveProvider(lastFmConfig,new LastFmProvider());
            IPersister persister = new Persister(playlistConfig.FileName,lastFmConfig.User);
            using (
                var dataProvider =
                    new DataProvider($"Data Source={playlistConfig.ClementineDatabaseFile};version=3;"))
            {
                IPlaylistCreator playlistCreator = new PlaylistCreator(dataProvider);

                ICoordinator coordinator = new Coordinator(loveProvider, persister, playlistCreator);
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