using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Element.Domain
{
    public interface IElementService
    {
        Task<ElementSettings> ReadElementAsync(long tenantId, long elementId);
        Task<ElementContent> ReadElementContentAsync(long tenantId, long elementId, long pageId);
    }

    public interface IElementService<S, C> where S : ElementSettings where C : ElementContent
    {
        Task<S> ReadElementAsync(long tenantId, long elementId);
        Task<C> ReadElementContentAsync(long tenantId, long elementId, long pageId);
    }
}
