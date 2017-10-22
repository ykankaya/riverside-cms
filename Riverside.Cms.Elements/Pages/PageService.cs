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
    public class PageService : IBasicElementService
    {
        public Guid ElementTypeId
        {
            get
            {
                return new Guid("079a2393-1f20-4a6b-8cf2-1095e4828031");
            }
        }

        public IElementSettings New(long tenantId)
        {
            return new ElementSettings { TenantId = tenantId, ElementTypeId = ElementTypeId };
        }

        public IElementInfo NewInfo(IElementSettings settings, IElementContent content)
        {
            return new ElementInfo<ElementSettings, ElementContent> { Settings = settings, Content = content };
        }

        public IElementContent GetContent(IElementSettings settings, IPageContext pageContext, IUnitOfWork unitOfWork = null)
        {
            long? masterPageId = null;
            if (pageContext.RouteValues.ContainsKey("masterpageid"))
                masterPageId = Convert.ToInt64(pageContext.RouteValues["masterpageid"]);
            if (masterPageId.HasValue)
                return new PageContent { PartialViewName = "CreatePageAdmin", FormContext = string.Format("create|{0}", masterPageId) };
            else
                return new PageContent { PartialViewName = "UpdatePageAdmin", FormContext = string.Format("update|{0}", pageContext.Page.PageId) };
        }
    }
}
