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
    public class ResetPasswordService : IBasicElementService
    {
        public Guid ElementTypeId
        {
            get
            {
                return new Guid("9a0bd0fd-f15c-431a-a51a-a228a96a364b");
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
            return new ElementContent { PartialViewName = "ResetPassword" };
        }
    }
}
