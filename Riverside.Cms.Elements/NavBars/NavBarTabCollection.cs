using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.NavBars
{
    public class NavBarTabCollection : List<NavBarTab>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sdr = new SqlDataRecord(
                new SqlMetaData("NavBarTabId", SqlDbType.BigInt),
                new SqlMetaData("Name", SqlDbType.NVarChar, 50),
                new SqlMetaData("SortOrder", SqlDbType.Int),
                new SqlMetaData("PageId", SqlDbType.BigInt)
            );

            foreach (NavBarTab navBarTab in this)
            {
                if (navBarTab.NavBarTabId != 0)
                    sdr.SetInt64(0, navBarTab.NavBarTabId);
                else
                    sdr.SetDBNull(0);
                sdr.SetString(1, navBarTab.Name);
                sdr.SetInt32(2, navBarTab.SortOrder);
                sdr.SetInt64(3, navBarTab.PageId);
                yield return sdr;
            }
        }
    }
}
