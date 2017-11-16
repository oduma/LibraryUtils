using System;
using MySynch.Q.Common;


namespace Sciendo.Playlist.Translator
{
    public interface ITranslator:IMessageTranslator
    {
        event EventHandler<PathEventArgs> PathTranslated; 
        void Start();

        void Stop();
    }
}
