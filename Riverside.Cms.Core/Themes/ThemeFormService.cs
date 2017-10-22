using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Assets;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Resources;
using Riverside.Cms.Core.Webs;
using Riverside.Utilities.Validation;
using Riverside.UI.Forms;

namespace Riverside.Cms.Core.Themes
{
    /// <summary>
    /// Deals with theme form retrieval and submission.
    /// </summary>
    public class ThemeFormService : IFormService
    {
        // Member variables
        private IAssetService _assetService;
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IFormHelperService _formHelperService;
        private IPagePortalService _pagePortalService;
        private IWebService _webService;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="assetService">For retrieval of asset information.</param>
        /// <param name="authenticationService">Provides access to authentication related code.</param>
        /// <param name="authorizationService">Authorization service.</param>
        /// <param name="formHelperService">Provides access to form helper methods for tasks such as creating form results.</param>
        /// <param name="pagePortalService">Page portal service.</param>
        /// <param name="webService">Used to retrieve and update web details.</param>
        public ThemeFormService(IAssetService assetService, IAuthenticationService authenticationService, IAuthorizationService authorizationService, IFormHelperService formHelperService, IPagePortalService pagePortalService, IWebService webService)
        {
            _assetService = assetService;
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _formHelperService = formHelperService;
            _pagePortalService = pagePortalService;
            _webService = webService;
        }

        /// <summary>
        /// Returns GUID, identifying the form that this form service is associated with.
        /// </summary>
        public Guid FormId { get { return new Guid("9602083f-d8b9-4be3-ada7-eeed6b3fd450"); } }

        /// <summary>
        /// Retrieves form.
        /// </summary>
        /// <param name="context">Form context.</param>
        /// <returns>View model used to render form.</returns>
        private Form GetUpdateForm(string context)
        {
            // Get website identifier and details
            long tenantId = _authenticationService.TenantId;
            Web web = _authenticationService.Web;

            // Construct view model
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("font", new SelectListField<string>
            {
                Name = "font",
                Label = ThemeResource.FontLabel,
                Items = new List<ListFieldItem<string>> { new ListFieldItem<string> { Name = ThemeResource.FontDefaultOption, Value = string.Empty } },
                Value = string.Empty
            });
            List<string> fonts = _assetService.GetFontOptions(tenantId);
            foreach (string font in fonts)
                ((SelectListField<string>)form.Fields["font"]).Items.Add(new ListFieldItem<string> { Name = font, Value = font });
            if (web.FontOption != null && fonts.Contains(web.FontOption))
                ((SelectListField<string>)form.Fields["font"]).Value = web.FontOption;
            form.Fields.Add("colour", new SelectListField<string>
            {
                Name = "colour",
                Label = ThemeResource.ColourLabel,
                Items = new List<ListFieldItem<string>> { new ListFieldItem<string> { Name = ThemeResource.ColourDefaultOption, Value = string.Empty } },
                Value = string.Empty
            });
            List<string> colours = _assetService.GetColourOptions(tenantId);
            foreach (string colour in colours)
                ((SelectListField<string>)form.Fields["colour"]).Items.Add(new ListFieldItem<string> { Name = colour, Value = colour });
            if (web.ColourOption != null && colours.Contains(web.ColourOption))
                ((SelectListField<string>)form.Fields["colour"]).Value = web.ColourOption;
            form.SubmitLabel = ThemeResource.UpdateThemeButtonLabel;

            // Return form
            return form;
        }

        /// <summary>
        /// Submits form.
        /// </summary>
        /// <param name="form">View model containing form definition and submitted values.</param>
        /// <returns>Result of form post.</returns>
        private FormResult PostUpdateForm(Form form)
        {
            // Get tenant identifier, website identifier 
            long tenantId = _authenticationService.TenantId;

            // Get font and colour options
            string fontOption = ((SelectListField<string>)form.Fields["font"]).Value;
            string colourOption = ((SelectListField<string>)form.Fields["colour"]).Value;

            // Do the update
            _webService.UpdateThemeOptions(tenantId, fontOption, colourOption);

            // Return form result with no errors
            string status = _pagePortalService.GetHomeUrl();
            return _formHelperService.GetFormResult(status);
        }

        /// <summary>
        /// Retrieves form.
        /// </summary>
        /// <param name="context">Form context.</param>
        /// <returns>View model used to render form.</returns>
        public Form GetForm(string context)
        {
            // Check permissions
            _authorizationService.AuthorizeUserForFunction(Functions.UpdateTheme);

            // The form that we will return
            Form form = GetUpdateForm(context);

            // Return the form
            return form;
        }

        /// <summary>
        /// Submits theme management form.
        /// </summary>
        /// <param name="form">View model containing form definition and submitted values.</param>
        /// <returns>Result of form post.</returns>
        public FormResult PostForm(Form form)
        {
            try
            {
                // Check permissions
                _authorizationService.AuthorizeUserForFunction(Functions.UpdateTheme);

                // The form result
                FormResult formResult = PostUpdateForm(form);

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
