using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Element.Domain
{
    public class ElementService : IElementService
    {
        private readonly IElementRepository _elementRepository;

        private readonly IElementRepository<PageHeaderElementSettings> _pageHeaderElementRepository;

        public ElementService(
            IElementRepository elementRepository,
            IElementRepository<PageHeaderElementSettings> pageHeaderElementRepository
        )
        {
            _elementRepository = elementRepository;
            _pageHeaderElementRepository = pageHeaderElementRepository;
        }

        public async Task<ElementSettings> ReadElementAsync(long tenantId, long elementId)
        {
            ElementSettings elementSettings = await _elementRepository.ReadElementAsync(tenantId, elementId);

            switch (elementSettings.ElementTypeId.ToString().ToLower())
            {
                case "1cbac30c-5deb-404e-8ea8-aabc20c82aa8":
                    return await _pageHeaderElementRepository.ReadElementAsync(tenantId, elementId);

                default:
                    return null;
            }
        }
    }
}
