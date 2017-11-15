using System;
using Sciendo.Common.MySynchExtensions;

namespace Sciendo.Playlist.Translator
{
    public interface ITranslator:ITextMessageTranslator
    {
        event EventHandler<PathEventArgs> PathTranslated; 
        void Start();

        void Stop();
    }
}
