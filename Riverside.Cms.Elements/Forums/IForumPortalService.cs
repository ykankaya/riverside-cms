using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Pages;

namespace Riverside.Cms.Elements.Forums
{
    public interface IForumPortalService
    {
        ThreadsViewModel GetThreadsViewModel(Page page, long tenantId, long? userId, long elementId, int? pageIndex);
        ThreadViewModel GetThreadViewModel(Page page, long tenantId, long? userId, long elementId, long threadId, int? pageIndex);
    }
}
