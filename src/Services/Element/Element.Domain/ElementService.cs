using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Element.Domain
{
    public class ElementService : IElementService
    {
        private readonly IElementRepository _elementRepository;

        private readonly IElementService<PageHeaderElementSettings, PageHeaderElementContent> _pageHeaderElementService;

        public ElementService(
            IElementRepository elementRepository,
            IElementService<PageHeaderElementSettings, PageHeaderElementContent> pageHeaderElementService
        )
        {
            _elementRepository = elementRepository;
            _pageHeaderElementService = pageHeaderElementService;
        }

        public async Task<ElementSettings> ReadElementAsync(long tenantId, long elementId)
        {
            ElementSettings elementSettings = await _elementRepository.ReadElementAsync(tenantId, elementId);

            switch (elementSettings.ElementTypeId.ToString().ToLower())
            {
                case "1cbac30c-5deb-404e-8ea8-aabc20c82aa8":
                    return await _pageHeaderElementService.ReadElementAsync(tenantId, elementId);

                default:
                    return null;
            }
        }

        public async Task<ElementContent> ReadElementContentAsync(long tenantId, long elementId, long pageId)
        {
            ElementSettings elementSettings = await _elementRepository.ReadElementAsync(tenantId, elementId);

            switch (elementSettings.ElementTypeId.ToString().ToLower())
            {
                case "1cbac30c-5deb-404e-8ea8-aabc20c82aa8":
                    return await _pageHeaderElementService.ReadElementContentAsync(tenantId, elementId, pageId);

                default:
                    return null;
            }
        }
    }
}
