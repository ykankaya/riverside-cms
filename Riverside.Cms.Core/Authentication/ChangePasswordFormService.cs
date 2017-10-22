using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Cms.Core.Resources;
using Riverside.UI.Forms;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Core.Authentication
{
    /// <summary>
    /// Deals with display and submission of the change password form.
    /// </summary>
    public class ChangePasswordFormService : IFormService
    {
        // Member variables
        private IAuthenticationService _authenticationService;
        private IFormHelperService _formHelperService;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="authenticationService">Provides access to authentication related code.</param>
        /// <param name="formHelperService">Provides access to form helper methods for tasks such as creating form results.</param>
        public ChangePasswordFormService(IAuthenticationService authenticationService, IFormHelperService formHelperService)
        {
            _authenticationService = authenticationService;
            _formHelperService = formHelperService;
        }

        /// <summary>
        /// Returns GUID, identifying the form that this form service is associated with.
        /// </summary>
        public Guid FormId { get { return new Guid("663928ae-1efe-4026-acc4-478202eb7916"); } }

        /// <summary>
        /// Retrieves form.
        /// </summary>
        /// <param name="context">Form context.</param>
        /// <returns>View model used to render form.</returns>
        public Form GetForm(string context)
        {
            // Check permissions
            _authenticationService.EnsureLoggedOnUser();

            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("currentPassword", new PasswordTextField
            {
                Name = "currentPassword",
                Label = AuthenticationResource.ChangePasswordCurrentPasswordLabel,
                Required = true,
                RequiredErrorMessage = AuthenticationResource.ChangePasswordCurrentPasswordRequiredMessage
            });
            form.Fields.Add("newPassword", new PasswordTextField
            {
                Name = "newPassword",
                Label = AuthenticationResource.ChangePasswordNewPasswordLabel,
                Required = true,
                RequiredErrorMessage = AuthenticationResource.ChangePasswordNewPasswordRequiredMessage,
                MaxLength = AuthenticationLengths.PasswordMaxLength,
                MaxLengthErrorMessage = string.Format(AuthenticationResource.ChangePasswordNewPasswordLengthMessage, "newPassword", AuthenticationLengths.PasswordMaxLength, AuthenticationLengths.PasswordMinLength),
                MinLength = AuthenticationLengths.PasswordMinLength,
                MinLengthErrorMessage = string.Format(AuthenticationResource.ChangePasswordNewPasswordLengthMessage, "newPassword", AuthenticationLengths.PasswordMaxLength, AuthenticationLengths.PasswordMinLength)
            });
            form.Fields.Add("confirmPassword", new PasswordTextField
            {
                Name = "confirmPassword",
                Label = AuthenticationResource.ChangePasswordConfirmPasswordLabel,
                Required = true,
                RequiredErrorMessage = AuthenticationResource.ChangePasswordConfirmPasswordRequiredMessage
            });
            form.SubmitLabel = AuthenticationResource.ChangePasswordButtonLabel;

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
            long tenantId = _authenticationService.TenantId;
            try
            {
                // Check permissions
                _authenticationService.EnsureLoggedOnUser();

                // Get user identifier
                long userId = _authenticationService.GetCurrentUser().User.UserId;

                // Get change password model from submitted form values
                ChangePasswordModel model = new ChangePasswordModel
                {
                    CurrentPassword = ((PasswordTextField)form.Fields["currentPassword"]).Value,
                    NewPassword = ((PasswordTextField)form.Fields["newPassword"]).Value,
                    ConfirmPassword = ((PasswordTextField)form.Fields["confirmPassword"]).Value,
                    TenantId = tenantId,
                    UserId = userId
                };

                // Change user password
                _authenticationService.ChangePassword(model);

                // Return form result with no errors
                return _formHelperService.GetFormResult();
            }
            catch (UserLockedOutException)
            {
                // Log user off
                _authenticationService.Logoff();

                // Return form result with status locked out
                return _formHelperService.GetFormResult("lockedout");
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
