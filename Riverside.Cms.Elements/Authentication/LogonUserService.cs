using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Resources;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.Authentication
{
    public class LogonUserService : IBasicElementService
    {
        public Guid ElementTypeId
        {
            get
            {
                return new Guid("5c17be3a-415a-4367-80bd-9406d527664c");
            }
        }

        public IElementSettings New(long tenantId)
        {
            return new ElementSettings { TenantId = tenantId, ElementTypeId = ElementTypeId };
        }

        public IElementInfo NewInfo(IElementSettings settings, IElementContent content)
        {
            return new ElementInfo<ElementSettings, LogonUserContent> { Settings = settings, Content = content };
        }

        public IElementContent GetContent(IElementSettings settings, IPageContext pageContext, IUnitOfWork unitOfWork = null)
        {
            // Get logon message
            string message = null;
            string reason;
            if (pageContext.Parameters.TryGetValue("reason", out reason))
            {
                switch (reason)
                {
                    case "resetpassword":
                        message = AuthenticationResource.ResetPasswordLogonMessage;
                        break;

                    case "changepassword":
                        message = AuthenticationResource.ChangePasswordLogonMessage;
                        break;

                    case "updateprofile":
                        message = AuthenticationResource.UpdateUserLogonMessage;
                        break;

                    case "confirmusersetpassword":
                        message = AuthenticationResource.ConfirmUserSetPasswordLogonMessage;
                        break;

                    case "confirmuser":
                        message = AuthenticationResource.ConfirmUserLogonMessage;
                        break;
                }
            }

            // Return logon user content
            return new LogonUserContent
            {
                LogonMessage = message,
                PartialViewName = "LogonUser"
            };
        }
    }
}
