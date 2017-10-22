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
    public class TagCollection : List<Tag>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sdr = new SqlDataRecord(
                new SqlMetaData("TagId", SqlDbType.BigInt),
                new SqlMetaData("Name", SqlDbType.NVarChar, 50)
            );

            foreach (Tag tag in this)
            {
                if (tag.TagId == 0)
                    sdr.SetDBNull(0);
                else
                    sdr.SetInt64(0, tag.TagId);
                sdr.SetString(1, tag.Name);
                yield return sdr;
            }
        }
    }
}
