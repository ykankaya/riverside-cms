using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.Albums
{
    public interface IAlbumService
    {
        AlbumPhoto ReadPhoto(long tenantId, long elementId, long albumPhotoId, IUnitOfWork unitOfWork = null);
    }
}
