using System;
using System.Collections.Generic;

namespace Sciendo.Playlist.Translator
{
    public abstract class TranslatorBase
    {
        protected Dictionary<string, string> FindReplaceParams { get; private set; }

        protected static string PerformOneTranslation(Dictionary<string, string> replacementVariants, string contents)
        {
            foreach (var fromReplacement in replacementVariants.Keys)
            {
                contents = contents.Replace(fromReplacement, replacementVariants[fromReplacement]);
            }
            return contents;
        }


        protected TranslatorBase(Dictionary<string, string> findReplaceParams)
        {
            if (findReplaceParams == null || findReplaceParams.Count < 1)
                throw new ArgumentNullException(nameof(findReplaceParams));
            FindReplaceParams = findReplaceParams;
        }
    }
}