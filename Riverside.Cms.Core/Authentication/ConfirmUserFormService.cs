using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authentication;
using Riverside.Utilities.Validation;
using Riverside.UI.Forms;

namespace Riverside.Cms.Core.Authentication
{
    /// <summary>
    /// Deals with display and submission of the confirm (activate) user form.
    /// </summary>
    public class ConfirmUserFormService : IFormService
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
        public ConfirmUserFormService(IAuthenticationService authenticationService, IAuthenticationValidator authenticationValidator, IFormHelperService formHelperService)
        {
            _authenticationService = authenticationService;
            _authenticationValidator = authenticationValidator;
            _formHelperService = formHelperService;
        }

        /// <summary>
        /// Returns GUID, identifying the form that this form service is associated with.
        /// </summary>
        public Guid FormId { get { return new Guid("9f05385f-c30b-4a22-a764-07bc38aeb6f3"); } }

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
                Id = FormId.ToString(),
                Context = context
            };

            try
            {
                // Get website identifier
                long tenantId = _authenticationService.TenantId;

                // Confirm user
                ConfirmUserModel model = new ConfirmUserModel
                {
                    ConfirmKey = context,
                    TenantId = tenantId
                };
                _authenticationService.ConfirmUser(model);
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
            // Return form result with no errors
            return _formHelperService.GetFormResult();
        }
    }
}
