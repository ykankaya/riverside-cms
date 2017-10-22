using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Resources;
using Riverside.Utilities.Validation;
using Riverside.UI.Forms;

namespace Riverside.Cms.Core.Authentication
{
    /// <summary>
    /// Deals with display and submission of the reset password form.
    /// </summary>
    public class ResetPasswordFormService : IFormService
    {
        // Member variables
        private IAuthenticationService _authenticationService;
        private IAuthenticationValidator _authenticationValidator;
        private IFormHelperService _formHelperService;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="authenticationService">Provides access to authentication related code.</param>
        /// <param name="authenticationValidator">Provides access to authentication validation code.</param>
        /// <param name="formHelperService">Provides access to form helper methods for tasks such as creating form results.</param>
        public ResetPasswordFormService(IAuthenticationService authenticationService, IAuthenticationValidator authenticationValidator, IFormHelperService formHelperService)
        {
            _authenticationService = authenticationService;
            _authenticationValidator = authenticationValidator;
            _formHelperService = formHelperService;
        }

        /// <summary>
        /// Returns GUID, identifying the form that this form service is associated with.
        /// </summary>
        public Guid FormId { get { return new Guid("5d9cb54b-0a34-469f-9362-2306f73be7a5"); } }

        /// <summary>
        /// Retrieves form.
        /// </summary>
        /// <param name="context">Form context.</param>
        /// <returns>View model used to render form.</returns>
        public Form GetForm(string context)
        {
            // Construct form
            Form form = new Form
            {
                Fields = new Dictionary<string, IFormField>(),
                CustomErrorMessages = new List<string>(),
                SubmitLabel = AuthenticationResource.ResetPasswordButtonLabel,
                Id = FormId.ToString(),
                Context = context
            };

            try
            {
                // Get website identifier
                long tenantId = _authenticationService.TenantId;

                // Get confirm user details
                ResetPasswordStatusModel model = new ResetPasswordStatusModel
                {
                    ResetPasswordKey = context,
                    TenantId = tenantId
                };

                // Validate supplied details
                _authenticationValidator.ValidateResetPasswordStatus(model);

                // Add form fields
                form.Fields.Add("password", new PasswordTextField
                {
                    Name = "password",
                    Label = AuthenticationResource.ResetPasswordPasswordLabel,
                    Required = true,
                    RequiredErrorMessage = AuthenticationResource.ResetPasswordPasswordRequiredMessage,
                    MaxLength = AuthenticationLengths.PasswordMaxLength,
                    MaxLengthErrorMessage = string.Format(AuthenticationResource.ResetPasswordPasswordLengthMessage, "password", AuthenticationLengths.PasswordMaxLength, AuthenticationLengths.PasswordMinLength),
                    MinLength = AuthenticationLengths.PasswordMinLength,
                    MinLengthErrorMessage = string.Format(AuthenticationResource.ResetPasswordPasswordLengthMessage, "password", AuthenticationLengths.PasswordMaxLength, AuthenticationLengths.PasswordMinLength)
                });
                form.Fields.Add("confirmPassword", new PasswordTextField
                {
                    Name = "confirmPassword",
                    Label = AuthenticationResource.ResetPasswordConfirmPasswordLabel,
                    Required = true,
                    RequiredErrorMessage = AuthenticationResource.ResetPasswordConfirmPasswordRequiredMessage
                });
            }
            catch (ValidationErrorException ex)
            {
                foreach (ValidationError error in ex.Errors)
                {
                    form.CustomErrorMessages.Add(error.Message);
                }
            }

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
                // Get website identifier
                long tenantId = _authenticationService.TenantId;

                // Get reset password model from submitted form values
                ResetPasswordModel model = new ResetPasswordModel
                {
                    Password = ((PasswordTextField)form.Fields["password"]).Value,
                    ConfirmPassword = ((PasswordTextField)form.Fields["confirmPassword"]).Value,
                    ResetPasswordKey = form.Context,
                    TenantId = tenantId
                };

                // Confirm user and set password
                _authenticationService.ResetPassword(model);

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
