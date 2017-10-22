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
    public class PageZoneCollection : List<PageZone>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sdr = new SqlDataRecord(
                new SqlMetaData("PageZoneId",       SqlDbType.BigInt),
                new SqlMetaData("MasterPageId",     SqlDbType.BigInt),
                new SqlMetaData("MasterPageZoneId", SqlDbType.BigInt),
                new SqlMetaData("PageId",           SqlDbType.BigInt)
            );

            foreach (PageZone pageZone in this)
            {
                if (pageZone.PageZoneId != 0)
                    sdr.SetInt64(0, pageZone.PageZoneId);
                else
                    sdr.SetDBNull(0);
                sdr.SetInt64(1, pageZone.MasterPageId);
                sdr.SetInt64(2, pageZone.MasterPageZoneId);
                if (pageZone.PageId != 0)
                    sdr.SetInt64(3, pageZone.PageId);
                else
                    sdr.SetDBNull(3);
                yield return sdr;
            }
        }
    }
}
