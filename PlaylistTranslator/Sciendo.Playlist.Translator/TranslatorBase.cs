using System.Collections.Generic;

namespace Sciendo.Playlist.Translator
{
    public abstract class TranslatorBase
    {
        protected static string PerformOneTranslation(Dictionary<string, string> replacementVariants, string contents)
        {
            foreach (var fromReplacement in replacementVariants.Keys)
            {
                contents = contents.Replace(fromReplacement, replacementVariants[fromReplacement]);
            }
            return contents;
        }
    }
}