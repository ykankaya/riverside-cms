using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.Authentication
{
    public class ConfirmUserService : IBasicElementService
    {
        public Guid ElementTypeId
        {
            get
            {
                return new Guid("b2ab475d-39f7-4ae2-ab85-6e0bba6cd9f2");
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
            return new ElementContent { PartialViewName = "ConfirmUser" };
        }
    }
}
