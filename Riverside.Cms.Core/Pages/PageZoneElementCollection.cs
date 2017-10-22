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
    public class PageZoneElementCollection : List<PageZoneElement>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sdr = new SqlDataRecord(
                new SqlMetaData("PageZoneId",               SqlDbType.BigInt),
                new SqlMetaData("PageZoneElementId",        SqlDbType.BigInt),
                new SqlMetaData("SortOrder",                SqlDbType.Int),
                new SqlMetaData("ElementId",                SqlDbType.BigInt),
                new SqlMetaData("MasterPageId",             SqlDbType.BigInt),
                new SqlMetaData("MasterPageZoneId",         SqlDbType.BigInt),
                new SqlMetaData("MasterPageZoneElementId",  SqlDbType.BigInt),
                new SqlMetaData("PageZoneMasterPageZoneId", SqlDbType.BigInt),
                new SqlMetaData("PageId",                   SqlDbType.BigInt)
            );

            foreach (PageZoneElement pageZoneElement in this)
            {
                if (pageZoneElement.PageZoneId != 0)
                    sdr.SetInt64(0, pageZoneElement.PageZoneId);
                else
                    sdr.SetDBNull(0);
                if (pageZoneElement.PageZoneElementId != 0)
                    sdr.SetInt64(1, pageZoneElement.PageZoneElementId);
                else
                    sdr.SetDBNull(1);
                if (pageZoneElement.SortOrder != null)
                    sdr.SetInt32(2, pageZoneElement.SortOrder.Value);
                else
                    sdr.SetDBNull(2);
                sdr.SetInt64(3, pageZoneElement.ElementId);

                if (pageZoneElement.MasterPageId != null)
                    sdr.SetInt64(4, pageZoneElement.MasterPageId.Value);
                else
                    sdr.SetDBNull(4);
                if (pageZoneElement.MasterPageZoneId != null)
                    sdr.SetInt64(5, pageZoneElement.MasterPageZoneId.Value);
                else
                    sdr.SetDBNull(5);
                if (pageZoneElement.MasterPageZoneElementId != null)
                    sdr.SetInt64(6, pageZoneElement.MasterPageZoneElementId.Value);
                else
                    sdr.SetDBNull(6);
                if (pageZoneElement.Parent != null)
                    sdr.SetInt64(7, pageZoneElement.Parent.MasterPageZoneId);
                else
                    sdr.SetDBNull(7);
                if (pageZoneElement.PageId != 0)
                    sdr.SetInt64(8, pageZoneElement.PageId);
                else
                    sdr.SetDBNull(8);
                yield return sdr;
            }
        }
    }
}
