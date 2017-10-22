using System;
using System.Collections.Generic;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Resources;
using Riverside.Cms.Elements.Resources;
using Riverside.UI.Forms;
using Riverside.Utilities.Data;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Elements.Contacts
{
    public class ContactService : IAdvancedElementService, IFormService
    {
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IContactRepository _contactRepository;
        private IFormHelperService _formHelperService;

        public ContactService(IAuthenticationService authenticationService, IAuthorizationService authorizationService, IContactRepository contactRepository, IFormHelperService formHelperService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _contactRepository = contactRepository;
            _formHelperService = formHelperService;
        }

        public Guid ElementTypeId
        {
            get
            {
                return new Guid("4e6b936d-e8a1-4ff2-9576-9f9b78f82895");
            }
        }

        public IElementSettings New(long tenantId)
        {
            return new ContactSettings { TenantId = tenantId, ElementTypeId = ElementTypeId };
        }

        public IElementInfo NewInfo(IElementSettings settings, IElementContent content)
        {
            return new ElementInfo<ContactSettings, ContactContent> { Settings = settings, Content = content };
        }

        public void Create(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _contactRepository.Create((ContactSettings)settings, unitOfWork);
        }

        public void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null)
        {
            _contactRepository.Copy(sourceTenantId, sourceElementId, destTenantId, destElementId, unitOfWork);
        }

        public void Read(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _contactRepository.Read((ContactSettings)settings, unitOfWork);
        }

        public void Update(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _contactRepository.Update((ContactSettings)settings, unitOfWork);
        }

        public void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null)
        {
            _contactRepository.Delete(tenantId, elementId, unitOfWork);
        }

        public IElementContent GetContent(IElementSettings settings, IPageContext pageContext, IUnitOfWork unitOfWork = null)
        {
            ContactContent content = new ContactContent { PartialViewName = "Contact" };
            ContactSettings contactSettings = (ContactSettings)settings;
            if (contactSettings.FacebookUsername != null)
                content.FacebookUrl = string.Format("https://www.facebook.com/{0}", contactSettings.FacebookUsername);
            if (contactSettings.InstagramUsername != null)
                content.InstagramUrl = string.Format("https://www.instagram.com/{0}", contactSettings.InstagramUsername);
            if (contactSettings.LinkedInCompanyUsername != null)
                content.LinkedInCompanyUrl = string.Format("https://www.linkedin.com/company/{0}", contactSettings.LinkedInCompanyUsername);
            if (contactSettings.LinkedInPersonalUsername != null)
                content.LinkedInPersonalUrl = string.Format("https://www.linkedin.com/in/{0}", contactSettings.LinkedInPersonalUsername);
            if (contactSettings.TwitterUsername != null)
                content.TwitterUrl = string.Format("https://twitter.com/{0}", contactSettings.TwitterUsername);
            if (contactSettings.YouTubeChannelId != null)
                content.YouTubeChannelUrl = string.Format("https://www.youtube.com/channel/{0}", contactSettings.YouTubeChannelId);
            return content;
        }

        public Guid FormId { get { return new Guid("4e6b936d-e8a1-4ff2-9576-9f9b78f82895"); } }

        public Form GetForm(string context)
        {
            // Check permissions
            _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

            // Get page and element identifiers
            string[] parts = context.Split('|');
            long pageId = Convert.ToInt64(parts[0]);
            long elementId = Convert.ToInt64(parts[1]);

            // Get current settings
            Guid elementTypeId = FormId;
            ContactSettings settings = (ContactSettings)New(_authenticationService.TenantId);
            settings.ElementId = elementId;
            Read(settings);

            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("displayName", new TextField {
                Name = "displayName",
                Label = ElementResource.ContactDisplayNameLabel,
                MaxLength = ContactLengths.DisplayNameMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.ContactDisplayNameMaxLengthMessage, "displayName", ContactLengths.DisplayNameMaxLength),
                Value = settings.DisplayName
            });
            form.Fields.Add("preamble", new MultiLineTextField {
                Name = "preamble",
                Label = ElementResource.ContactPreambleLabel,
                Value = settings.Preamble,
                Rows = 4
            });
            form.Fields.Add("address", new TextField {
                Name = "address",
                Label = ElementResource.ContactAddressLabel,
                MaxLength = ContactLengths.AddressMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.ContactAddressMaxLengthMessage, "address", ContactLengths.AddressMaxLength),
                Value = settings.Address
            });
            form.Fields.Add("email", new TextField {
                Name = "email",
                Label = ElementResource.ContactEmailLabel,
                MaxLength = ContactLengths.EmailMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.ContactEmailMaxLengthMessage, "email", ContactLengths.EmailMaxLength),
                Value = settings.Email
            });
            form.Fields.Add("facebookUsername", new TextField {
                Name = "facebookUsername",
                Label = ElementResource.ContactFacebookUsernameLabel,
                MaxLength = ContactLengths.FacebookUsernameMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.ContactFacebookUsernameMaxLengthMessage, "facebookUsername", ContactLengths.FacebookUsernameMaxLength),
                Value = settings.FacebookUsername
            });
            form.Fields.Add("instagramUsername", new TextField {
                Name = "instagramUsername",
                Label = ElementResource.ContactInstagramUsernameLabel,
                MaxLength = ContactLengths.InstagramUsernameMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.ContactInstagramUsernameMaxLengthMessage, "instagramUsername", ContactLengths.InstagramUsernameMaxLength),
                Value = settings.InstagramUsername
            });
            form.Fields.Add("linkedInCompanyUsername", new TextField {
                Name = "linkedInCompanyUsername",
                Label = ElementResource.ContactLinkedInCompanyUsernameLabel,
                MaxLength = ContactLengths.LinkedInCompanyUsernameMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.ContactLinkedInCompanyUsernameMaxLengthMessage, "linkedInCompanyUsername", ContactLengths.LinkedInCompanyUsernameMaxLength),
                Value = settings.LinkedInCompanyUsername
            });
            form.Fields.Add("linkedInPersonalUsername", new TextField {
                Name = "linkedInPersonalUsername",
                Label = ElementResource.ContactLinkedInPersonalUsernameLabel,
                MaxLength = ContactLengths.LinkedInPersonalUsernameMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.ContactLinkedInPersonalUsernameMaxLengthMessage, "linkedInPersonalUsername", ContactLengths.LinkedInPersonalUsernameMaxLength),
                Value = settings.LinkedInPersonalUsername
            });
            form.Fields.Add("telephoneNumber1", new TextField {
                Name = "telephoneNumber1",
                Label = ElementResource.ContactTelephoneNumber1Label,
                MaxLength = ContactLengths.TelephoneNumber1MaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.ContactTelephoneNumber1MaxLengthMessage, "telephoneNumber1", ContactLengths.TelephoneNumber1MaxLength),
                Value = settings.TelephoneNumber1
            });
            form.Fields.Add("telephoneNumber2", new TextField {
                Name = "telephoneNumber2",
                Label = ElementResource.ContactTelephoneNumber2Label,
                MaxLength = ContactLengths.TelephoneNumber2MaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.ContactTelephoneNumber2MaxLengthMessage, "telephoneNumber2", ContactLengths.TelephoneNumber2MaxLength),
                Value = settings.TelephoneNumber2
            });
            form.Fields.Add("twitterUsername", new TextField {
                Name = "twitterUsername",
                Label = ElementResource.ContactTwitterUsernameLabel,
                MaxLength = ContactLengths.TwitterUsernameMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.ContactTwitterUsernameMaxLengthMessage, "twitterUsername", ContactLengths.TwitterUsernameMaxLength),
                Value = settings.TwitterUsername
            });
            form.Fields.Add("youTubeChannelId", new TextField {
                Name = "youTubeChannelId",
                Label = ElementResource.ContactYouTubeChannelIdLabel,
                MaxLength = ContactLengths.YouTubeChannelIdMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.ContactYouTubeChannelIdMaxLengthMessage, "youTubeChannelId", ContactLengths.YouTubeChannelIdMaxLength),
                Value = settings.YouTubeChannelId
            });
            form.SubmitLabel = ElementResource.ContactButtonLabel;

            // Return result
            return form;
        }

        /// <summary>
        /// Submits form.
        /// </summary>
        /// <param name="form">View model containing form definition and submitted values.</param>
        /// <returns>Result of form post.</returns>
        public FormResult PostForm(Form form)
        {
            try
            {
                // Check permissions
                _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

                // Get page and element identifiers
                string[] parts = form.Context.Split('|');
                long pageId = Convert.ToInt64(parts[0]);
                long elementId = Convert.ToInt64(parts[1]);

                // Get updated settings
                ContactSettings settings = (ContactSettings)New(_authenticationService.TenantId);
                settings.ElementId = elementId;
                settings.DisplayName = string.IsNullOrWhiteSpace(((TextField)form.Fields["displayName"]).Value) ? null : ((TextField)form.Fields["displayName"]).Value;
                settings.Preamble = string.IsNullOrWhiteSpace(((MultiLineTextField)form.Fields["preamble"]).Value) ? null : ((MultiLineTextField)form.Fields["preamble"]).Value;
                settings.Address = string.IsNullOrWhiteSpace(((TextField)form.Fields["address"]).Value) ? null : ((TextField)form.Fields["address"]).Value;
                settings.Email = string.IsNullOrWhiteSpace(((TextField)form.Fields["email"]).Value) ? null : ((TextField)form.Fields["email"]).Value;
                settings.FacebookUsername = string.IsNullOrWhiteSpace(((TextField)form.Fields["facebookUsername"]).Value) ? null : ((TextField)form.Fields["facebookUsername"]).Value;
                settings.InstagramUsername = string.IsNullOrWhiteSpace(((TextField)form.Fields["instagramUsername"]).Value) ? null : ((TextField)form.Fields["instagramUsername"]).Value;
                settings.LinkedInCompanyUsername = string.IsNullOrWhiteSpace(((TextField)form.Fields["linkedInCompanyUsername"]).Value) ? null : ((TextField)form.Fields["linkedInCompanyUsername"]).Value;
                settings.LinkedInPersonalUsername = string.IsNullOrWhiteSpace(((TextField)form.Fields["linkedInPersonalUsername"]).Value) ? null : ((TextField)form.Fields["linkedInPersonalUsername"]).Value;
                settings.TelephoneNumber1 = string.IsNullOrWhiteSpace(((TextField)form.Fields["telephoneNumber1"]).Value) ? null : ((TextField)form.Fields["telephoneNumber1"]).Value;
                settings.TelephoneNumber2 = string.IsNullOrWhiteSpace(((TextField)form.Fields["telephoneNumber2"]).Value) ? null : ((TextField)form.Fields["telephoneNumber2"]).Value;
                settings.TwitterUsername = string.IsNullOrWhiteSpace(((TextField)form.Fields["twitterUsername"]).Value) ? null : ((TextField)form.Fields["twitterUsername"]).Value;
                settings.YouTubeChannelId = string.IsNullOrWhiteSpace(((TextField)form.Fields["youTubeChannelId"]).Value) ? null : ((TextField)form.Fields["youTubeChannelId"]).Value;

                // Perform the update
                Update(settings);

                // Return form result with no errors
                return _formHelperService.GetFormResult();
            }
            catch (ValidationErrorException ex)
            {
                // Return form result containing errors
                return _formHelperService.GetFormResultWithValidationErrors(ex.Errors);
            }
            catch (Exception)
            {
                // Return form result containing unexpected error message
                return _formHelperService.GetFormResultWithErrorMessage(ApplicationResource.UnexpectedErrorMessage);
            }
        }
    }
}
