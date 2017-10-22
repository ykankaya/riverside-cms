using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Uploads;

namespace Riverside.Cms.Elements.Carousels
{
    public interface ICarouselValidator
    {
        void ValidatePrepareImages(long tenantId, long elementId, CreateUploadModel model, string keyPrefix = null);
        void ValidateSlide(CarouselSlide slide, string keyPrefix = null);
    }
}
