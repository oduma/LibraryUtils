using System.Collections.Generic;

namespace Sciendo.Playlist.Translator
{
    public class Translator:TranslatorBase,ITranslator
    {
        public Translator(Dictionary<string, string> findReplaceParams) : base(findReplaceParams)
        {
        }

        public string Translate(string inMessage)
        {
            if (string.IsNullOrEmpty(inMessage))
                return inMessage;
            return PerformOneTranslation(FindReplaceParams, inMessage);
        }

    }
}
