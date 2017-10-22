using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Injection
{
    public interface IInjectionService
    {
        T CreateType<T>(Type type);
    }
}
