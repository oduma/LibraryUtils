using AE.Configuration;
using AE.DataTypes;
using AE.IoAccess;
using AE.Mappers;
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
            List<RelationBandArtist> bandsWithMembers = new List<RelationBandArtist>();
            List<ArtistNode> bandsWithoutMembers = new List<ArtistNode>();
            var allOutputArtists = ProcessAllBands(ConfigureServices(serviceCollection, artistEnhancerConfiguration), allArtists, artistEnhancerConfiguration, out bandsWithMembers, out bandsWithoutMembers);
            logger.LogInformation("{0} artists for writing", allArtists.Count);
            IoManager.WriteWithMapper<OutputArtistNode, OutputArtistNodeMap>(allOutputArtists, artistEnhancerConfiguration.AllArtistsOutputFile);
            IoManager.WriteWithMapper<RelationBandArtist, RelationBandArtistMap>(bandsWithMembers, artistEnhancerConfiguration.RelationshipBandArtistFile);
            IoManager.WriteWithMapper<ArtistNode, ArtistNodeMap>(bandsWithoutMembers, artistEnhancerConfiguration.BandsWithoutMembersFile);


        }

        private static List<OutputArtistNode> ProcessAllBands(ServiceProvider serviceProvider, List<ArtistNode> allArtists, ArtistEnhancerConfiguration artistEnhancerConfiguration, out List<RelationBandArtist> bandsWithMembers, out List<ArtistNode> bandsWithoutMembers)
        {
            var bandEnhancerService = serviceProvider.GetService<IBandEnhancer>();
            List<OutputArtistNode> outputArtists = new List<OutputArtistNode>();
            bandsWithMembers = new List<RelationBandArtist>();
            bandsWithoutMembers = new List<ArtistNode>();
            foreach(var artist in allArtists)
            {
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
            }
            return outputArtists;
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
