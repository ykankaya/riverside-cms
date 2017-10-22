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
    public class ForgottenPasswordService : IBasicElementService
    {
        public Guid ElementTypeId
        {
            get
            {
                return new Guid("e7b223b4-3b83-4b3c-8a3c-11fa8c44dda5");
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
            return new ElementContent { PartialViewName = "ForgottenPassword" };
        }
    }
}
