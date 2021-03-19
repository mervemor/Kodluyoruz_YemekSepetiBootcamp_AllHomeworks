using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SingletonScopedTransient.Services.Interfaces
{
    public interface IScopedService
    {
        public int generateNumber { get; }
    }
}
