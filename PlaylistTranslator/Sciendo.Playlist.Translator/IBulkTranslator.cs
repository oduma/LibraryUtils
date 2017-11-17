using System;

namespace Sciendo.Playlist.Translator
{
    public interface IBulkTranslator
    {
        event EventHandler<PathEventArgs> PathTranslated; 
        void Start();

        void Stop();
    }
}
