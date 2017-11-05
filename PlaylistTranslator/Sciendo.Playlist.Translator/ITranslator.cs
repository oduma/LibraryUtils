using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Playlist.Translator
{
    public interface ITranslator
    {
        event EventHandler<PathEventArgs> PathTranslated; 
        void Start();

        void Stop();
    }
}
