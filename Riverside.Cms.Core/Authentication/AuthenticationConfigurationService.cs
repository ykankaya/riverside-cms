using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Domains;
using Riverside.Cms.Core.Webs;
using Riverside.Cms.Core.Resources;
using Riverside.Utilities.Http;
using Riverside.Utilities.Mail;
using Riverside.Utilities.Security;
using Riverside.Utilities.Text;
using Riverside.UI.Web;
using Riverside.UI.Routes;

namespace Riverside.Cms.Core.Authentication
{
    public class AuthenticationConfigurationService : IAuthenticationConfigurationService
    {
        private IAuthenticationUrlService _authenticationUrlService;
        private IEmailConfigurationService _emailConfigurationService;
        private IStringService _stringService;
        private IWebHelperService _webHelperService;

        private const string AliasKeyword = "%ALIAS%";
        private const string EmailKeyword = "%EMAIL%";
        private const string ConfirmUrl = "%CONFIRMURL%";
        private const string ConfirmExpires = "%CONFIRMEXPIRES%";
        private const string ResetPasswordUrl = "%RESETPASSWORDURL%";
        private const string ResetPasswordExpires = "%RESETPASSWORDEXPIRES%";

        private const int SizeSalt = 16; // 256 bit salt

        public AuthenticationConfigurationService(IEmailConfigurationService emailConfigurationService, IStringService stringService, IAuthenticationUrlService authenticationUrlService, IWebHelperService webHelperService)
        {
            _authenticationUrlService = authenticationUrlService;
            _emailConfigurationService = emailConfigurationService;
            _stringService = stringService;
            _webHelperService = webHelperService;
        }

        public TimeSpan GetCreateUserExpiryTimeSpan(long tenantId)
        {
            return new TimeSpan(2, 0, 0, 0);    // 2 days
        }

        public TimeSpan GetUpdateUserExpiryTimeSpan(long tenantId)
        {
            return new TimeSpan(2, 0, 0, 0);    // 2 days
        }

        public TimeSpan GetForgottenPasswordExpiryTimeSpan(long tenantId)
        {
            return new TimeSpan(2, 0, 0, 0);    // 2 days
        }

        public TimeSpan GetLockOutDuration(long tenantId)
        {
            return new TimeSpan(0, 10, 0);      // 10 minutes
        }

        public int GetPasswordFailuresBeforeLockOut(long tenantId)
        {
            return 3;
        }

        private IEnumerable<KeyValuePair<string, string>> GetCreateUserEmailSubstitutions(long tenantId, string email, string alias, Token confirmToken)
        {
            UrlParameters urlParameters = _authenticationUrlService.GetConfirmSetPasswordUrlParameters(tenantId, confirmToken);
            string confirmUrl = _webHelperService.GetUrl(urlParameters);
            Dictionary<string, string> substitutions = new Dictionary<string, string>();
            substitutions.Add(AliasKeyword, alias);
            substitutions.Add(EmailKeyword, email);
            substitutions.Add(ConfirmUrl, confirmUrl);
            substitutions.Add(ConfirmExpires, confirmToken.Expiry.ToString("dd MMMM yyyy HH:mm:ss") + " GMT");
            return substitutions;
        }

        private EmailContent GetCreateUserEmailContent(long tenantId, string email, string alias, Token confirmToken)
        {
            EmailContent content = new EmailContent
            {
                Subject = AuthenticationResource.CreateUserEmailSubject,
                PlainTextBody = AuthenticationResource.CreateUserEmailPlainTextBody,
                HtmlBody = AuthenticationResource.CreateUserEmailHtmlBody
            };
            IEnumerable<KeyValuePair<string, string>> substitutions = GetCreateUserEmailSubstitutions(tenantId, email, alias, confirmToken);
            content.Subject = _stringService.SubstituteKeywords(content.Subject, substitutions, false);
            content.HtmlBody = _stringService.SubstituteKeywords(content.HtmlBody, substitutions, true);
            content.PlainTextBody = _stringService.SubstituteKeywords(content.PlainTextBody, substitutions, false);
            return content;
        }

        public Email GetCreateUserEmail(Web web, Domain domain, string email, string alias, Token confirmToken)
        {
            // Get to recipients
            EmailAddress toEmailAddress = new EmailAddress { Email = email, DisplayName = alias };
            IEnumerable<EmailAddress> toAddresses = Enumerable.Repeat(toEmailAddress, 1);
            IEnumerable<EmailAddress> configurationToAddresses = _emailConfigurationService.GetToEmailRecipients();
            if (configurationToAddresses != null)
                toAddresses.Concat(configurationToAddresses);

            // Get from (and reply to) email address
            string host = _webHelperService.GetHostFromUrl(domain.Url, true).ToLower();
            EmailAddress fromEmailAddress = new EmailAddress { Email = string.Format("donotreply@{0}", host), DisplayName = web.Name };

            // Get email content
            EmailContent content = GetCreateUserEmailContent(web.TenantId, email, alias, confirmToken);

            // Return resulting email
            return new Email
            {
                Content = content,
                FromAddress = fromEmailAddress,
                ToAddresses = toAddresses,
                ReplyToAddress = fromEmailAddress,
                BccAddresses = _emailConfigurationService.GetBccEmailRecipients()
            };
        }

        public int GetPasswordSaltSize(long tenantId)
        {
            return SizeSalt;
        }

        private IEnumerable<KeyValuePair<string, string>> GetUpdateUserEmailSubstitutions(long tenantId, string email, string alias, Token confirmToken)
        {
            UrlParameters urlParameters = _authenticationUrlService.GetConfirmUrlParameters(tenantId, confirmToken);
            string confirmUrl = _webHelperService.GetUrl(urlParameters);
            Dictionary<string, string> substitutions = new Dictionary<string, string>();
            substitutions.Add(AliasKeyword, alias);
            substitutions.Add(EmailKeyword, email);
            substitutions.Add(ConfirmUrl, confirmUrl);
            substitutions.Add(ConfirmExpires, confirmToken.Expiry.ToString("dd MMMM yyyy HH:mm:ss") + " GMT");
            return substitutions;
        }

        private EmailContent GetUpdateUserEmailContent(long tenantId, string email, string alias, Token confirmToken)
        {
            EmailContent content = new EmailContent
            {
                Subject = AuthenticationResource.UpdateUserEmailSubject,
                PlainTextBody = AuthenticationResource.UpdateUserEmailPlainTextBody,
                HtmlBody = AuthenticationResource.UpdateUserEmailHtmlBody
            };
            IEnumerable<KeyValuePair<string, string>> substitutions = GetUpdateUserEmailSubstitutions(tenantId, email, alias, confirmToken);
            content.Subject = _stringService.SubstituteKeywords(content.Subject, substitutions, false);
            content.HtmlBody = _stringService.SubstituteKeywords(content.HtmlBody, substitutions, true);
            content.PlainTextBody = _stringService.SubstituteKeywords(content.PlainTextBody, substitutions, false);
            return content;
        }

        public Email GetUpdateUserEmail(Web web, Domain domain, string email, string alias, Token confirmToken)
        {
            // Get to recipients
            EmailAddress toEmailAddress = new EmailAddress { Email = email, DisplayName = alias };
            IEnumerable<EmailAddress> toAddresses = Enumerable.Repeat(toEmailAddress, 1);
            IEnumerable<EmailAddress> configurationToAddresses = _emailConfigurationService.GetToEmailRecipients();
            if (configurationToAddresses != null)
                toAddresses.Concat(configurationToAddresses);

            // Get from (and reply to) email address
            string host = _webHelperService.GetHostFromUrl(domain.Url, true).ToLower();
            EmailAddress fromEmailAddress = new EmailAddress { Email = string.Format("donotreply@{0}", host), DisplayName = web.Name };

            // Get email content
            EmailContent content = GetUpdateUserEmailContent(web.TenantId, email, alias, confirmToken);

            // Return resulting email
            return new Email
            {
                Content = content,
                FromAddress = fromEmailAddress,
                ToAddresses = toAddresses,
                ReplyToAddress = fromEmailAddress,
                BccAddresses = _emailConfigurationService.GetBccEmailRecipients()
            };
        }

        private IEnumerable<KeyValuePair<string, string>> GetForgottenPasswordEmailSubstitutions(long tenantId, string email, string alias, Token resetPasswordToken)
        {
            UrlParameters urlParameters = _authenticationUrlService.GetForgottenPasswordUrlParameters(tenantId, resetPasswordToken);
            string resetPasswordUrl = _webHelperService.GetUrl(urlParameters);
            Dictionary<string, string> substitutions = new Dictionary<string, string>();
            substitutions.Add(AliasKeyword, alias);
            substitutions.Add(EmailKeyword, email);
            substitutions.Add(ResetPasswordUrl, resetPasswordUrl);
            substitutions.Add(ResetPasswordExpires, resetPasswordToken.Expiry.ToString("dd MMMM yyyy HH:mm:ss") + " GMT");
            return substitutions;
        }

        private EmailContent GetForgottenPasswordEmailContent(long tenantId, string email, string alias, Token resetPasswordToken)
        {
            EmailContent content = new EmailContent
            {
                Subject = AuthenticationResource.ForgottenPasswordEmailSubject,
                PlainTextBody = AuthenticationResource.ForgottenPasswordEmailPlainTextBody,
                HtmlBody = AuthenticationResource.ForgottenPasswordEmailHtmlBody
            };
            IEnumerable<KeyValuePair<string, string>> substitutions = GetForgottenPasswordEmailSubstitutions(tenantId, email, alias, resetPasswordToken);
            content.Subject = _stringService.SubstituteKeywords(content.Subject, substitutions, false);
            content.HtmlBody = _stringService.SubstituteKeywords(content.HtmlBody, substitutions, true);
            content.PlainTextBody = _stringService.SubstituteKeywords(content.PlainTextBody, substitutions, false);
            return content;
        }

        public Email GetForgottenPasswordEmail(Web web, Domain domain, string email, string alias, Token resetPasswordToken)
        {
            // Get to recipients
            EmailAddress toEmailAddress = new EmailAddress { Email = email, DisplayName = alias };
            IEnumerable<EmailAddress> toAddresses = Enumerable.Repeat(toEmailAddress, 1);
            IEnumerable<EmailAddress> configurationToAddresses = _emailConfigurationService.GetToEmailRecipients();
            if (configurationToAddresses != null)
                toAddresses.Concat(configurationToAddresses);

            // Get from (and reply to) email address
            string host = _webHelperService.GetHostFromUrl(domain.Url, true).ToLower();
            EmailAddress fromEmailAddress = new EmailAddress { Email = string.Format("donotreply@{0}", host), DisplayName = web.Name };

            // Get email content
            EmailContent content = GetForgottenPasswordEmailContent(web.TenantId, email, alias, resetPasswordToken);

            // Return resulting email
            return new Email
            {
                Content = content,
                FromAddress = fromEmailAddress,
                ToAddresses = toAddresses,
                ReplyToAddress = fromEmailAddress,
                BccAddresses = _emailConfigurationService.GetBccEmailRecipients()
            };
        }
    }
}
