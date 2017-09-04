using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Sciendo.Love2Playlist.Processor.DataTypes;
using Newtonsoft.Json;

namespace Sciendo.Love2Playlist.Processor
{
    public class LoveProvider: ILoveProvider
    {
        private readonly IUrlProvider _urlProvider;

        public LoveProvider(IUrlProvider urlProvider)
        {
            _urlProvider = urlProvider;
        }
        public LovePage GetPage(int currentLovedPage)
        {
            var url= _urlProvider.GetUrl(currentLovedPage);
            return GetLovePage(url);
        }

        private LovePage GetLovePage(Uri url)
        {
            var httpClient = new HttpClient();
            try
            {
                using (var getTask = httpClient.GetStringAsync(url)
                    .ContinueWith(p => p).Result)
                {
                    if (getTask.Status == TaskStatus.RanToCompletion || !string.IsNullOrEmpty(getTask.Result))
                    {
                        return JsonConvert.DeserializeObject<LovePage>(getTask.Result);
                    }
                    return new LovePage();
                }

            }
            catch
            {
                return new LovePage();
            }

        }

    }
}
