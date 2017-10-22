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

namespace Riverside.Cms.Elements.NavBars
{
    public class NavBarAdminFormService : IFormService
    {
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IElementFactory _elementFactory;
        private IFormHelperService _formHelperService;
        private IPageService _pageService;

        public NavBarAdminFormService(IAuthenticationService authenticationService, IAuthorizationService authorizationService, IElementFactory elementFactory, IFormHelperService formHelperService, IPageService pageService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _elementFactory = elementFactory;
            _formHelperService = formHelperService;
            _pageService = pageService;
        }

        public Guid FormId { get { return new Guid("a94c34c0-1a4c-4c91-a669-2f830cf1ea5f"); } }

        private IEnumerable<Page> GetFoldersAndDocuments()
        {
            long tenantId = _authenticationService.TenantId;
            ISearchParameters searchParameters = new SearchParameters { PageIndex = 0, PageSize = 1000 }; // TODO: Need way to return all pages, not have some max bound upper limit
            ISearchResult<Page> folderResult = _pageService.List(tenantId, searchParameters, null, PageSortBy.Name, true, true, PageType.Folder, false);
            ISearchResult<Page> documentResult = _pageService.List(tenantId, searchParameters, null, PageSortBy.Name, true, true, PageType.Document, false);
            IEnumerable<Page> foldersAndDocuments = folderResult.Items.Concat(documentResult.Items).OrderBy(p => p.Name);
            return foldersAndDocuments;
        }

        private SelectListField<string> GetPageSelectListField(long navBarTabId, long pageId, IEnumerable<Page> foldersAndDocuments)
        {
            SelectListField<string> selectListField = new SelectListField<string>
            {
                Name = string.Format("tab_{0}_page", navBarTabId),
                Label = ElementResource.NavBarPageLabel,
                Required = true,
                RequiredErrorMessage = ElementResource.NavBarTabPageRequiredMessage,
                Value = pageId.ToString(),
                Items = new List<ListFieldItem<string>>()
            };
            foreach (Page page in foldersAndDocuments)
                selectListField.Items.Add(new ListFieldItem<string> { Name = page.Name, Value = page.PageId.ToString() });
            return selectListField;
        }

        private TextField GetNameTextField(long navBarTabId, string name)
        {
            return new TextField
            {
                Name = string.Format("tab_{0}_name", navBarTabId),
                Label = ElementResource.NavBarTabNameLabel,
                Value = name,
                Required = true,
                RequiredErrorMessage = ElementResource.NavBarTabNameRequiredMessage,
                MaxLength = NavBarLengths.TabNameMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.NavBarTabNameMaxLengthMessage, "name", NavBarLengths.TabNameMaxLength)
            };
        }

        private IList<FormFieldSet> GetFieldSets(NavBarSettings settings, IEnumerable<Page> foldersAndDocuments)
        {
            List<FormFieldSet> fieldSets = new List<FormFieldSet>();
            foreach (NavBarTab tab in settings.Tabs)
            {
                FormFieldSet fieldSet = new FormFieldSet { Fields = new Dictionary<string, IFormField>() };
                fieldSet.Fields.Add("name", GetNameTextField(tab.NavBarTabId, tab.Name));
                fieldSet.Fields.Add("tab", GetPageSelectListField(tab.NavBarTabId, tab.PageId, foldersAndDocuments));
                fieldSets.Add(fieldSet);
            }
            return fieldSets;
        }

        private IDictionary<string, FormFieldSet> GetNamedFieldSets(IEnumerable<Page> foldersAndDocuments)
        {
            IDictionary<string, FormFieldSet> fieldSets = new Dictionary<string, FormFieldSet>();
            FormFieldSet fieldSet = new FormFieldSet { Fields = new Dictionary<string, IFormField>() };
            fieldSet.Fields.Add("name", GetNameTextField(0, null));
            fieldSet.Fields.Add("tab", GetPageSelectListField(0, foldersAndDocuments.FirstOrDefault().PageId, foldersAndDocuments));
            fieldSets.Add("newTab", fieldSet);
            return fieldSets;
        }

        public Form GetForm(string context)
        {
            // Check permissions
            _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

            // Get page and element identifiers
            string[] parts = context.Split('|');
            long pageId = Convert.ToInt64(parts[0]);
            long elementId = Convert.ToInt64(parts[1]);

            // Get current nav bar settings
            Guid elementTypeId = FormId;
            IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(elementTypeId);
            NavBarSettings navBarSettings = (NavBarSettings)elementService.New(_authenticationService.TenantId);
            navBarSettings.ElementId = elementId;
            elementService.Read(navBarSettings);

            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("navBarName", new TextField
            {
                Name = "navBarName",
                Label = ElementResource.NavBarNameLabel,
                Value = navBarSettings.NavBarName
            });
            form.Fields.Add("showLoggedOffUserOptions", new BooleanField
            {
                Name = "showLoggedOffUserOptions",
                Label = ElementResource.ShowLoggedOffUserOptionsLabel,
                Value = navBarSettings.ShowLoggedOffUserOptions
            });
            form.Fields.Add("showLoggedOnUserOptions", new BooleanField
            {
                Name = "showLoggedOnUserOptions",
                Label = ElementResource.ShowLoggedOnUserOptionsLabel,
                Value = navBarSettings.ShowLoggedOnUserOptions
            });
            form.SubmitLabel = ElementResource.NavBarButtonLabel;

            // Get form field sets
            IEnumerable<Page> foldersAndDocuments = GetFoldersAndDocuments();
            form.FieldSets = GetFieldSets(navBarSettings, foldersAndDocuments);
            form.NamedFieldSets = GetNamedFieldSets(foldersAndDocuments);

            // Return result
            return form;
        }

        private List<NavBarTab> GetTabs(long tenantId, long elementId, Form form)
        {
            List<NavBarTab> tabs = new List<NavBarTab>();
            for (int index = 0; index < form.FieldSets.Count; index++)
            {
                FormFieldSet fieldSet = form.FieldSets[index];
                long pageId = Convert.ToInt64(((SelectListField<string>)fieldSet.Fields["tab"]).Value);
                long navBarTabId = Math.Max(Convert.ToInt64(((SelectListField<string>)fieldSet.Fields["tab"]).Name.Split('_')[1]), 0);
                tabs.Add(new NavBarTab
                {
                    ElementId = elementId,
                    Name = ((TextField)fieldSet.Fields["name"]).Value,
                    NavBarTabId = navBarTabId,
                    PageId = pageId,
                    SortOrder = index,
                    TenantId = tenantId,
                });
            }
            return tabs;
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

                // Get the nav bar element service
                IAdvancedElementService elementService = (IAdvancedElementService)_elementFactory.GetElementService(FormId);

                // Get existing nav bar settings
                NavBarSettings navBarSettings = (NavBarSettings)elementService.New(_authenticationService.TenantId);
                navBarSettings.ElementId = elementId;

                // Update nav bar settings
                navBarSettings.Tabs = GetTabs(tenantId, elementId, form);
                navBarSettings.NavBarName = string.IsNullOrWhiteSpace(((TextField)form.Fields["navBarName"]).Value) ? null : ((TextField)form.Fields["navBarName"]).Value;
                navBarSettings.ShowLoggedOffUserOptions = ((BooleanField)form.Fields["showLoggedOffUserOptions"]).Value;
                navBarSettings.ShowLoggedOnUserOptions = ((BooleanField)form.Fields["showLoggedOnUserOptions"]).Value;

                // Perform the update
                elementService.Update(navBarSettings);

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
