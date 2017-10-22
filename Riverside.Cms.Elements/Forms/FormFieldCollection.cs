using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Riverside.Cms.Elements.Forms
{
    public class FormFieldCollection : List<FormElementField>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sdr = new SqlDataRecord(
                new SqlMetaData("FormFieldId", SqlDbType.BigInt),
                new SqlMetaData("SortOrder", SqlDbType.Int),
                new SqlMetaData("FormFieldType", SqlDbType.Int),
                new SqlMetaData("Label", SqlDbType.NVarChar, 100),
                new SqlMetaData("Required", SqlDbType.Bit)
            );

            foreach (FormElementField field in this)
            {
                if (field.FormFieldId != 0)
                    sdr.SetInt64(0, field.FormFieldId);
                else
                    sdr.SetDBNull(0);
                sdr.SetInt32(1, field.SortOrder);
                sdr.SetInt32(2, (int)field.FieldType);
                sdr.SetString(3, field.Label);
                sdr.SetBoolean(4, field.Required);
                yield return sdr;
            }
        }
    }
}
