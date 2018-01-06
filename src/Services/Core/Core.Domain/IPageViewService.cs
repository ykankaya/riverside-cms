using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Core.Domain
{
    public interface IPageViewService
    {
        Task<PageView> ReadPageViewAsync(long tenantId, long pageId);
    }
}
