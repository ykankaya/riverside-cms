using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Domains;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Webs;
using Riverside.Cms.Elements.Resources;
using Riverside.UI.Web;
using Riverside.Utilities.Data;
using Riverside.Utilities.Http;
using Riverside.Utilities.Mail;

namespace Riverside.Cms.Elements.Forms
{
    public class FormService : IAdvancedElementService
    {
        private IAuthenticationService _authenticationService;
        private IEmailConfigurationService _emailConfigurationService;
        private IEmailService _emailService;
        private IFormRepository _formRepository;
        private IPageService _pageService;
        private IWebHelperService _webHelperService;

        public FormService(IAuthenticationService authenticationService, IEmailConfigurationService emailConfigurationService, IEmailService emailService, IFormRepository formRepository, IPageService pageService, IWebHelperService webHelperService)
        {
            _authenticationService = authenticationService;
            _emailConfigurationService = emailConfigurationService;
            _emailService = emailService;
            _formRepository = formRepository;
            _pageService = pageService;
            _webHelperService = webHelperService;
        }

        public Guid ElementTypeId
        {
            get
            {
                return new Guid("eafbd5ab-8c98-4edc-b8e1-42f5e8bfe2dc");
            }
        }

        public IElementSettings New(long tenantId)
        {
            return new FormSettings
            {
                TenantId = tenantId,
                ElementTypeId = ElementTypeId,
                RecipientEmail = "test@example.com",
                SubmitButtonLabel = ElementResource.FormDefaultSubmitButtonLabel,
                SubmittedMessage = ElementResource.FormDefaultSubmittedMessage,
                Fields = new List<FormElementField>() {
                     new FormElementField { FieldType = FormElementFieldType.MultiLineTextField, Required = true, TenantId = tenantId, Label = ElementResource.FormDefaultFieldLabel }
                }
            };
        }

        public IElementInfo NewInfo(IElementSettings settings, IElementContent content)
        {
            return new ElementInfo<FormSettings, FormContent> { Settings = settings, Content = content };
        }

        public void Create(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _formRepository.Create((FormSettings)settings, unitOfWork);
        }

        public void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null)
        {
            _formRepository.Copy(sourceTenantId, sourceElementId, destTenantId, destElementId, unitOfWork);
        }

        public void Read(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _formRepository.Read((FormSettings)settings, unitOfWork);
        }

        public void Update(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _formRepository.Update((FormSettings)settings, unitOfWork);
        }

        public void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null)
        {
            _formRepository.Delete(tenantId, elementId, unitOfWork);
        }

        public IElementContent GetContent(IElementSettings settings, IPageContext pageContext, IUnitOfWork unitOfWork = null)
        {
            return new ElementContent { PartialViewName = "Form" };
        }

        private string GetPlainTextEmailContent(Page page, IList<FormElementFieldValue> fieldValues)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format(ElementResource.FormEmailHeading, page.Name));
            sb.AppendLine();
            for (int index = 0; index < fieldValues.Count; index++)
            {
                FormElementFieldValue fieldValue = fieldValues[index];
                sb.AppendLine(fieldValue.Label + ":");
                sb.AppendLine(fieldValue.Value ?? string.Empty);
                if (index != fieldValues.Count - 1)
                    sb.AppendLine();
            }
            return sb.ToString();
        }

        private string GetHtmlEmailContent(Page page, IList<FormElementFieldValue> fieldValues)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<style>");
            sb.AppendLine("h1 { margin: 20px 0 10px 0; padding: 0; font-family: \"Helvetica Neue\", Helvetica, Arial, sans-serif; font-size: 36px; line-height: 1.1; color: #333333; }");
            sb.AppendLine("h2 { margin: 20px 0 10px 0; padding: 0; font-family: \"Helvetica Neue\", Helvetica, Arial, sans-serif; font-size: 30px; line-height: 1.1; color: #333333; }");
            sb.AppendLine("p { margin: 10px 0 10px 0; padding: 0; width: 100%; font-family: \"Helvetica Neue\", Helvetica, Arial, sans-serif; font-size: 14px; line-height: 1.4; color: #333333; }");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendFormat("<h1>{0}</h1>", _webHelperService.HtmlEncode(string.Format(ElementResource.FormEmailHeading, page.Name)));
            for (int index = 0; index < fieldValues.Count; index++)
            {
                sb.AppendLine("<p>");
                FormElementFieldValue fieldValue = fieldValues[index];
                sb.AppendLine("<strong>" + fieldValue.Label + ":</strong><br>");
                sb.AppendLine(_webHelperService.FormatMultiLine(_webHelperService.HtmlEncode(fieldValue.Value ?? string.Empty)));
                sb.AppendLine("</p>");
            }
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            return sb.ToString();
        }

        private EmailContent GetEmailContent(Page page, IList<FormElementFieldValue> fieldValues)
        {
            return new EmailContent
            {
                Subject = string.Format(ElementResource.FormEmailSubject, page.Name),
                HtmlBody = GetHtmlEmailContent(page, fieldValues),
                PlainTextBody = GetPlainTextEmailContent(page, fieldValues)
            };
        }

        /// <summary>
        /// Converts email addresses in a text string into a list of strongly typed emails.
        /// Credit: http://stackoverflow.com/questions/1547476/easiest-way-to-split-a-string-on-newlines-in-net (see Guffa's answer).
        /// </summary>
        /// <param name="separators">Separators used to split text string of tags.</param>
        /// <param name="recipientEmail">Text string containing email addresses.</param>
        /// <returns>Collection of email addresses.</returns>
        private IEnumerable<EmailAddress> GetEmailAddresses(string[] separators, string recipientEmail)
        {
            if (string.IsNullOrWhiteSpace(recipientEmail))
                return new List<EmailAddress>();
            return recipientEmail.Split(separators, StringSplitOptions.None).Select(e => e.ToLower().Trim()).Distinct().Where(e => e != string.Empty).Select(e => new EmailAddress { Email = e });
        }

        private Email GetEmail(Web web, Domain domain, Page page, FormSettings formSettings, IList<FormElementFieldValue> fieldValues)
        {
            // Get to recipients
            IEnumerable<EmailAddress> toAddresses = GetEmailAddresses(new string[] { "\r\n", "\n" }, formSettings.RecipientEmail);
            IEnumerable<EmailAddress> configurationToAddresses = _emailConfigurationService.GetToEmailRecipients();
            if (configurationToAddresses != null)
                toAddresses.Concat(configurationToAddresses);

            // Get from (and reply to) email address
            string host = _webHelperService.GetHostFromUrl(domain.Url, true).ToLower();
            EmailAddress fromEmailAddress = new EmailAddress { Email = string.Format("donotreply@{0}", host), DisplayName = web.Name };

            // Return email to send
            return new Email
            {
                BccAddresses = _emailConfigurationService.GetBccEmailRecipients(),
                Content = GetEmailContent(page, fieldValues),
                FromAddress = fromEmailAddress,
                ReplyToAddress = fromEmailAddress,
                ToAddresses = toAddresses,
            };
        }

        /// <summary>
        /// Sends submitted form field values to recipient. TODO: Verify submitted form fields are as specified in form definition.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <param name="pageId">The page containing the form that has been submitted.</param>
        /// <param name="elementId">Identifies element whose fields are being sent.</param>
        /// <param name="fieldValues">Field values to send.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Send(long tenantId, long pageId, long elementId, IList<FormElementFieldValue> fieldValues, IUnitOfWork unitOfWork = null)
        {
            // Get form settings
            FormSettings formSettings = (FormSettings)New(tenantId);
            formSettings.ElementId = elementId;
            Read(formSettings, unitOfWork);

            // Get page, web and domain settings
            Page page = _pageService.Read(tenantId, pageId, unitOfWork);
            Web web = _authenticationService.Web;
            Domain domain = _authenticationService.Domain;

            // Construct email
            Email email = GetEmail(web, domain, page, formSettings, fieldValues);

            // Send finished email
            _emailService.SendEmail(email);
        }
    }
}
