using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Riverside.Cms.Core.Authentication
{
    public class UserRoleCollection : List<string>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sdr = new SqlDataRecord(
                new SqlMetaData("Name", SqlDbType.NVarChar, 64)
            );

            foreach (string userRole in this)
            {
                sdr.SetString(0, userRole);
                yield return sdr;
            }
        }
    }
}
