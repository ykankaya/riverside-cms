using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc.Routing;
using Riverside.Cms.Core.Authentication;
using Riverside.UI.Routes;
using Riverside.UI.Web;
using Riverside.Utilities.Http;
using Riverside.Utilities.Security;

namespace Riverside.Cms.Core.Authentication
{
    public class AuthenticationUrlService : IAuthenticationUrlService
    {
        private ISecurityService _securityService;
        private IWebHelperService _webHelperService;

        public AuthenticationUrlService(ISecurityService securityService, IWebHelperService webHelperService)
        {
            _securityService = securityService;
            _webHelperService = webHelperService;
        }

        public UrlParameters GetConfirmSetPasswordUrlParameters(long tenantId, Token confirmToken)
        {
            string confirmTokenText = _securityService.SerializeToken(confirmToken);
            return new UrlParameters
            {
                ActionName = "ConfirmSetPassword",
                ControllerName = "Users",
                RouteValues = new { activationkey = confirmTokenText },
                Protocol = _webHelperService.GetRequestScheme(),
                HostName = _webHelperService.GetRequestHost()
            };
        }

        public UrlParameters GetConfirmUrlParameters(long tenantId, Token confirmToken)
        {
            string confirmTokenText = _securityService.SerializeToken(confirmToken);
            return new UrlParameters
            {
                ActionName = "Confirm",
                ControllerName = "Users",
                RouteValues = new { activationkey = confirmTokenText },
                Protocol = _webHelperService.GetRequestScheme(),
                HostName = _webHelperService.GetRequestHost()
            };
        }

        public UrlParameters GetForgottenPasswordUrlParameters(long tenantId, Token resetPasswordToken)
        {
            string resetPasswordTokenText = _securityService.SerializeToken(resetPasswordToken);
            return new UrlParameters
            {
                ActionName = "ResetPassword",
                ControllerName = "Users",
                RouteValues = new { resetpasswordkey = resetPasswordTokenText },
                Protocol = _webHelperService.GetRequestScheme(),
                HostName = _webHelperService.GetRequestHost()
            };
        }

        public string GetUpdateProfileLogonUrl()
        {
            return _webHelperService.RouteUrl("LogonUser", new { reason = "updateprofile" });
        }
    }
}
