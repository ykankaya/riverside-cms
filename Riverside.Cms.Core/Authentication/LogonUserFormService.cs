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
    /// Deals with display and submission of the user logon form.
    /// </summary>
    public class LogonUserFormService : IFormService
    {
        // Member variables
        private IAuthenticationService _authenticationService;
        private IFormHelperService _formHelperService;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="authenticationService">Provides access to authentication related code.</param>
        /// <param name="formHelperService">Provides access to form helper methods for tasks such as creating form results.</param>
        public LogonUserFormService(IAuthenticationService authenticationService, IFormHelperService formHelperService)
        {
            _authenticationService = authenticationService;
            _formHelperService = formHelperService;
        }

        /// <summary>
        /// Returns GUID, identifying the form that this form service is associated with.
        /// </summary>
        public Guid FormId { get { return new Guid("8bc495ba-9b4f-4d8c-b1ae-dc50dc3b22b1"); } }

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
                SubmitLabel = AuthenticationResource.LogonUserButtonLabel,
                Id = FormId.ToString(),
                Context = context
            };

            // Add form fields
            form.Fields.Add("email", new TextField
            {
                Name = "email",
                Label = AuthenticationResource.EmailLabel,
                Required = true,
                RequiredErrorMessage = AuthenticationResource.EmailRequiredMessage,
                MaxLength = AuthenticationLengths.EmailMaxLength,
                MaxLengthErrorMessage = string.Format(AuthenticationResource.EmailMaxLengthMessage, "email", AuthenticationLengths.EmailMaxLength),
                Pattern = RegularExpression.Email,
                PatternErrorMessage = AuthenticationResource.EmailInvalidMessage
            });
            form.Fields.Add("password", new PasswordTextField
            {
                Name = "password",
                Label = AuthenticationResource.PasswordLabel,
                Required = true,
                RequiredErrorMessage = AuthenticationResource.PasswordRequiredMessage
            });
            form.Fields.Add("rememberMe", new BooleanField
            {
                Name = "rememberMe",
                Label = AuthenticationResource.RememberMeLabel
            });

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

                // Get confirm user set password model from submitted form values
                LogonModel model = new LogonModel
                {
                    Email = ((TextField)form.Fields["email"]).Value,
                    Password = ((PasswordTextField)form.Fields["password"]).Value,
                    RememberMe = ((BooleanField)form.Fields["rememberMe"]).Value,
                    TenantId = tenantId
                };

                // Log user on
                _authenticationService.Logon(model);

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
