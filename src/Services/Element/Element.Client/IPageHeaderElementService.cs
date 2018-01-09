using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Element.Client
{
    public interface IPageHeaderElementService
    {
        Task<PageHeaderElementSettings> ReadElementAsync(long tenantId, long elementId);

        Task<PageHeaderElementView> GetElementViewAsync(long tenantId, long elementId, long pageId);
    }
}
