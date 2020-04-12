using AC2.Configuration;
using AC2.DataTypes;
using AC2.IoAccess;
using AC2.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sciendo.ArtistClassifier.NLP.NER.Contracts;
using Sciendo.ArtistClassifier.NLP.NER.Logic;
using Sciendo.Config;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AC2
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            var serviceProvider = ConfigureLog(serviceCollection);
            var logger = serviceProvider.GetService<ILogger<Program>>();
            var artistClassifierPass2Configuration = ReadConfiguration(logger, args);


            logger.LogInformation("Loading tags from file: {0}...", artistClassifierPass2Configuration.AllArtistsInputFile);
            var allArtists = IoManager.ReadWithMapper<ArtistNode,ArtistNodeMap>(artistClassifierPass2Configuration.AllArtistsInputFile);
            logger.LogInformation("Loaded {0} artists.", allArtists.Count());
            var allArtistsProcessed = ProcessAllArtists(ConfigureServices(serviceCollection, artistClassifierPass2Configuration), allArtists);
            IoManager.WriteWithMapper<ArtistNode, ArtistNodeMap>(allArtistsProcessed.ToList(), artistClassifierPass2Configuration.AllArtistsOutputFile);
            logger.LogInformation("Processing done!");

        }

        private static IEnumerable<ArtistNode> ProcessAllArtists(ServiceProvider serviceProvider, 
            IEnumerable<ArtistNode> allArtists)
        {
            var artistClassifier = serviceProvider.GetService<IArtistClassifier>();
            foreach(var artist in allArtists)
            {
                if(artist.ArtistLabel == "Artist")
                    artist.ArtistLabel = artistClassifier.Classify(artist.Name).ArtistType.ToString();
                yield return artist;
            }
        }

        private static ServiceProvider ConfigureServices(ServiceCollection serviceCollection, ArtistClassifierPass2Configuration artistClassifierPass2Configuration)
        {

            serviceCollection.AddSingleton<IArtistClassifier>(r => new ArtistClassifier(artistClassifierPass2Configuration.NlpConfig));
            return serviceCollection.BuildServiceProvider();
        }

        private static ArtistClassifierPass2Configuration ReadConfiguration(ILogger<Program> logger, string[] args)
        {
            try
            {
                return new ConfigurationManager<ArtistClassifierPass2Configuration>().GetConfiguration(new ConfigurationBuilder()
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

        private static ServiceProvider ConfigureLog(ServiceCollection services)
        {
            return services.AddLogging(configure => configure.AddSerilog(new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger())).BuildServiceProvider();
        }

    }
}
