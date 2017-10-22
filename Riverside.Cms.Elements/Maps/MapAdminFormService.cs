using System;
using System.Collections.Generic;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Resources;
using Riverside.Cms.Elements.Resources;
using Riverside.UI.Forms;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Elements.Maps
{
    public class MapAdminFormService : IFormService
    {
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IElementFactory _elementFactory;
        private IFormHelperService _formHelperService;

        public MapAdminFormService(IAuthenticationService authenticationService, IAuthorizationService authorizationService, IElementFactory elementFactory, IFormHelperService formHelperService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _elementFactory = elementFactory;
            _formHelperService = formHelperService;
        }

        public Guid FormId { get { return new Guid("9a4b77e3-2edd-42db-8e14-153ae1f47005"); } }

        public Form GetForm(string context)
        {
            // Check permissions
            _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

            // Get page and element identifiers
            string[] parts = context.Split('|');
            long pageId = Convert.ToInt64(parts[0]);
            long elementId = Convert.ToInt64(parts[1]);

            // Get current map settings
            Guid elementTypeId = FormId;
            IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(elementTypeId);
            MapSettings mapSettings = (MapSettings)elementService.New(_authenticationService.TenantId);
            mapSettings.ElementId = elementId;
            elementService.Read(mapSettings);

            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("displayName", new TextField
            {
                Name = "displayName",
                Label = ElementResource.MapDisplayNameLabel,
                MaxLength = MapLengths.DisplayNameMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.MapDisplayNameMaxLengthMessage, "displayName", MapLengths.DisplayNameMaxLength),
                Value = mapSettings.DisplayName
            });
            form.Fields.Add("latitude", new TextField
            { // TODO: Implement this using numeric form field that accepts decimals
                Name = "latitude",
                Label = ElementResource.MapLatitudeLabel,
                MaxLength = MapLengths.LatitudeMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.MapLatitudeMaxLengthMessage, "latitude", MapLengths.LatitudeMaxLength),
                Value = mapSettings.Latitude.ToString(),
                Required = true,
                RequiredErrorMessage = ElementResource.MapLatitudeRequiredMessage
            });
            form.Fields.Add("longitude", new TextField
            { // TODO: Implement this using numeric form field that accepts decimals
                Name = "longitude",
                Label = ElementResource.MapLongitudeLabel,
                MaxLength = MapLengths.LongitudeMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.MapLongitudeMaxLengthMessage, "longitude", MapLengths.LongitudeMaxLength),
                Value = mapSettings.Longitude.ToString(),
                Required = true,
                RequiredErrorMessage = ElementResource.MapLongitudeRequiredMessage
            });
            form.SubmitLabel = ElementResource.MapButtonLabel;

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

                // Get the HTML element service
                IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(FormId);

                // Get updated map settings
                MapSettings mapSettings = (MapSettings)elementService.New(_authenticationService.TenantId);
                mapSettings.ElementId = elementId;
                mapSettings.DisplayName = string.IsNullOrWhiteSpace(((TextField)form.Fields["displayName"]).Value) ? null : ((TextField)form.Fields["displayName"]).Value;

                // Get longitude and latitude
                double latitude;
                double longitude;
                string latitudeText = ((TextField)form.Fields["latitude"]).Value;
                string longitudeText = ((TextField)form.Fields["longitude"]).Value;
                bool latitudeSuccess = Double.TryParse(latitudeText, out latitude);
                bool longitudeSuccess = Double.TryParse(longitudeText, out longitude);
                if (!latitudeSuccess)
                    throw new ValidationErrorException(new ValidationError("latitude", ElementResource.MapLatitudeInvalidMessage));
                if (!longitudeSuccess)
                    throw new ValidationErrorException(new ValidationError("longitude", ElementResource.MapLongitudeInvalidMessage));
                mapSettings.Latitude = latitude;
                mapSettings.Longitude = longitude;

                // Perform the update
                elementService.Update(mapSettings);

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
