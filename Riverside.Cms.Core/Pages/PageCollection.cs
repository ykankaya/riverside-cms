using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Riverside.Cms.Core.Pages;

namespace Riverside.Cms.Core.Pages
{
    public class PageCollection : List<Page>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sdr = new SqlDataRecord(
                new SqlMetaData("PageId", SqlDbType.BigInt)
            );

            foreach (Page page in this)
            {
                sdr.SetInt64(0, page.PageId);
                yield return sdr;
            }
        }
    }
}
