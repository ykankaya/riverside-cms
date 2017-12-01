using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Riverside.Utilities.Reflection
{
    public class ReflectionService : IReflectionService
    {
        public IEnumerable<Type> GetTypesThatImplementInterface<T>(string assemblyPath)
        {
            Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
            Type[] assemblyTypes = assembly.GetTypes();
            foreach (Type assemblyType in assemblyTypes)
            {
                if (assemblyType.IsInterface)
                    continue;
                if (assemblyType.GetInterface(typeof(T).Name) == null)
                    continue;
                yield return assemblyType;
            }
        }

        public IEnumerable<Type> GetTypesThatImplementInterface<T>(string[] assemblyPaths)
        {
            IEnumerable<Type> types = Enumerable.Empty<Type>();
            foreach (string assemblyPath in assemblyPaths)
                types = types.Concat(GetTypesThatImplementInterface<T>(assemblyPath));
            return types;
        }

        /// <summary>
        /// Get file path of the executing assmembly.
        /// Credit: John Sibly http://stackoverflow.com/questions/253468/whats-the-best-way-to-get-the-directory-from-which-an-assembly-is-executing
        /// </summary>
        /// <returns>Location of executing assembly.</returns>
        public string GetExecutingAssemblyFolderPath()
        {
            string codeBase = Assembly.GetExecutingAssembly().GetName().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            path = Path.GetDirectoryName(path);
            return path;
        }
    }
}
