using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Elements.Testimonials;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.TestimonialCarousels
{
    /// <summary>
    /// Implements custom functionality for the testimonial carousel element. Functionality is based upon the testimonial element.
    /// The only difference between the two elements is how they are rendered. The testimonial element is a simple list of testimonials.
    /// The testimonial carousel element displays testimonials in a carousel, displaying one testimonial at a time, one after another.
    /// </summary>
    public class TestimonialCarouselService : IAdvancedElementService
    {
        private ITestimonialService _testimonialService;

        public TestimonialCarouselService(ITestimonialService testimonialService)
        {
            _testimonialService = testimonialService;
        }

        public Guid ElementTypeId { get { return new Guid("7d6d413f-bf9a-4964-9ae7-59ca0f1eb73a"); } }

        public IElementSettings New(long tenantId)
        {
            IElementSettings elementSettings = _testimonialService.New(tenantId);
            elementSettings.ElementTypeId = ElementTypeId; // Do not want to use testimonial element type identifier
            return elementSettings;
        }

        public IElementInfo NewInfo(IElementSettings settings, IElementContent content)
        {
            return _testimonialService.NewInfo(settings, content);
        }

        public void Create(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _testimonialService.Create((TestimonialSettings)settings, unitOfWork);
        }

        public void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null)
        {
            _testimonialService.Copy(sourceTenantId, sourceElementId, destTenantId, destElementId, unitOfWork);
        }

        public void Read(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _testimonialService.Read((TestimonialSettings)settings, unitOfWork);
        }

        public void Update(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _testimonialService.Update(settings, unitOfWork);
        }

        public void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null)
        {
            _testimonialService.Delete(tenantId, elementId, unitOfWork);
        }

        public IElementContent GetContent(IElementSettings settings, IPageContext pageContext, IUnitOfWork unitOfWork = null)
        {
            IElementContent elementContent = _testimonialService.GetContent(settings, pageContext, unitOfWork);
            elementContent.PartialViewName = "TestimonialCarousel"; // Display testimonial carousel front end, rather that the standard testimonial UI
            return elementContent;
        }
    }
}
