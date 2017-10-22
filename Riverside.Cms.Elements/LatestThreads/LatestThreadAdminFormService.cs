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

namespace Riverside.Cms.Elements.LatestThreads
{
    public class LatestThreadAdminFormService : IFormService
    {
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IElementFactory _elementFactory;
        private IFormHelperService _formHelperService;
        private IPageService _pageService;

        public LatestThreadAdminFormService(IAuthenticationService authenticationService, IAuthorizationService authorizationService, IElementFactory elementFactory, IFormHelperService formHelperService, IPageService pageService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _elementFactory = elementFactory;
            _formHelperService = formHelperService;
            _pageService = pageService;
        }

        public Guid FormId { get { return new Guid("f9557287-ba01-48e3-9ab4-e2f4831933d0"); } }

        public Form GetForm(string context)
        {
            // Check permissions
            _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

            // Get page and element identifiers
            string[] parts = context.Split('|');
            long pageId = Convert.ToInt64(parts[0]);
            long elementId = Convert.ToInt64(parts[1]);

            // Get current latest thread settings
            Guid elementTypeId = FormId;
            IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(elementTypeId);
            LatestThreadSettings latestThreadSettings = (LatestThreadSettings)elementService.New(_authenticationService.TenantId);
            latestThreadSettings.ElementId = elementId;
            elementService.Read(latestThreadSettings);

            // Get possible parent pages for latest thread
            long tenantId = _authenticationService.TenantId;
            ISearchParameters searchParameters = new SearchParameters { PageIndex = 0, PageSize = 1000 }; // TODO: Need way to return all pages, not have some max bound upper limit
            ISearchResult<Page> result = _pageService.List(tenantId, searchParameters, null, PageSortBy.Name, true, true, PageType.Folder, false);

            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("displayName", new TextField
            {
                Name = "displayName",
                Label = ElementResource.LatestThreadDisplayNameLabel,
                MaxLength = LatestThreadLengths.DisplayNameMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.LatestThreadDisplayNameMaxLengthMessage, "displayName", LatestThreadLengths.DisplayNameMaxLength),
                Value = latestThreadSettings.DisplayName
            });
            form.Fields.Add("preamble", new MultiLineTextField
            {
                Name = "preamble",
                Label = ElementResource.LatestThreadPreambleLabel,
                Value = latestThreadSettings.Preamble,
                Rows = 4
            });
            form.Fields.Add("page", new SelectListField<string>
            {
                Name = "page",
                Label = ElementResource.LatestThreadPageLabel,
                Value = latestThreadSettings.PageId == null ? string.Empty : latestThreadSettings.PageId.Value.ToString(),
                Items = new List<ListFieldItem<string>> { new ListFieldItem<string> { Name = ElementResource.FolderDefaultOption, Value = string.Empty } }
            });
            foreach (Page page in result.Items)
                ((SelectListField<string>)form.Fields["page"]).Items.Add(new ListFieldItem<string> { Name = page.Name, Value = page.PageId.ToString() });
            form.Fields.Add("pageSize", new IntegerField
            {
                Name = "pageSize",
                Label = ElementResource.LatestThreadPageSizeLabel,
                Min = LatestThreadLengths.PageSizeMinValue,
                Max = LatestThreadLengths.PageSizeMaxValue,
                Value = latestThreadSettings.PageSize,
                Required = true,
                RequiredErrorMessage = ElementResource.LatestThreadPageSizeRequiredMessage,
                MinErrorMessage = string.Format(ElementResource.PageListPageSizeRangeMessage, "pageSize", LatestThreadLengths.PageSizeMinValue, LatestThreadLengths.PageSizeMaxValue),
                MaxErrorMessage = string.Format(ElementResource.PageListPageSizeRangeMessage, "pageSize", LatestThreadLengths.PageSizeMinValue, LatestThreadLengths.PageSizeMaxValue)
            });
            form.Fields.Add("recursive", new BooleanField
            {
                Name = "recursive",
                Label = ElementResource.LatestThreadRecursiveLabel,
                Value = latestThreadSettings.Recursive
            });
            form.Fields.Add("noThreadsMessage", new TextField
            {
                Name = "noThreadsMessage",
                Label = ElementResource.LatestThreadNoThreadsMessageLabel,
                Value = latestThreadSettings.NoThreadsMessage,
                Required = true,
                RequiredErrorMessage = ElementResource.LatestThreadNoThreadsMessageRequiredMessage
            });
            form.SubmitLabel = ElementResource.LatestThreadButtonLabel;

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

                // Get latest thread page
                string pageValue = ((SelectListField<string>)form.Fields["page"]).Value;
                long? latestThreadPageId = pageValue == string.Empty ? null : (long?)Convert.ToInt64(((SelectListField<string>)form.Fields["page"]).Value);
                long? latestThreadTenantId = latestThreadPageId.HasValue ? (long?)tenantId : null;

                // Get the latest thread element service
                IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(FormId);

                // Get updated latest thread settings
                LatestThreadSettings latestThreadSettings = (LatestThreadSettings)elementService.New(_authenticationService.TenantId);
                latestThreadSettings.ElementId = elementId;
                latestThreadSettings.DisplayName = string.IsNullOrWhiteSpace(((TextField)form.Fields["displayName"]).Value) ? null : ((TextField)form.Fields["displayName"]).Value;
                latestThreadSettings.Preamble = string.IsNullOrWhiteSpace(((MultiLineTextField)form.Fields["preamble"]).Value) ? null : ((MultiLineTextField)form.Fields["preamble"]).Value;
                latestThreadSettings.PageId = latestThreadPageId;
                latestThreadSettings.PageTenantId = latestThreadTenantId;
                latestThreadSettings.PageSize = ((IntegerField)form.Fields["pageSize"]).Value.Value;
                latestThreadSettings.Recursive = ((BooleanField)form.Fields["recursive"]).Value;
                latestThreadSettings.NoThreadsMessage = ((TextField)form.Fields["noThreadsMessage"]).Value;

                // Perform the update
                elementService.Update(latestThreadSettings);

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
