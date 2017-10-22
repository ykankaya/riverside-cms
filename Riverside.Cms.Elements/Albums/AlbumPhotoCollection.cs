using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Riverside.Cms.Elements.Albums
{
    public class AlbumPhotoCollection : List<AlbumPhoto>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sdr = new SqlDataRecord(
                new SqlMetaData("AlbumPhotoId", SqlDbType.BigInt),
                new SqlMetaData("ImageTenantId",SqlDbType.BigInt),
                new SqlMetaData("ThumbnailImageUploadId",SqlDbType.BigInt),
                new SqlMetaData("PreviewImageUploadId", SqlDbType.BigInt),
                new SqlMetaData("ImageUploadId", SqlDbType.BigInt),
                new SqlMetaData("Name", SqlDbType.NVarChar, 256),
                new SqlMetaData("Description", SqlDbType.NVarChar, -1),
                new SqlMetaData("SortOrder", SqlDbType.Int)
            );

            foreach (AlbumPhoto albumPhoto in this)
            {
                if (albumPhoto.AlbumPhotoId != 0)
                    sdr.SetInt64(0, albumPhoto.AlbumPhotoId);
                else
                    sdr.SetDBNull(0);
                sdr.SetInt64(1, albumPhoto.ImageTenantId);
                sdr.SetInt64(2, albumPhoto.ThumbnailImageUploadId);
                sdr.SetInt64(3, albumPhoto.PreviewImageUploadId);
                sdr.SetInt64(4, albumPhoto.ImageUploadId);
                if (albumPhoto.Name != null)
                    sdr.SetString(5, albumPhoto.Name);
                else
                    sdr.SetDBNull(5);
                if (albumPhoto.Description != null)
                    sdr.SetString(6, albumPhoto.Description);
                else
                    sdr.SetDBNull(6);
                sdr.SetInt32(7, albumPhoto.SortOrder);
                yield return sdr;
            }
        }
    }
}
