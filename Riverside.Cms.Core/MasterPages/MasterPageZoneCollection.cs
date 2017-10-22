using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Riverside.Cms.Core.MasterPages;

namespace Riverside.Cms.Core.MasterPages
{
    /// <summary>
    /// A collection of master page zones that can be quickly passed to a stored procedure via a table variable parameter.
    /// </summary>
    public class MasterPageZoneCollection : List<MasterPageZone>, IEnumerable<SqlDataRecord>
    {
        /// <summary>
        /// Retrieves master page zone records.
        /// </summary>
        /// <returns>Enumerable SQL data records.</returns>
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sdr = new SqlDataRecord(
                new SqlMetaData("MasterPageZoneId", SqlDbType.BigInt),
                new SqlMetaData("Name", SqlDbType.NVarChar, 50),
                new SqlMetaData("SortOrder", SqlDbType.Int),
                new SqlMetaData("AdminType", SqlDbType.Int),
                new SqlMetaData("ContentType", SqlDbType.Int),
                new SqlMetaData("BeginRender", SqlDbType.NVarChar, -1),
                new SqlMetaData("EndRender", SqlDbType.NVarChar, -1)
            );

            foreach (MasterPageZone masterPageZone in this)
            {
                if (masterPageZone.MasterPageZoneId != 0)
                    sdr.SetInt64(0, masterPageZone.MasterPageZoneId);
                else
                    sdr.SetDBNull(0);
                sdr.SetString(1, masterPageZone.Name);
                sdr.SetInt32(2, (int)masterPageZone.SortOrder);
                sdr.SetInt32(3, (int)masterPageZone.AdminType);
                sdr.SetInt32(4, (int)masterPageZone.ContentType);
                if (masterPageZone.BeginRender != null)
                    sdr.SetString(5, masterPageZone.BeginRender);
                else
                    sdr.SetDBNull(5);
                if (masterPageZone.EndRender != null)
                    sdr.SetString(6, masterPageZone.EndRender);
                else
                    sdr.SetDBNull(6);
                yield return sdr;
            }
        }
    }
}
