using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Uploads;

namespace Riverside.Cms.Elements.Albums
{
    public interface IAlbumValidator
    {
        void ValidatePrepareImages(long tenantId, long elementId, CreateUploadModel model, string keyPrefix = null);
        void ValidatePhoto(AlbumPhoto photo, string keyPrefix = null);
    }
}
