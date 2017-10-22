using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Riverside.Cms.Elements.Carousels
{
    public class CarouselSlideCollection : List<CarouselSlide>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sdr = new SqlDataRecord(
                new SqlMetaData("CarouselSlideId", SqlDbType.BigInt),
                new SqlMetaData("ImageTenantId", SqlDbType.BigInt),
                new SqlMetaData("ThumbnailImageUploadId", SqlDbType.BigInt),
                new SqlMetaData("PreviewImageUploadId", SqlDbType.BigInt),
                new SqlMetaData("ImageUploadId", SqlDbType.BigInt),
                new SqlMetaData("Name", SqlDbType.NVarChar, 256),
                new SqlMetaData("Description", SqlDbType.NVarChar, -1),
                new SqlMetaData("PageTenantId", SqlDbType.BigInt),
                new SqlMetaData("PageId", SqlDbType.BigInt),
                new SqlMetaData("PageText", SqlDbType.NVarChar, 50),
                new SqlMetaData("SortOrder", SqlDbType.Int)
            );

            foreach (CarouselSlide carouselSlide in this)
            {
                if (carouselSlide.CarouselSlideId != 0)
                    sdr.SetInt64(0, carouselSlide.CarouselSlideId);
                else
                    sdr.SetDBNull(0);
                sdr.SetInt64(1, carouselSlide.ImageTenantId);
                sdr.SetInt64(2, carouselSlide.ThumbnailImageUploadId);
                sdr.SetInt64(3, carouselSlide.PreviewImageUploadId);
                sdr.SetInt64(4, carouselSlide.ImageUploadId);
                if (carouselSlide.Name != null)
                    sdr.SetString(5, carouselSlide.Name);
                else
                    sdr.SetDBNull(5);
                if (carouselSlide.Description != null)
                    sdr.SetString(6, carouselSlide.Description);
                else
                    sdr.SetDBNull(6);
                if (carouselSlide.PageTenantId.HasValue)
                    sdr.SetInt64(7, carouselSlide.PageTenantId.Value);
                else
                    sdr.SetDBNull(7);
                if (carouselSlide.PageId.HasValue)
                    sdr.SetInt64(8, carouselSlide.PageId.Value);
                else
                    sdr.SetDBNull(8);
                if (carouselSlide.PageText != null)
                    sdr.SetString(9, carouselSlide.PageText);
                else
                    sdr.SetDBNull(9);
                sdr.SetInt32(10, carouselSlide.SortOrder);
                yield return sdr;
            }
        }
    }
}
