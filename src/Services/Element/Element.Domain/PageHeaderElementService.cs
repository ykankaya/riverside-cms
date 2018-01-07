using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Element.Domain
{
    public class PageHeaderElementService : IElementService<PageHeaderElementSettings>
    {
        private readonly IElementRepository<PageHeaderElementSettings> _elementRepository;

        public PageHeaderElementService(IElementRepository<PageHeaderElementSettings> elementRepository)
        {
            _elementRepository = elementRepository;
        }

        public Task<ElementSettings> ReadElementAsync(long tenantId, long elementId)
        {
            return _elementRepository.ReadElementAsync(tenantId, elementId);
        }
    }
}
