using System;
using System.Collections.Generic;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Resources;
using Riverside.Cms.Elements.Resources;
using Riverside.UI.Forms;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Elements.Shares
{
    public class ShareAdminFormService : IFormService
    {
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IElementFactory _elementFactory;
        private IFormHelperService _formHelperService;

        public ShareAdminFormService(IAuthenticationService authenticationService, IAuthorizationService authorizationService, IElementFactory elementFactory, IFormHelperService formHelperService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _elementFactory = elementFactory;
            _formHelperService = formHelperService;
        }

        public Guid FormId { get { return new Guid("cf0d7834-54fb-4a6e-86db-0f238f8b1ac1"); } }

        public Form GetForm(string context)
        {
            // Check permissions
            _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

            // Get page and element identifiers
            string[] parts = context.Split('|');
            long pageId = Convert.ToInt64(parts[0]);
            long elementId = Convert.ToInt64(parts[1]);

            // Get current share settings
            Guid elementTypeId = FormId;
            IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(elementTypeId);
            ShareSettings shareSettings = (ShareSettings)elementService.New(_authenticationService.TenantId);
            shareSettings.ElementId = elementId;
            elementService.Read(shareSettings);

            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("displayName", new TextField
            {
                Name = "displayName",
                Label = ElementResource.ShareDisplayNameLabel,
                MaxLength = ShareLengths.DisplayNameMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.ShareDisplayNameMaxLengthMessage, "displayName", ShareLengths.DisplayNameMaxLength),
                Value = shareSettings.DisplayName
            });
            form.Fields.Add("shareOnDigg", new BooleanField
            {
                Name = "shareOnDigg",
                Label = ElementResource.ShareShareOnDiggLabel,
                Value = shareSettings.ShareOnDigg
            });
            form.Fields.Add("shareOnFacebook", new BooleanField
            {
                Name = "shareOnFacebook",
                Label = ElementResource.ShareShareOnFacebookLabel,
                Value = shareSettings.ShareOnFacebook
            });
            form.Fields.Add("shareOnGoogle", new BooleanField
            {
                Name = "shareOnGoogle",
                Label = ElementResource.ShareShareOnGoogleLabel,
                Value = shareSettings.ShareOnGoogle
            });
            form.Fields.Add("shareOnLinkedIn", new BooleanField
            {
                Name = "shareOnLinkedIn",
                Label = ElementResource.ShareShareOnLinkedInLabel,
                Value = shareSettings.ShareOnLinkedIn
            });
            form.Fields.Add("shareOnPinterest", new BooleanField
            {
                Name = "shareOnPinterest",
                Label = ElementResource.ShareShareOnPinterestLabel,
                Value = shareSettings.ShareOnPinterest
            });
            form.Fields.Add("shareOnReddit", new BooleanField
            {
                Name = "shareOnReddit",
                Label = ElementResource.ShareShareOnRedditLabel,
                Value = shareSettings.ShareOnReddit
            });
            form.Fields.Add("shareOnStumbleUpon", new BooleanField
            {
                Name = "shareOnStumbleUpon",
                Label = ElementResource.ShareShareOnStumbleUponLabel,
                Value = shareSettings.ShareOnStumbleUpon
            });
            form.Fields.Add("shareOnTumblr", new BooleanField
            {
                Name = "shareOnTumblr",
                Label = ElementResource.ShareShareOnTumblrLabel,
                Value = shareSettings.ShareOnTumblr
            });
            form.Fields.Add("shareOnTwitter", new BooleanField
            {
                Name = "shareOnTwitter",
                Label = ElementResource.ShareShareOnTwitterLabel,
                Value = shareSettings.ShareOnTwitter
            });
            form.SubmitLabel = ElementResource.ShareButtonLabel;

            // Return result
            return form;
        }

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

                // Get the share element service
                IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(FormId);

                // Get updated share settings
                ShareSettings shareSettings = (ShareSettings)elementService.New(_authenticationService.TenantId);
                shareSettings.ElementId = elementId;
                shareSettings.DisplayName = string.IsNullOrWhiteSpace(((TextField)form.Fields["displayName"]).Value) ? null : ((TextField)form.Fields["displayName"]).Value;
                shareSettings.ShareOnDigg = ((BooleanField)form.Fields["shareOnDigg"]).Value;
                shareSettings.ShareOnFacebook = ((BooleanField)form.Fields["shareOnFacebook"]).Value;
                shareSettings.ShareOnGoogle = ((BooleanField)form.Fields["shareOnGoogle"]).Value;
                shareSettings.ShareOnLinkedIn = ((BooleanField)form.Fields["shareOnLinkedIn"]).Value;
                shareSettings.ShareOnPinterest = ((BooleanField)form.Fields["shareOnPinterest"]).Value;
                shareSettings.ShareOnReddit = ((BooleanField)form.Fields["shareOnReddit"]).Value;
                shareSettings.ShareOnStumbleUpon = ((BooleanField)form.Fields["shareOnStumbleUpon"]).Value;
                shareSettings.ShareOnTumblr = ((BooleanField)form.Fields["shareOnTumblr"]).Value;
                shareSettings.ShareOnTwitter = ((BooleanField)form.Fields["shareOnTwitter"]).Value;

                // Perform the update
                elementService.Update(shareSettings);

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
