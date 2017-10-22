using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.Albums
{
    public interface IAlbumRepository
    {
        void Create(AlbumSettings settings, IUnitOfWork unitOfWork = null);
        void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null);
        void Read(AlbumSettings settings, IUnitOfWork unitOfWork = null);
        AlbumPhoto ReadPhoto(long tenantId, long elementId, long albumPhotoId, IUnitOfWork unitOfWork = null);
        void Update(AlbumSettings settings, IUnitOfWork unitOfWork = null);
        void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null);
    }
}
