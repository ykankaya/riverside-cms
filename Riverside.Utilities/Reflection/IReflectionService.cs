using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Reflection
{
    public interface IReflectionService
    {
        List<Type> GetTypes<T>(string path);
        string GetExecutingAssemblyPath();
    }
}
