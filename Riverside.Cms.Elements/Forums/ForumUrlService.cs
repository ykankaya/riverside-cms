using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Riverside.Cms.Core.Extensions;
using Riverside.UI.Extensions;
using Riverside.UI.Routes;
using Riverside.UI.Web;
using Riverside.Utilities.Http;

namespace Riverside.Cms.Elements.Forums
{
    public class ForumUrlService : IForumUrlService
    {
        private IActionContextAccessor _actionContextAccessor;
        private IUrlHelperFactory _urlHelperFactory;
        private IWebHelperService _webHelperService;

        public ForumUrlService(IActionContextAccessor actionContextAccessor, IUrlHelperFactory urlHelperFactory, IWebHelperService webHelperService)
        {
            _actionContextAccessor = actionContextAccessor;
            _urlHelperFactory = urlHelperFactory;
            _webHelperService = webHelperService;
        }

        public UrlParameters GetCreatePostUrlParameters(long postId, int? page)
        {
            return new UrlParameters
            {
                ActionName = "thread",
                ControllerName = null,
                RouteValues = new { page = page, postId = postId },
                Protocol = _webHelperService.GetRequestScheme(),
                HostName = _webHelperService.GetRequestHost()
            };
        }

        public string GetThreadUrl(long pageId, long threadId, string subject, int? page = null)
        {
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
            string description = urlHelper.UrlFriendly(subject);
            object routeValues = null;
            if (page.HasValue && page.Value > 0)
                routeValues = new { pageid = pageId, description = description, forumaction = "thread", threadid = threadId, page = page.Value + 1 };
            else
                routeValues = new { pageid = pageId, description = description, forumaction = "thread", threadid = threadId };
            return urlHelper.RouteUrl("ReadPage", routeValues);
        }

        public string GetForumUrl(long pageId, string name)
        {
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
            string description = urlHelper.UrlFriendly(name);
            return urlHelper.RouteUrl("ReadPage", new { pageid = pageId, description = description });
        }
    }
}
