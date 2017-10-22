using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.UI.Routes;
using Riverside.Utilities.Http;
using Riverside.Utilities.Security;

namespace Riverside.Cms.Core.Authentication
{
    public interface IAuthenticationUrlService
    {
        UrlParameters GetConfirmSetPasswordUrlParameters(long tenantId, Token confirmToken);
        UrlParameters GetConfirmUrlParameters(long tenantId, Token confirmToken);
        UrlParameters GetForgottenPasswordUrlParameters(long tenantId, Token resetPasswordToken);
        string GetUpdateProfileLogonUrl();
    }
}
