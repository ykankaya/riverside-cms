using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Cms.Core.Resources;
using Riverside.Cms.Core.Uploads;
using Riverside.Cms.Core.Webs;
using Riverside.UI.Forms;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Core.Authentication
{
    public class AuthenticationFormService : IFormService, IFormUploadService
    {
        // Member variables
        private IAuthenticationService _authenticationService;
        private IAuthenticationUrlService _authenticationUrlService;
        private IFormHelperService _formHelperService;
        private IWebService _webService;

        public AuthenticationFormService(IAuthenticationService authenticationService, IAuthenticationUrlService authenticationUrlService, IFormHelperService formHelperService, IWebService webService)
        {
            _authenticationService = authenticationService;
            _authenticationUrlService = authenticationUrlService;
            _formHelperService = formHelperService;
            _webService = webService;
        }

        public Guid FormId { get { return new Guid("627a0edb-da2f-461b-9722-175c393c314f"); } }

        private Form GetUpdateUserForm(string context)
        {
            // Get logged on user details
            Web web = _authenticationService.Web;
            AuthenticatedUser user = _authenticationService.GetCurrentUser().User;

            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("alias", new TextField
            {
                Name = "alias",
                Label = AuthenticationResource.UpdateUserAliasLabel,
                Required = true,
                RequiredErrorMessage = AuthenticationResource.UpdateUserAliasRequiredMessage,
                MaxLength = AuthenticationLengths.AliasMaxLength,
                MaxLengthErrorMessage = string.Format(AuthenticationResource.UpdateUserAliasMaxLengthMessage, "alias", AuthenticationLengths.AliasMaxLength),
                Value = user.Alias
            });
            form.Fields.Add("email", new TextField
            {
                Name = "email",
                Label = AuthenticationResource.UpdateUserEmailLabel,
                Required = true,
                RequiredErrorMessage = AuthenticationResource.UpdateUserEmailRequiredMessage,
                MaxLength = AuthenticationLengths.EmailMaxLength,
                MaxLengthErrorMessage = string.Format(AuthenticationResource.UpdateUserEmailMaxLengthMessage, "email", AuthenticationLengths.EmailMaxLength),
                Pattern = RegularExpression.Email,
                PatternErrorMessage = AuthenticationResource.UpdateUserEmailInvalidMessage,
                Value = user.Email
            });
            if (web.UserHasImage)
            {
                form.Fields.Add("upload", new UploadField
                {
                    Name = "upload",
                    Label = AuthenticationResource.UpdateUserUploadLabel
                });
            }
            form.SubmitLabel = AuthenticationResource.UpdateUserButtonLabel;

            // Return result
            return form;
        }

        private FormResult PostUpdateUserUpload(string id, string context, string name, string contentType, byte[] content)
        {
            // Get website details
            Web web = _authenticationService.Web;

            // Get upload model
            CreateUploadModel model = new CreateUploadModel
            {
                Content = content,
                ContentType = contentType,
                Name = name,
                TenantId = web.TenantId
            };

            // Create uploads, ready to be assigned to user when form submitted
            ImageUploadIds uploadIds = _authenticationService.PrepareImages(web.TenantId, model);

            // Return form result
            string status = string.Format("{0}|{1}|{2}", uploadIds.ThumbnailImageUploadId, uploadIds.PreviewImageUploadId, uploadIds.ImageUploadId);
            return _formHelperService.GetFormResult(status);
        }

        private FormResult PostUpdateUserForm(Form form)
        {
            // Get web and user details
            Web web = _authenticationService.Web;
            long userId = _authenticationService.GetCurrentUser().User.UserId;

            // Get new profile details from form values
            UpdateUserModel model = new UpdateUserModel
            {
                Alias = ((TextField)form.Fields["alias"]).Value,
                Email = ((TextField)form.Fields["email"]).Value,
                TenantId = web.TenantId,
                UserId = userId
            };

            // Get upload identifiers for thumbnail, preview and source images
            if (web.UserHasImage)
            {
                string uploadIds = ((UploadField)form.Fields["upload"]).Value;
                if (!string.IsNullOrWhiteSpace(uploadIds))
                {
                    string[] uploadParts = uploadIds.Split('|');
                    model.ImageTenantId = web.TenantId;
                    model.ThumbnailImageUploadId = Convert.ToInt64(uploadParts[0]);
                    model.PreviewImageUploadId = Convert.ToInt64(uploadParts[1]);
                    model.ImageUploadId = Convert.ToInt64(uploadParts[2]);
                }
            }

            // Update user profile
            string status = null;
            bool confirmRequired = _authenticationService.UpdateUser(model);
            if (confirmRequired)
            {
                _authenticationService.Logoff();
                status = _authenticationUrlService.GetUpdateProfileLogonUrl();
            }

            // Return form result with no errors
            return _formHelperService.GetFormResult(status);
        }

        public Form GetForm(string context)
        {
            // Check permissions
            _authenticationService.EnsureLoggedOnUser();

            // The form that we will return
            Form form = null;

            // Get action from context
            string action = context.Split('|')[0];

            // Get the correct form based on action
            switch (action)
            {
                case "updateuser":
                    form = GetUpdateUserForm(context);
                    break;
            }

            // Return the form
            return form;
        }

        public FormResult PostUpload(string id, string context, string name, string contentType, byte[] content)
        {
            try
            {
                // Check permissions
                _authenticationService.EnsureLoggedOnUser();

                // The form result
                FormResult formResult = null;

                // Split context into different parts
                string action = context.Split('|')[0];

                // Perform the correct action based on form context
                switch (action)
                {
                    case "updateuser":
                        formResult = PostUpdateUserUpload(id, context, name, contentType, content);
                        break;
                }

                // Return result
                return formResult;
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

        public FormResult PostForm(Form form)
        {
            try
            {
                // Check permissions
                _authenticationService.EnsureLoggedOnUser();

                // The form result
                FormResult formResult = null;

                // Split context into different parts
                string action = form.Context.Split('|')[0];

                // Perform the correct action based on form context
                switch (action)
                {
                    case "updateuser":
                        formResult = PostUpdateUserForm(form);
                        break;
                }

                // Return result
                return formResult;
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
