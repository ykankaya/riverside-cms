using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.Carousels
{
    public interface ICarouselService
    {
        CarouselSlide ReadSlide(long tenantId, long elementId, long carouselSlideId, IUnitOfWork unitOfWork = null);
    }
}
