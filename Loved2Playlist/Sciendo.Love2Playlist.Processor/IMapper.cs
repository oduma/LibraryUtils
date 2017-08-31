using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Love2Playlist.Processor
{
    public interface IMapper<in TInput, out TOutput> 
    {
        TOutput Map(TInput page);
    }
}
