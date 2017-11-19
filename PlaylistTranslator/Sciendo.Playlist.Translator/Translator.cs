using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Sciendo.Playlist.Translator.Configuration;

namespace Sciendo.Playlist.Translator
{
    public class Translator:TranslatorBase,ITranslator
    {

        public string Translate(string inMessage)
        {
            var findReplaceParams = GetParamsFromConfigFile();
            if (findReplaceParams == null || findReplaceParams.Keys.Count < 1)
                return inMessage;
            if (string.IsNullOrEmpty(inMessage))
                return inMessage;
            return PerformOneTranslation(findReplaceParams, inMessage);
        }

        private Dictionary<string, string> GetParamsFromConfigFile()
        {
            return GetSortedParams(((FindAndReplaceConfigSection)ConfigurationManager.GetSection("playlistTranslatorSection"))
            .FromToParams
            .Cast<FromToParamsElement>().Select(e => e).OrderBy(e => e.Priority));
        }

        private static Dictionary<string, string> GetSortedParams(IOrderedEnumerable<FromToParamsElement> fromToParams)
        {
            var result = new Dictionary<string, string>();
            foreach (var fromToParam in fromToParams)
            {
                result.Add(fromToParam.From, fromToParam.To);
            }
            return result;
        }

    }
}
