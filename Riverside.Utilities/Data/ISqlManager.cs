using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Data
{
    public interface ISqlManager
    {
        string GetSql(string name);
    }
}
