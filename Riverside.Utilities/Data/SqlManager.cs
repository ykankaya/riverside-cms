using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Riverside.Utilities.Data
{
    public class SqlManager : ISqlManager
    {
        public string GetSql(string name)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(assembly.GetName().Name + "." + name))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
