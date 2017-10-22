using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Html
{
    public class HtmlUploadCollection : List<HtmlUpload>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sdr = new SqlDataRecord(
                new SqlMetaData("HtmlUploadId", SqlDbType.BigInt),
                new SqlMetaData("ImageTenantId", SqlDbType.BigInt),
                new SqlMetaData("ThumbnailImageUploadId", SqlDbType.BigInt),
                new SqlMetaData("PreviewImageUploadId", SqlDbType.BigInt),
                new SqlMetaData("ImageUploadId", SqlDbType.BigInt),
                new SqlMetaData("UploadTenantId", SqlDbType.BigInt),
                new SqlMetaData("UploadId", SqlDbType.BigInt)
            );

            foreach (HtmlUpload htmlUpload in this)
            {
                if (htmlUpload.HtmlUploadId != 0) sdr.SetInt64(0, htmlUpload.HtmlUploadId); else sdr.SetDBNull(0);
                if (htmlUpload.ImageTenantId.HasValue) sdr.SetInt64(1, htmlUpload.ImageTenantId.Value); else sdr.SetDBNull(1);
                if (htmlUpload.ThumbnailImageUploadId.HasValue) sdr.SetInt64(2, htmlUpload.ThumbnailImageUploadId.Value); else sdr.SetDBNull(2);
                if (htmlUpload.PreviewImageUploadId.HasValue) sdr.SetInt64(3, htmlUpload.PreviewImageUploadId.Value); else sdr.SetDBNull(3);
                if (htmlUpload.ImageUploadId.HasValue) sdr.SetInt64(4, htmlUpload.ImageUploadId.Value); else sdr.SetDBNull(4);
                if (htmlUpload.UploadTenantId.HasValue) sdr.SetInt64(5, htmlUpload.UploadTenantId.Value); else sdr.SetDBNull(5);
                if (htmlUpload.UploadId.HasValue) sdr.SetInt64(6, htmlUpload.UploadId.Value); else sdr.SetDBNull(6);
                yield return sdr;
            }
        }
    }
}
