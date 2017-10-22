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
    /// A collection of master page zone element types that can be quickly passed to a stored procedure via a table variable parameter.
    /// </summary>
    public class MasterPageZoneElementTypeCollection : List<MasterPageZoneElementType>, IEnumerable<SqlDataRecord>
    {
        /// <summary>
        /// Retrieves master page zone element type records.
        /// </summary>
        /// <returns>Enumerable SQL data records.</returns>
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sdr = new SqlDataRecord(
                new SqlMetaData("MasterPageZoneId",        SqlDbType.BigInt),
                new SqlMetaData("MasterPageZoneSortOrder", SqlDbType.Int),
                new SqlMetaData("ElementTypeId",           SqlDbType.UniqueIdentifier)
            );

            foreach (MasterPageZoneElementType masterPageZoneElementType in this)
            {
                if (masterPageZoneElementType.MasterPageZoneId != 0)
                    sdr.SetInt64(0, masterPageZoneElementType.MasterPageZoneId);
                else
                    sdr.SetDBNull(0);
                sdr.SetInt32(1, masterPageZoneElementType.MasterPageZoneSortOrder);
                sdr.SetGuid(2, masterPageZoneElementType.ElementTypeId);
                yield return sdr;
            }
        }
    }
}
