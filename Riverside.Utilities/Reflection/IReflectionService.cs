using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Reflection
{
    public interface IReflectionService
    {
        IEnumerable<Type> GetTypesThatImplementInterface<T>(string assemblyPath);
        IEnumerable<Type> GetTypesThatImplementInterface<T>(string[] assemblyPaths);
        string GetExecutingAssemblyFolderPath();
    }
}
