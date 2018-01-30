using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Common.Music.Contracts
{
    public interface IPostProcessor
    {
        void Process(string message);
    }
}
