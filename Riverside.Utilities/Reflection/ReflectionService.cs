using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Riverside.Utilities.Reflection
{
    public class ReflectionService : IReflectionService
    {
        /// <summary>
        /// Uses reflection to identify types within assemblies that implement specified interface.
        /// </summary>
        /// <param name="path">The location of assemblies (dlls) to be searched.</param>
        /// <typeparam name="T">The type of interface that returned types must implement.</typeparam>
        /// <returns>List of types that implement interface, T.</returns>
        public List<Type> GetTypes<T>(string path)
        {
            List<Type> types = new List<Type>();
            string[] files = Directory.GetFiles(path, "*.dll");
            foreach (string file in files)
            {
                Assembly a = Assembly.LoadFrom(file);
                Type[] assemblyTypes = a.GetTypes();
                foreach (Type assemblyType in assemblyTypes)
                {
                    if (!assemblyType.IsInterface && assemblyType.GetInterface(typeof(T).Name) != null)
                        types.Add(assemblyType);
                }
            }
            return types;
        }

        /// <summary>
        /// Get file path of the executing assmembly.
        /// Credit: John Sibly http://stackoverflow.com/questions/253468/whats-the-best-way-to-get-the-directory-from-which-an-assembly-is-executing
        /// </summary>
        /// <returns>Location of executing assembly.</returns>
        public string GetExecutingAssemblyPath()
        {
            string codeBase = Assembly.GetExecutingAssembly().GetName().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            path = Path.GetDirectoryName(path);
            return path;
        }
    }
}
