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
    public class ChangePasswordService : IBasicElementService
    {
        public Guid ElementTypeId
        {
            get
            {
                return new Guid("65ddc9df-922b-46e0-b7aC-313ddd2da828");
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
            return new ElementContent { PartialViewName = "ChangePassword" };
        }
    }
}
