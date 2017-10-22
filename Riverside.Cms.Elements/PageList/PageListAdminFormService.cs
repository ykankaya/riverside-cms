using System;
using System.Collections.Generic;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Resources;
using Riverside.Cms.Elements.Resources;
using Riverside.UI.Forms;
using Riverside.Utilities.Annotations;
using Riverside.Utilities.Data;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Elements.PageList
{
    public class PageListAdminFormService : IFormService
    {
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IDataAnnotationsService _dataAnnotationsService;
        private IElementFactory _elementFactory;
        private IFormHelperService _formHelperService;
        private IPageService _pageService;

        public PageListAdminFormService(IAuthenticationService authenticationService, IAuthorizationService authorizationService, IDataAnnotationsService dataAnnotationsService, IElementFactory elementFactory, IFormHelperService formHelperService, IPageService pageService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _dataAnnotationsService = dataAnnotationsService;
            _elementFactory = elementFactory;
            _formHelperService = formHelperService;
            _pageService = pageService;
        }

        public Guid FormId { get { return new Guid("61f55535-9f3e-4ef5-96a2-bc84d648842a"); } }

        public Form GetForm(string context)
        {
            // Check permissions
            _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

            // Get page and element identifiers
            string[] parts = context.Split('|');
            long pageId = Convert.ToInt64(parts[0]);
            long elementId = Convert.ToInt64(parts[1]);

            // Get current page list settings
            Guid elementTypeId = FormId;
            IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(elementTypeId);
            PageListSettings pageListSettings = (PageListSettings)elementService.New(_authenticationService.TenantId);
            pageListSettings.ElementId = elementId;
            elementService.Read(pageListSettings);

            // Get possible parent pages for page list
            long tenantId = _authenticationService.TenantId;
            ISearchParameters searchParameters = new SearchParameters { PageIndex = 0, PageSize = 1000 }; // TODO: Need way to return all pages, not have some max bound upper limit
            ISearchResult<Page> result = _pageService.List(tenantId, searchParameters, null, PageSortBy.Name, true, true, PageType.Folder, false);

            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("displayName", new TextField
            {
                Name = "displayName",
                Label = ElementResource.PageListDisplayNameLabel,
                MaxLength = PageListLengths.DisplayNameMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.PageListDisplayNameMaxLengthMessage, "displayName", PageListLengths.DisplayNameMaxLength),
                Value = pageListSettings.DisplayName
            });
            form.Fields.Add("preamble", new MultiLineTextField
            {
                Name = "preamble",
                Label = ElementResource.PageListPreambleLabel,
                Value = pageListSettings.Preamble,
                Rows = 4
            });
            form.Fields.Add("page", new SelectListField<string>
            {
                Name = "page",
                Label = ElementResource.PageListPageLabel,
                Value = pageListSettings.PageId == null ? string.Empty : pageListSettings.PageId.Value.ToString(),
                Items = new List<ListFieldItem<string>> { new ListFieldItem<string> { Name = ElementResource.FolderDefaultOption, Value = string.Empty } }
            });
            foreach (Page page in result.Items)
                ((SelectListField<string>)form.Fields["page"]).Items.Add(new ListFieldItem<string> { Name = page.Name, Value = page.PageId.ToString() });
            form.Fields.Add("sortBy", new SelectListField<string>
            {
                Name = "sortBy",
                Label = ElementResource.PageListSortByLabel,
                Value = pageListSettings.SortBy.ToString(),
                Items = new List<ListFieldItem<string>> {
                    new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<PageSortBy>(PageSortBy.Created), Value = PageSortBy.Created.ToString() },
                    new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<PageSortBy>(PageSortBy.Updated), Value = PageSortBy.Updated.ToString() },
                    new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<PageSortBy>(PageSortBy.Occurred), Value = PageSortBy.Occurred.ToString() },
                    new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<PageSortBy>(PageSortBy.Name), Value = PageSortBy.Name.ToString() },
                },
                Required = true,
                RequiredErrorMessage = ElementResource.PageListSortByRequiredMessage
            });
            form.Fields.Add("sortAsc", new SelectListField<string>
            {
                Name = "sortAsc",
                Label = ElementResource.PageListSortAscLabel,
                Value = pageListSettings.SortAsc.ToString(),
                Items = new List<ListFieldItem<string>> {
                    new ListFieldItem<string> { Name = ElementResource.PageListSortAscendingOption, Value = true.ToString() },
                    new ListFieldItem<string> { Name = ElementResource.PageListSortDescendingOption, Value = false.ToString() }
                },
                Required = true,
                RequiredErrorMessage = ElementResource.PageListSortAscRequiredMessage
            });
            form.Fields.Add("showRelated", new BooleanField
            {
                Name = "showRelated",
                Label = ElementResource.PageListShowRelatedLabel,
                Value = pageListSettings.ShowRelated
            });
            form.Fields.Add("showDescription", new BooleanField
            {
                Name = "showDescription",
                Label = ElementResource.PageListShowDescriptionLabel,
                Value = pageListSettings.ShowDescription
            });
            form.Fields.Add("showImage", new BooleanField
            {
                Name = "showImage",
                Label = ElementResource.PageListShowImageLabel,
                Value = pageListSettings.ShowImage
            });
            form.Fields.Add("showBackgroundImage", new BooleanField
            {
                Name = "showBackgroundImage",
                Label = ElementResource.PageListShowBackgroundImageLabel,
                Value = pageListSettings.ShowBackgroundImage
            });
            form.Fields.Add("showCreated", new BooleanField
            {
                Name = "showCreated",
                Label = ElementResource.PageListShowCreatedLabel,
                Value = pageListSettings.ShowCreated
            });
            form.Fields.Add("showUpdated", new BooleanField
            {
                Name = "showUpdated",
                Label = ElementResource.PageListShowUpdatedLabel,
                Value = pageListSettings.ShowUpdated
            });
            form.Fields.Add("showOccurred", new BooleanField
            {
                Name = "showOccurred",
                Label = ElementResource.PageListShowOccurredLabel,
                Value = pageListSettings.ShowOccurred
            });
            form.Fields.Add("showComments", new BooleanField
            {
                Name = "showComments",
                Label = ElementResource.PageListShowCommentsLabel,
                Value = pageListSettings.ShowComments
            });
            form.Fields.Add("showTags", new BooleanField
            {
                Name = "showTags",
                Label = ElementResource.PageListShowTagsLabel,
                Value = pageListSettings.ShowTags
            });
            form.Fields.Add("pageSize", new IntegerField
            {
                Name = "pageSize",
                Label = ElementResource.PageListPageSizeLabel,
                Min = PageListLengths.PageSizeMinValue,
                Max = PageListLengths.PageSizeMaxValue,
                Value = pageListSettings.PageSize,
                Required = true,
                RequiredErrorMessage = ElementResource.PageListPageSizeRequiredMessage,
                MinErrorMessage = string.Format(ElementResource.PageListPageSizeRangeMessage, "pageSize", PageListLengths.PageSizeMinValue, PageListLengths.PageSizeMaxValue),
                MaxErrorMessage = string.Format(ElementResource.PageListPageSizeRangeMessage, "pageSize", PageListLengths.PageSizeMinValue, PageListLengths.PageSizeMaxValue)
            });
            form.Fields.Add("showPager", new BooleanField
            {
                Name = "showPager",
                Label = ElementResource.PageListShowPagerLabel,
                Value = pageListSettings.ShowPager
            });
            form.Fields.Add("moreMessage", new TextField
            {
                Name = "moreMessage",
                Label = ElementResource.PageListMoreMessageLabel,
                MaxLength = PageListLengths.MoreMessageMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.PageListMoreMessageMaxLengthMessage, "moreMessage", PageListLengths.MoreMessageMaxLength),
                Value = pageListSettings.MoreMessage
            });
            form.Fields.Add("recursive", new BooleanField
            {
                Name = "recursive",
                Label = ElementResource.PageListRecursiveLabel,
                Value = pageListSettings.Recursive
            });
            form.Fields.Add("pageType", new SelectListField<string>
            {
                Name = "pageType",
                Label = ElementResource.PageListPageTypeLabel,
                Value = pageListSettings.PageType.ToString(),
                Items = new List<ListFieldItem<string>> {
                    new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<PageType>(PageType.Document), Value = PageType.Document.ToString() },
                    new ListFieldItem<string> { Name = _dataAnnotationsService.GetEnumDisplayName<PageType>(PageType.Folder), Value = PageType.Folder.ToString() }
                },
                Required = true,
                RequiredErrorMessage = ElementResource.PageListPageTypeRequiredMessage
            });
            form.Fields.Add("noPagesMessage", new TextField
            {
                Name = "noPagesMessage",
                Label = ElementResource.PageListNoPagesMessageLabel,
                Value = pageListSettings.NoPagesMessage
            });
            form.SubmitLabel = ElementResource.PageListButtonLabel;

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

                // Get sort by enumeration value
                PageSortBy sortBy;
                Enum.TryParse<PageSortBy>(((SelectListField<string>)form.Fields["sortBy"]).Value, out sortBy);

                // Get page type enumeration value
                PageType pageType;
                Enum.TryParse<PageType>(((SelectListField<string>)form.Fields["pageType"]).Value, out pageType);

                // Get Booleans
                bool sortAsc;
                Boolean.TryParse(((SelectListField<string>)form.Fields["sortAsc"]).Value, out sortAsc);

                // Get page list page
                string pageValue = ((SelectListField<string>)form.Fields["page"]).Value;
                long? pageListPageId = pageValue == string.Empty ? null : (long?)Convert.ToInt64(((SelectListField<string>)form.Fields["page"]).Value);
                long? pageListTenantId = pageListPageId.HasValue ? (long?)tenantId : null;

                // Get the page list element service
                IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(FormId);

                // Get updated page list settings
                PageListSettings pageListSettings = (PageListSettings)elementService.New(_authenticationService.TenantId);
                pageListSettings.ElementId = elementId;
                pageListSettings.DisplayName = string.IsNullOrWhiteSpace(((TextField)form.Fields["displayName"]).Value) ? null : ((TextField)form.Fields["displayName"]).Value;
                pageListSettings.PageId = pageListPageId;
                pageListSettings.PageTenantId = pageListTenantId;
                pageListSettings.SortBy = sortBy;
                pageListSettings.SortAsc = sortAsc;
                pageListSettings.ShowRelated = ((BooleanField)form.Fields["showRelated"]).Value;
                pageListSettings.ShowDescription = ((BooleanField)form.Fields["showDescription"]).Value;
                pageListSettings.ShowImage = ((BooleanField)form.Fields["showImage"]).Value;
                pageListSettings.ShowBackgroundImage = ((BooleanField)form.Fields["showBackgroundImage"]).Value;
                pageListSettings.ShowCreated = ((BooleanField)form.Fields["showCreated"]).Value;
                pageListSettings.ShowUpdated = ((BooleanField)form.Fields["showUpdated"]).Value;
                pageListSettings.ShowOccurred = ((BooleanField)form.Fields["showOccurred"]).Value;
                pageListSettings.ShowComments = ((BooleanField)form.Fields["showComments"]).Value;
                pageListSettings.ShowTags = ((BooleanField)form.Fields["showTags"]).Value;
                pageListSettings.PageSize = ((IntegerField)form.Fields["pageSize"]).Value.Value;
                pageListSettings.ShowPager = ((BooleanField)form.Fields["showPager"]).Value;
                pageListSettings.MoreMessage = ((TextField)form.Fields["moreMessage"]).Value;
                pageListSettings.Recursive = ((BooleanField)form.Fields["recursive"]).Value;
                pageListSettings.PageType = pageType;
                pageListSettings.NoPagesMessage = string.IsNullOrWhiteSpace(((TextField)form.Fields["noPagesMessage"]).Value) ? null : ((TextField)form.Fields["noPagesMessage"]).Value;
                pageListSettings.Preamble = string.IsNullOrWhiteSpace(((MultiLineTextField)form.Fields["preamble"]).Value) ? null : ((MultiLineTextField)form.Fields["preamble"]).Value;

                // Perform the update
                elementService.Update(pageListSettings);

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
