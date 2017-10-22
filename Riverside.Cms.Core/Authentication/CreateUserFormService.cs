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
    /// Deals with display and submission of the create (register) user form.
    /// </summary>
    public class CreateUserFormService : IFormService
    {
        // Member variables
        private IAuthenticationService _authenticationService;
        private IFormHelperService _formHelperService;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="authenticationService">Provides access to authentication related code.</param>
        /// <param name="formHelperService">Provides access to form helper methods for tasks such as creating form results.</param>
        public CreateUserFormService(IAuthenticationService authenticationService, IFormHelperService formHelperService)
        {
            _authenticationService = authenticationService;
            _formHelperService = formHelperService;
        }

        /// <summary>
        /// Returns GUID, identifying the form that this form service is associated with.
        /// </summary>
        public Guid FormId { get { return new Guid("06e416f7-c219-4e05-9aba-ac0e1008e79a"); } }

        /// <summary>
        /// Retrieves form.
        /// </summary>
        /// <param name="context">Form context.</param>
        /// <returns>View model used to render form.</returns>
        public Form GetForm(string context)
        {
            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("alias", new TextField
            {
                Name = "alias",
                Label = AuthenticationResource.AliasLabel,
                Required = true,
                RequiredErrorMessage = AuthenticationResource.AliasRequiredMessage,
                MaxLength = AuthenticationLengths.AliasMaxLength,
                MaxLengthErrorMessage = string.Format(AuthenticationResource.AliasMaxLengthMessage, "alias", AuthenticationLengths.AliasMaxLength),
            });
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
            form.SubmitLabel = AuthenticationResource.CreateUserButtonLabel;

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

                // Get create user model from submitted form values
                CreateUserModel model = new CreateUserModel
                {
                    Alias = ((TextField)form.Fields["alias"]).Value,
                    Email = ((TextField)form.Fields["email"]).Value,
                    TenantId = tenantId
                };

                // Create user
                long userId = _authenticationService.CreateUser(model);

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
