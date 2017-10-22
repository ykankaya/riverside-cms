using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using Riverside.Utilities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Pages
{
    public class PageZoneElementService : IBasicElementService
    {
        public Guid ElementTypeId
        {
            get
            {
                return new Guid("2835ba3d-0e3d-4820-8c94-fbfba293c6f0");
            }
        }

        public IElementSettings New(long tenantId)
        {
            return new ElementSettings { TenantId = tenantId, ElementTypeId = ElementTypeId };
        }

        public IElementInfo NewInfo(IElementSettings settings, IElementContent content)
        {
            return new ElementInfo<ElementSettings, PageZoneContent> { Settings = settings, Content = content };
        }

        public IElementContent GetContent(IElementSettings settings, IPageContext pageContext, IUnitOfWork unitOfWork = null)
        {
            long pageId = pageContext.Page.PageId;
            long pageZoneId = Convert.ToInt64(pageContext.RouteValues["pagezoneid"]);
            return new PageZoneContent { PartialViewName = "PageZoneAdmin", PageId = pageId, PageZoneId = pageZoneId };
        }
    }
}
