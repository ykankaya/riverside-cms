using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Riverside.Cms.Elements.Html
{
    public class HtmlUrlService : IHtmlUrlService
    {
        private IActionContextAccessor _actionContextAccessor;
        private IUrlHelperFactory _urlHelperFactory;

        public HtmlUrlService(IActionContextAccessor actionContextAccessor, IUrlHelperFactory urlHelperFactory)
        {
            _actionContextAccessor = actionContextAccessor;
            _urlHelperFactory = urlHelperFactory;
        }

        public string GetHtmlUploadUrl(long elementId, long uploadId)
        {
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
            object routeValues = new { elementid = elementId, uploadid = uploadId, t = DateTime.UtcNow.Ticks };
            return urlHelper.RouteUrl("ReadElementUpload", routeValues);
        }
    }
}
