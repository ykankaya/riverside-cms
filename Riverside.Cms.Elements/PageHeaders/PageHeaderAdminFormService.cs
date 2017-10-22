using System;
using System.Collections.Generic;
using System.Linq;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Resources;
using Riverside.Cms.Elements.Resources;
using Riverside.UI.Forms;
using Riverside.Utilities.Data;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Elements.PageHeaders
{
    public class PageHeaderAdminFormService : IFormService
    {
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IElementFactory _elementFactory;
        private IFormHelperService _formHelperService;
        private IPageService _pageService;

        public PageHeaderAdminFormService(IAuthenticationService authenticationService, IAuthorizationService authorizationService, IElementFactory elementFactory, IFormHelperService formHelperService, IPageService pageService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _elementFactory = elementFactory;
            _formHelperService = formHelperService;
            _pageService = pageService;
        }

        public Guid FormId { get { return new Guid("1cbac30c-5deb-404e-8ea8-aabc20c82aa8"); } }

        public Form GetForm(string context)
        {
            // Check permissions
            _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

            // Get page and element identifiers
            string[] parts = context.Split('|');
            long pageId = Convert.ToInt64(parts[0]);
            long elementId = Convert.ToInt64(parts[1]);

            // Get current page header settings
            Guid elementTypeId = FormId;
            IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(elementTypeId);
            PageHeaderSettings pageHeaderSettings = (PageHeaderSettings)elementService.New(_authenticationService.TenantId);
            pageHeaderSettings.ElementId = elementId;
            elementService.Read(pageHeaderSettings);

            // Get possible pages for page header
            long tenantId = _authenticationService.TenantId;
            ISearchParameters searchParameters = new SearchParameters { PageIndex = 0, PageSize = 1000 }; // TODO: Need way to return all pages, not have some max bound upper limit
            ISearchResult<Page> folderResult = _pageService.List(tenantId, searchParameters, null, PageSortBy.Name, true, true, PageType.Folder, false);
            ISearchResult<Page> documentResult = _pageService.List(tenantId, searchParameters, null, PageSortBy.Name, true, true, PageType.Document, false);
            IEnumerable<Page> foldersAndDocuments = folderResult.Items.Concat(documentResult.Items).OrderBy(p => p.Name);

            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("showName", new BooleanField
            {
                Name = "showName",
                Label = ElementResource.PageHeaderShowNameLabel,
                Value = pageHeaderSettings.ShowName
            });
            form.Fields.Add("showDescription", new BooleanField
            {
                Name = "showDescription",
                Label = ElementResource.PageHeaderShowDescriptionLabel,
                Value = pageHeaderSettings.ShowDescription
            });
            form.Fields.Add("showImage", new BooleanField
            {
                Name = "showImage",
                Label = ElementResource.PageHeaderShowImageLabel,
                Value = pageHeaderSettings.ShowImage
            });
            form.Fields.Add("showCreated", new BooleanField
            {
                Name = "showCreated",
                Label = ElementResource.PageHeaderShowCreatedLabel,
                Value = pageHeaderSettings.ShowCreated
            });
            form.Fields.Add("showUpdated", new BooleanField
            {
                Name = "showUpdated",
                Label = ElementResource.PageHeaderShowUpdatedLabel,
                Value = pageHeaderSettings.ShowUpdated
            });
            form.Fields.Add("showOccurred", new BooleanField
            {
                Name = "showOccurred",
                Label = ElementResource.PageHeaderShowOccurredLabel,
                Value = pageHeaderSettings.ShowUpdated
            });
            form.Fields.Add("showBreadcrumbs", new BooleanField
            {
                Name = "showBreadcrumbs",
                Label = ElementResource.PageHeaderShowBreadcrumbsLabel,
                Value = pageHeaderSettings.ShowBreadcrumbs
            });
            form.Fields.Add("page", new SelectListField<string>
            {
                Name = "page",
                Label = ElementResource.PageHeaderPageLabel,
                Value = pageHeaderSettings.PageId == null ? string.Empty : pageHeaderSettings.PageId.Value.ToString(),
                Items = new List<ListFieldItem<string>> { new ListFieldItem<string> { Name = ElementResource.PageDefaultOption, Value = string.Empty } }
            });
            foreach (Page page in foldersAndDocuments)
                ((SelectListField<string>)form.Fields["page"]).Items.Add(new ListFieldItem<string> { Name = page.Name, Value = page.PageId.ToString() });
            form.SubmitLabel = ElementResource.PageHeaderButtonLabel;

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

                // Get page header page
                string pageValue = ((SelectListField<string>)form.Fields["page"]).Value;
                long? pageHeaderPageId = pageValue == string.Empty ? null : (long?)Convert.ToInt64(((SelectListField<string>)form.Fields["page"]).Value);
                long? pageHeaderTenantId = pageHeaderPageId.HasValue ? (long?)tenantId : null;

                // Get the page header element service
                IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(FormId);

                // Get updated page header settings
                PageHeaderSettings pageHeaderSettings = (PageHeaderSettings)elementService.New(_authenticationService.TenantId);
                pageHeaderSettings.ElementId = elementId;
                pageHeaderSettings.PageId = pageHeaderPageId;
                pageHeaderSettings.PageTenantId = pageHeaderTenantId;
                pageHeaderSettings.ShowCreated = ((BooleanField)form.Fields["showCreated"]).Value;
                pageHeaderSettings.ShowDescription = ((BooleanField)form.Fields["showDescription"]).Value;
                pageHeaderSettings.ShowImage = ((BooleanField)form.Fields["showImage"]).Value;
                pageHeaderSettings.ShowName = ((BooleanField)form.Fields["showName"]).Value;
                pageHeaderSettings.ShowOccurred = ((BooleanField)form.Fields["showOccurred"]).Value;
                pageHeaderSettings.ShowUpdated = ((BooleanField)form.Fields["showUpdated"]).Value;
                pageHeaderSettings.ShowBreadcrumbs = ((BooleanField)form.Fields["showBreadcrumbs"]).Value;

                // Perform the update
                elementService.Update(pageHeaderSettings);

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
