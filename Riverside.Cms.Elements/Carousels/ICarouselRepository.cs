using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.Carousels
{
    public interface ICarouselRepository
    {
        void Create(CarouselSettings settings, IUnitOfWork unitOfWork = null);
        void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null);
        void Read(CarouselSettings settings, IUnitOfWork unitOfWork = null);
        CarouselSlide ReadSlide(long tenantId, long elementId, long carouselSlideId, IUnitOfWork unitOfWork = null);
        void Update(CarouselSettings settings, IUnitOfWork unitOfWork = null);
        void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null);
    }
}
