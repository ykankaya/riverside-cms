using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Element.Domain
{
    public interface IPageHeaderElementService : IElementSettingsService<PageHeaderElementSettings>
    {
        Task<PageHeaderElementView> GetElementViewAsync(long tenantId, long elementId, long pageId);
    }
}
