using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProvaPub.Contract
{
    public interface IRandomServiceContract
    {
        Task<int> GetRandom();
    }
}