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
    /// A collection of master page zone elements that can be quickly passed to a stored procedure via a table variable parameter.
    /// </summary>
    public class MasterPageZoneElementCollection : List<MasterPageZoneElement>, IEnumerable<SqlDataRecord>
    {
        /// <summary>
        /// Retrieves master page zone element records.
        /// </summary>
        /// <returns>Enumerable SQL data records.</returns>
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sdr = new SqlDataRecord(
                new SqlMetaData("MasterPageZoneId",        SqlDbType.BigInt),
                new SqlMetaData("MasterPageZoneElementId", SqlDbType.BigInt),
                new SqlMetaData("MasterPageZoneSortOrder", SqlDbType.Int),
                new SqlMetaData("SortOrder",               SqlDbType.Int),
                new SqlMetaData("ElementId",               SqlDbType.BigInt),
                new SqlMetaData("BeginRender",             SqlDbType.NVarChar, -1),
                new SqlMetaData("EndRender",               SqlDbType.NVarChar, -1)
            );

            foreach (MasterPageZoneElement masterPageZoneElement in this)
            {
                if (masterPageZoneElement.MasterPageZoneId != 0)
                    sdr.SetInt64(0, masterPageZoneElement.MasterPageZoneId);
                else
                    sdr.SetDBNull(0);
                if (masterPageZoneElement.MasterPageZoneElementId != 0)
                    sdr.SetInt64(1, masterPageZoneElement.MasterPageZoneElementId);
                else
                    sdr.SetDBNull(1);
                sdr.SetInt32(2, masterPageZoneElement.MasterPageZoneSortOrder);
                sdr.SetInt32(3, masterPageZoneElement.SortOrder);
                sdr.SetInt64(4, masterPageZoneElement.ElementId);
                if (masterPageZoneElement.BeginRender != null)
                    sdr.SetString(5, masterPageZoneElement.BeginRender);
                else
                    sdr.SetDBNull(5);
                if (masterPageZoneElement.EndRender != null)
                    sdr.SetString(6, masterPageZoneElement.EndRender);
                else
                    sdr.SetDBNull(6);
                yield return sdr;
            }
        }
    }
}
