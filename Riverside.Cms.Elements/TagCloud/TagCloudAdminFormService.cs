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

namespace Riverside.Cms.Elements.TagCloud
{
    public class TagCloudAdminFormService : IFormService
    {
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IElementFactory _elementFactory;
        private IFormHelperService _formHelperService;
        private IPageService _pageService;

        public TagCloudAdminFormService(IAuthenticationService authenticationService, IAuthorizationService authorizationService, IElementFactory elementFactory, IFormHelperService formHelperService, IPageService pageService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _elementFactory = elementFactory;
            _formHelperService = formHelperService;
            _pageService = pageService;
        }

        public Guid FormId { get { return new Guid("b910c231-7dbd-4cad-92ef-775981e895b4"); } }

        public Form GetForm(string context)
        {
            // Check permissions
            _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

            // Get page and element identifiers
            string[] parts = context.Split('|');
            long pageId = Convert.ToInt64(parts[0]);
            long elementId = Convert.ToInt64(parts[1]);

            // Get current tag cloud settings
            Guid elementTypeId = FormId;
            IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(elementTypeId);
            TagCloudSettings tagCloudSettings = (TagCloudSettings)elementService.New(_authenticationService.TenantId);
            tagCloudSettings.ElementId = elementId;
            elementService.Read(tagCloudSettings);

            // Get possible parent pages for tag cloud
            long tenantId = _authenticationService.TenantId;
            ISearchParameters searchParameters = new SearchParameters { PageIndex = 0, PageSize = 1000 }; // TODO: Need way to return all pages, not have some max bound upper limit
            ISearchResult<Page> result = _pageService.List(tenantId, searchParameters, null, PageSortBy.Name, true, true, PageType.Folder, false);

            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("displayName", new TextField
            {
                Name = "displayName",
                Label = ElementResource.TagCloudDisplayNameLabel,
                MaxLength = TagCloudLengths.DisplayNameMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.TagCloudDisplayNameMaxLengthMessage, "displayName", TagCloudLengths.DisplayNameMaxLength),
                Value = tagCloudSettings.DisplayName
            });
            form.Fields.Add("page", new SelectListField<string>
            {
                Name = "page",
                Label = ElementResource.TagCloudPageLabel,
                Value = tagCloudSettings.PageId == null ? string.Empty : tagCloudSettings.PageId.Value.ToString(),
                Items = new List<ListFieldItem<string>> { new ListFieldItem<string> { Name = ElementResource.FolderDefaultOption, Value = string.Empty } }
            });
            foreach (Page page in result.Items)
                ((SelectListField<string>)form.Fields["page"]).Items.Add(new ListFieldItem<string> { Name = page.Name, Value = page.PageId.ToString() });
            form.Fields.Add("recursive", new BooleanField
            {
                Name = "recursive",
                Label = ElementResource.TagCloudRecursiveLabel,
                Value = tagCloudSettings.Recursive
            });
            form.Fields.Add("noTagsMessage", new TextField
            {
                Name = "noTagsMessage",
                Label = ElementResource.TagCloudNoTagsMessageLabel,
                Value = tagCloudSettings.NoTagsMessage,
                Required = true,
                RequiredErrorMessage = ElementResource.TagCloudNoTagsMessageRequiredMessage
            });
            form.SubmitLabel = ElementResource.TagCloudButtonLabel;

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

                // Get website identifier
                long tenantId = _authenticationService.TenantId;

                // Get tag cloud page
                string pageValue = ((SelectListField<string>)form.Fields["page"]).Value;
                long? tagCloudPageId = pageValue == string.Empty ? null : (long?)Convert.ToInt64(((SelectListField<string>)form.Fields["page"]).Value);
                long? tagCloudTenantId = tagCloudPageId.HasValue ? (long?)tenantId : null;

                // Get the tag cloud element service
                IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(FormId);

                // Get updated tag cloud settings
                TagCloudSettings tagCloudSettings = (TagCloudSettings)elementService.New(_authenticationService.TenantId);
                tagCloudSettings.ElementId = elementId;
                tagCloudSettings.DisplayName = string.IsNullOrWhiteSpace(((TextField)form.Fields["displayName"]).Value) ? null : ((TextField)form.Fields["displayName"]).Value;
                tagCloudSettings.PageId = tagCloudPageId;
                tagCloudSettings.PageTenantId = tagCloudTenantId;
                tagCloudSettings.Recursive = ((BooleanField)form.Fields["recursive"]).Value;
                tagCloudSettings.NoTagsMessage = ((TextField)form.Fields["noTagsMessage"]).Value;

                // Perform the update
                elementService.Update(tagCloudSettings);

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
