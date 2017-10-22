using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Domains;
using Riverside.Cms.Core.Webs;
using Riverside.Utilities.Mail;
using Riverside.Utilities.Security;

namespace Riverside.Cms.Core.Authentication
{
    public interface IAuthenticationConfigurationService
    {
        TimeSpan GetCreateUserExpiryTimeSpan(long tenantId);
        TimeSpan GetUpdateUserExpiryTimeSpan(long tenantId);
        TimeSpan GetForgottenPasswordExpiryTimeSpan(long tenantId);
        TimeSpan GetLockOutDuration(long tenantId);
        int GetPasswordFailuresBeforeLockOut(long tenantId);
        Email GetCreateUserEmail(Web web, Domain domain, string email, string alias, Token confirmToken);
        int GetPasswordSaltSize(long tenantId);
        Email GetUpdateUserEmail(Web web, Domain domain, string email, string alias, Token confirmToken);
        Email GetForgottenPasswordEmail(Web web, Domain domain, string email, string alias, Token resetPasswordToken);
    }
}
