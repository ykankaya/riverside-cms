using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Riverside.Cms.Services.Element.Client;

namespace RiversideCms.Mvc.Services
{
    public interface IElementServiceFactory
    {
        Task<IElementSettings> ReadElementSettingsAsync(long tenantId, Guid elementTypeId, long elementId);
        Task<object> ReadElementContentAsync(long tenantId, Guid elementTypeId, long elementId, long pageId);
    }
}
