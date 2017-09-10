using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Love2Playlist.Processor
{
    public class LastFmUrlProvider:IUrlProvider
    {
        private readonly string _root;
        private readonly string _appKey;

        public LastFmUrlProvider(string root, string appKey)
        {
            _root = root;
            _appKey = appKey;
        }
        public Uri GetUrl(int pageNumber)
        {
            return new Uri($"{_root}{_appKey}{pageNumber}");
        }
    }
}
