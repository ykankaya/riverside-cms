using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.Testimonials
{
    /// <summary>
    /// Repository for testimonial elements.
    /// </summary>
    public interface ITestimonialRepository
    {
        void Create(TestimonialSettings settings, IUnitOfWork unitOfWork = null);
        void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null);
        void Read(TestimonialSettings settings, IUnitOfWork unitOfWork = null);
        void Update(TestimonialSettings settings, IUnitOfWork unitOfWork = null);
        void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null);
    }
}
