using Sciendo.Love2Playlist.Processor.DataTypes;

namespace Sciendo.Love2Playlist.Processor
{
    public class LoveProvider : ILoveProvider
    {
        private readonly IUrlProvider _urlProvider;
        private readonly ILastFmProvider _lastFmProvider;

        public LoveProvider(IUrlProvider urlProvider, ILastFmProvider lastFmProvider)
        {
            _urlProvider = urlProvider;
            _lastFmProvider = lastFmProvider;
        }

        public LovePage GetPage(int currentLovedPage)
        {
            var url = _urlProvider.GetUrl(currentLovedPage);
            var rawData = _lastFmProvider.GetLastFmContent(url);
            if (string.IsNullOrEmpty(rawData))
                return new LovePage();

            return Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(rawData).LovePage;
        }
    }
}
