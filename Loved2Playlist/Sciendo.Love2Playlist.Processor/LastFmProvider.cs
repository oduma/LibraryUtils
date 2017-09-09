using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Sciendo.Love2Playlist.Processor.DataTypes;

namespace Sciendo.Love2Playlist.Processor
{
    public class LastFmProvider:ILastFmProvider
    {
        public string GetLastFmContent(Uri lastFmUri)
        {
            var httpClient = new HttpClient();
            try
            {
                using (var getTask = httpClient.GetStringAsync(lastFmUri)
                    .ContinueWith(p => p).Result)
                {
                    if (getTask.Status == TaskStatus.RanToCompletion || !string.IsNullOrEmpty(getTask.Result))
                    {
                        return getTask.Result;
                    }
                    return string.Empty;
                }

            }
            catch
            {
                return string.Empty;
            }

        }
    }
}
