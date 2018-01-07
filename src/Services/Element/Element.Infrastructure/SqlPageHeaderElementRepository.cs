using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Services.Element.Domain;

namespace Riverside.Cms.Services.Element.Infrastructure
{
    public class SqlPageHeaderElementRepository : IElementRepository<PageHeaderElementSettings>
    {
        public Task<ElementSettings> ReadElementAsync(long tenantId, long elementId)
        {
            throw new NotImplementedException();
        }
    }
}
