using System;
using System.Collections.Generic;

namespace Sciendo.Playlist.Translator
{
    public interface IBulkTranslator
    {
        event EventHandler<PathEventArgs> PathTranslated; 
        void Start(Dictionary<string, string> findReplaceParams);

        void Stop();
    }
}
