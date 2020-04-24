using AE.Configuration;
using AE.DataTypes;
using AE.IoAccess;
using AE.Mappers;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sciendo.ArtistClassifier.NLP.NER.Contracts;
using Sciendo.ArtistClassifier.NLP.NER.Logic;
using Sciendo.ArtistEnhancer.Contracts;
using Sciendo.ArtistEnhancer.Contracts.DataTypes;
using Sciendo.ArtistEnhancer.KnowledgeBaseProvider;
using Sciendo.ArtistEnhancer.Logic;
using Sciendo.Config;
using Sciendo.Web;
using Sciendo.Wiki.Search.Contracts;
using Sciendo.Wiki.Search.Logic;
using Sciendo.Wiki.Search.Logic.UrlProviders;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AE
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            var serviceProvider = ConfigureLog(serviceCollection);
            var logger = serviceProvider.GetService<ILogger<Program>>();
            var artistEnhancerConfiguration = ReadConfiguration(logger, args);
            logger.LogInformation("Loading artists from file: {0}...", artistEnhancerConfiguration.AllArtistsInputFile);
            var allArtists = IoManager.ReadWithMapper<ArtistNode, ArtistNodeMap>(artistEnhancerConfiguration.AllArtistsInputFile);
            logger.LogInformation("Loaded {0} artists.", allArtists.Count());
            ProcessAllBands(logger,ConfigureServices(serviceCollection, artistEnhancerConfiguration), allArtists, artistEnhancerConfiguration);
            Save(logger,artistEnhancerConfiguration);


        }

        private static void Save(ILogger<Program> logger, ArtistEnhancerConfiguration artistEnhancerConfiguration)
        {
            var fileTrackers = GetFileTrackers(artistEnhancerConfiguration);
            var outputArtists = GetAllItems<OutputArtistNode,OutputArtistNodeMap>(fileTrackers, artistEnhancerConfiguration.AllArtistsOutputFile);
            var bandsWithMembers = GetAllItems<RelationBandArtist,RelationBandArtistMap>(fileTrackers, artistEnhancerConfiguration.RelationshipBandArtistFile);
            var bandsWithoutMembers = GetAllItems<ArtistNode, ArtistNodeMap>(fileTrackers, artistEnhancerConfiguration.BandsWithoutMembersFile);

            IoManager.WriteWithMapper<OutputArtistNode, OutputArtistNodeMap>(outputArtists, artistEnhancerConfiguration.AllArtistsOutputFile);
            IoManager.WriteWithMapper<RelationBandArtist, RelationBandArtistMap>(bandsWithMembers, artistEnhancerConfiguration.RelationshipBandArtistFile);
            IoManager.WriteWithMapper<ArtistNode, ArtistNodeMap>(bandsWithoutMembers, artistEnhancerConfiguration.BandsWithoutMembersFile);

            logger.LogInformation("Total write {0} Artists.", outputArtists.Count);
            logger.LogInformation("Total write {0} Bands Artists relations.", bandsWithMembers.Count);
            logger.LogInformation("Total write {0} Bands without members.", bandsWithoutMembers.Count);


        }

        private static List<T> GetAllItems<T,TMap>(string[] fileTrackers, string file) where T: class where TMap:ClassMap
        {
            var result = new List<T>();
            foreach(var fileTracker in fileTrackers)
            {
                result.AddRange(IoManager.ReadWithMapper<T, TMap>(file.Replace(".csv", $".{fileTracker}.csv")));
            }
            return result;
        }

        private static string[] GetFileTrackers(ArtistEnhancerConfiguration artistEnhancerConfiguration)
        {
            var fileNames = Directory.GetFiles(Path.GetDirectoryName(artistEnhancerConfiguration.AllArtistsOutputFile), Path.GetFileName(artistEnhancerConfiguration.AllArtistsOutputFile).Replace(".csv", ".???.csv"))
                .OrderBy(s => s);
            return fileNames.Select(f => f.Split(new[] { '.' })[1]).ToArray();
        }

        private static void ProcessAllBands(ILogger<Program> logger, ServiceProvider serviceProvider, List<ArtistNode> allArtists, ArtistEnhancerConfiguration artistEnhancerConfiguration)
        {
            var bandEnhancerService = serviceProvider.GetService<IBandEnhancer>();
            List<OutputArtistNode> outputArtists = new List<OutputArtistNode>();
            var bandsWithMembers = new List<RelationBandArtist>();
            var bandsWithoutMembers = new List<ArtistNode>();
            int fileTracker = DetermineStartingFileTracker(artistEnhancerConfiguration);
            int artistCounter = 1;
            var nextArtist = GetNextArtist(artistEnhancerConfiguration);
            bool beginFound = string.IsNullOrEmpty(nextArtist);
            string lastArtist = string.Empty;
            foreach(var artist in allArtists)
            {
                if(!beginFound && artist.Name!=nextArtist)
                {
                    continue;
                }
                else
                {
                    beginFound = true;
                }
                if(artistCounter%artistEnhancerConfiguration.WriteBatchSize==0)
                {
                    TemporaryWrite(outputArtists, bandsWithMembers, bandsWithoutMembers, fileTracker++, artistEnhancerConfiguration, artist.Name);
                    logger.LogInformation("Temporary write {0} Artists.", outputArtists.Count);
                    logger.LogInformation("Temporary write {0} Bands Artists relations.", bandsWithMembers.Count);
                    logger.LogInformation("Temporary write {0} Bands without members.", bandsWithoutMembers.Count);

                    outputArtists = new List<OutputArtistNode>();
                    bandsWithMembers = new List<RelationBandArtist>();
                    bandsWithoutMembers = new List<ArtistNode>();
                }
                artistCounter++;
                if (artist.ArtistLabel == "Artist")
                    outputArtists.Add(new OutputArtistNode(artist));
                else
                {
                    var bandWithInfo = bandEnhancerService.FindBandInWikipedia(artist.Name);
                    if(bandWithInfo.PageId==0 || string.IsNullOrEmpty(bandWithInfo.Language))
                    {
                        outputArtists.Add(new OutputArtistNode(artist));
                        bandsWithoutMembers.Add(artist);
                    }
                    else if(bandWithInfo.Members==null || !bandWithInfo.Members.Any())
                    {
                        outputArtists.Add(MapBandWithInfoToOutputArtist(artistEnhancerConfiguration, artist, bandWithInfo));
                        bandsWithoutMembers.Add(artist);
                    }
                    else
                    {
                        outputArtists.Add(MapBandWithInfoToOutputArtist(artistEnhancerConfiguration, artist, bandWithInfo));
                        var members = MapMembersToOutputArtists(allArtists, outputArtists, bandWithInfo.Members);
                        outputArtists.AddRange(members);
                        bandsWithMembers.AddRange(CreateRelationsBetweenBandAndMembers(artist.ArtistId,members));
                    }
                }
                lastArtist = artist.Name;
            }
            TemporaryWrite(outputArtists, bandsWithMembers, bandsWithoutMembers, fileTracker++, artistEnhancerConfiguration, lastArtist);
            logger.LogInformation("Temporary write {0} Artists.", outputArtists.Count);
            logger.LogInformation("Temporary write {0} Bands Artists relations.", bandsWithMembers.Count);
            logger.LogInformation("Temporary write {0} Bands without members.", bandsWithoutMembers.Count);

        }

        private static string GetNextArtist(ArtistEnhancerConfiguration artistEnhancerConfiguration)
        {
            if (!File.Exists(artistEnhancerConfiguration.NextArtistFile))
                return string.Empty;
            return File.ReadAllText(artistEnhancerConfiguration.NextArtistFile);
        }

        private static void TemporaryWrite(List<OutputArtistNode> outputArtists, List<RelationBandArtist> bandsWithMembers, List<ArtistNode> bandsWithoutMembers, int fileTracker, ArtistEnhancerConfiguration artistEnhancerConfiguration, string nextArtist)
        {

            var fileTrackerSequence = fileTracker.ToString().PadLeft(3, '0');

            IoManager.WriteWithMapper<OutputArtistNode, OutputArtistNodeMap>(outputArtists, artistEnhancerConfiguration.AllArtistsOutputFile.Replace(".csv",$".{fileTrackerSequence}.csv"));
            IoManager.WriteWithMapper<RelationBandArtist, RelationBandArtistMap>(bandsWithMembers, artistEnhancerConfiguration.RelationshipBandArtistFile.Replace(".csv", $".{fileTrackerSequence}.csv"));
            IoManager.WriteWithMapper<ArtistNode, ArtistNodeMap>(bandsWithoutMembers, artistEnhancerConfiguration.BandsWithoutMembersFile.Replace(".csv", $".{fileTrackerSequence}.csv"));
            if (!string.IsNullOrEmpty(nextArtist))
                File.WriteAllText(artistEnhancerConfiguration.NextArtistFile, nextArtist);
            else
                File.Delete(artistEnhancerConfiguration.NextArtistFile);
        }

        private static int DetermineStartingFileTracker(ArtistEnhancerConfiguration artistEnhancerConfiguration)
        {
            var temporaryOutputFile = Directory.GetFiles(Path.GetDirectoryName(artistEnhancerConfiguration.AllArtistsOutputFile), Path.GetFileName(artistEnhancerConfiguration.AllArtistsOutputFile).Replace(".csv", ".???.csv"))
                .OrderBy(s=>s).LastOrDefault();
            if (string.IsNullOrEmpty(temporaryOutputFile))
                return 0;
            var fileParts = temporaryOutputFile.Split(new[] { '.' });
            var currentMax = Convert.ToInt32(fileParts[1]);
            return currentMax + 1;

        }

        private static IEnumerable<RelationBandArtist> CreateRelationsBetweenBandAndMembers(Guid artistId, IEnumerable<OutputArtistNode> members)
        {
            foreach(var member in members)
            {
                yield return new RelationBandArtist { BandId = artistId, ArtistId = member.ArtistId };
            }
        }

        private static IEnumerable<OutputArtistNode> MapMembersToOutputArtists(List<ArtistNode> allArtists, List<OutputArtistNode> outputArtists, List<string> members)
        {
            foreach (var member in members)
            {
                var outputArtist = new OutputArtistNode();
                var existingArtist = allArtists.FirstOrDefault(a => a.Name == member.ToLower());
                if (existingArtist == null)
                    existingArtist = outputArtists.FirstOrDefault(a => a.Name == member.ToLower());
                if (existingArtist == null)
                {
                    yield return new OutputArtistNode 
                    {
                        ArtistId = Guid.NewGuid(),
                        ArtistLabel = "Artist",
                        Name = member
                    };
                }
                else
                {
                    yield return new OutputArtistNode(existingArtist);
                }
            }
        }

        private static OutputArtistNode MapBandWithInfoToOutputArtist(ArtistEnhancerConfiguration artistEnhancerConfiguration, ArtistNode artist, BandWikiPageInfo bandWithInfo)
        {
            var outputArtist = new OutputArtistNode(artist);
            outputArtist.WikiPage = string.Format(artistEnhancerConfiguration.WikiSearchConfig.WikiPageGetTemplateUrl, bandWithInfo.Language, bandWithInfo.PageId);
            return outputArtist;
        }

        public delegate IUrlProvider UrlProviderResolver(string key);

        private static ServiceProvider ConfigureServices(ServiceCollection serviceCollection, ArtistEnhancerConfiguration artistEnhancerConfiguration)
        {

            serviceCollection.AddTransient<SearchUrlProvider>(r => new SearchUrlProvider(artistEnhancerConfiguration.WikiSearchConfig));
            serviceCollection.AddTransient<PageUrlProvider>(r => new PageUrlProvider(artistEnhancerConfiguration.WikiSearchConfig));
            serviceCollection.AddTransient<UrlProviderResolver>(serviceProvider => key =>
            {
                switch (key)
                {
                    case "SEARCH":
                        return serviceProvider.GetService<SearchUrlProvider>();
                    case "PAGE":
                        return serviceProvider.GetService<PageUrlProvider>();
                    default:
                        throw new KeyNotFoundException();
                }
            });
            serviceCollection.AddTransient<IWebGet<string>>(r => new WebStringGet(r.GetRequiredService<ILogger<WebStringGet>>()));
            serviceCollection.AddTransient<IWiki>(r => new Wiki(r.GetService<UrlProviderResolver>()("SEARCH"), r.GetService<UrlProviderResolver>()("PAGE"), r.GetRequiredService<IWebGet<string>>(), artistEnhancerConfiguration.WikiSearchConfig));
            serviceCollection.AddSingleton<IPersonsNameFinder>(r => new PersonsNameFinder(artistEnhancerConfiguration.NlpConfig));
            serviceCollection.AddTransient<IKnowledgeBaseFactory, KnowledgeBaseFactory>();
            serviceCollection.AddTransient<IBandEnhancer>(r => new BandEnhancer(r.GetRequiredService<ILogger<BandEnhancer>>(), r.GetRequiredService<IKnowledgeBaseFactory>(), artistEnhancerConfiguration.KnowledgeBaseFile, r.GetRequiredService<IWiki>(), r.GetRequiredService<IPersonsNameFinder>()));
            return serviceCollection.BuildServiceProvider();
        }


        private static ServiceProvider ConfigureLog(ServiceCollection services)
        {
            return services.AddLogging(configure => configure.AddSerilog(new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger())).BuildServiceProvider();
        }

        private static ArtistEnhancerConfiguration ReadConfiguration(ILogger<Program> logger, string[] args)
        {
            try
            {
                return new ConfigurationManager<ArtistEnhancerConfiguration>().GetConfiguration(new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"{AppDomain.CurrentDomain.FriendlyName}.json")
                    .AddCommandLine(args)
                    .Build());
            }
            catch (Exception e)
            {
                logger.LogError(e, "wrong config!");
                throw e;
            }
        }

    }
}
