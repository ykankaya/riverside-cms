using System;
using System.Collections.Generic;
using System.Linq;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.MasterPages;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Resources;
using Riverside.UI.Forms;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Elements.Pages
{
    public class PageZoneFormService : IFormService
    {
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IElementService _elementService;
        private IFormHelperService _formHelperService;
        private IMasterPageService _masterPageService;
        private Core.Pages.IPageService _pageService;

        public PageZoneFormService(IAuthenticationService authenticationService, IAuthorizationService authorizationService, IElementService elementService, IFormHelperService formHelperService, IMasterPageService masterPageService, Core.Pages.IPageService pageService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _elementService = elementService;
            _formHelperService = formHelperService;
            _masterPageService = masterPageService;
            _pageService = pageService;
        }

        public Guid FormId { get { return new Guid("2835ba3d-0e3d-4820-8c94-fbfba293c6f0"); } }

        private TextField GetNameTextField(long pageZoneElementId, string name)
        {
            return new TextField
            {
                Name = string.Format("element_{0}_name", pageZoneElementId),
                Label = PageResource.PageZoneElementNameLabel,
                Value = name,
                Required = true,
                RequiredErrorMessage = PageResource.PageZoneElementNameRequiredMessage,
                MaxLength = ElementLengths.NameMaxLength,
                MaxLengthErrorMessage = string.Format(PageResource.PageZoneElementNameMaxLengthMessage, "name", ElementLengths.NameMaxLength)
            };
        }

        private SelectListField<string> GetElementTypeSelectListField(long pageZoneElementId, Guid? elementTypeId, Dictionary<Guid, ElementType> allElementTypesById, IEnumerable<ElementType> availableElementTypes)
        {
            SelectListField<string> selectListField = new SelectListField<string>
            {
                Name = string.Format("element_{0}_elementType", pageZoneElementId),
                Label = PageResource.PageZoneElementTypeLabel,
                Required = true,
                RequiredErrorMessage = PageResource.PageZoneElementTypeRequiredMessage,
                Items = new List<ListFieldItem<string>>()
            };
            if (elementTypeId.HasValue)
            {
                selectListField.Items.Add(new ListFieldItem<string> { Name = allElementTypesById[elementTypeId.Value].Name, Value = elementTypeId.ToString() });
                selectListField.Value = elementTypeId.Value.ToString();
            }
            else if (availableElementTypes.Count() > 0)
            {
                foreach (ElementType elementType in availableElementTypes)
                    selectListField.Items.Add(new ListFieldItem<string> { Name = elementType.Name, Value = elementType.ElementTypeId.ToString() });
                selectListField.Value = availableElementTypes.First().ElementTypeId.ToString();
            }
            return selectListField;
        }

        private IList<FormFieldSet> GetFieldSets(Dictionary<Guid, ElementType> elementTypesById, PageZone pageZone)
        {
            List<FormFieldSet> fieldSets = new List<FormFieldSet>();
            foreach (PageZoneElement pageZoneElement in pageZone.PageZoneElements)
            {
                FormFieldSet fieldSet = new FormFieldSet { Fields = new Dictionary<string, IFormField>() };
                IElementSettings element = _elementService.Read(pageZoneElement.TenantId, pageZoneElement.ElementTypeId, pageZoneElement.ElementId);
                fieldSet.Fields.Add("name", GetNameTextField(pageZoneElement.PageZoneElementId, element.Name));
                fieldSet.Fields.Add("elementType", GetElementTypeSelectListField(pageZoneElement.PageZoneElementId, pageZoneElement.ElementTypeId, elementTypesById, null));
                fieldSets.Add(fieldSet);
            }
            return fieldSets;
        }

        private IDictionary<string, FormFieldSet> GetNamedFieldSets(IEnumerable<ElementType> elementTypes)
        {
            IDictionary<string, FormFieldSet> fieldSets = new Dictionary<string, FormFieldSet>();
            FormFieldSet fieldSet = new FormFieldSet { Fields = new Dictionary<string, IFormField>() };
            fieldSet.Fields.Add("name", GetNameTextField(0, null));
            fieldSet.Fields.Add("elementType", GetElementTypeSelectListField(0, null, null, elementTypes));
            fieldSets.Add("newElement", fieldSet);
            return fieldSets;
        }

        public Form GetForm(string context)
        {
            // Check permissions
            _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

            // Get tenant, page and page zone identifiers
            string[] parts = context.Split('|');
            long pageId = Convert.ToInt64(parts[0]);
            long pageZoneId = Convert.ToInt64(parts[1]);
            long tenantId = _authenticationService.TenantId;

            // Get page and master page zones
            Page page = _pageService.Read(tenantId, pageId);
            MasterPage masterPage = _masterPageService.Read(tenantId, page.MasterPageId);
            PageZone pageZone = page.PageZones.Where(pz => pz.PageZoneId == pageZoneId).First();
            MasterPageZone masterPageZone = masterPage.MasterPageZones.Where(mpz => mpz.MasterPageZoneId == pageZone.MasterPageZoneId).First();

            // Get all element types
            List<IElementSettings> elements = new List<IElementSettings>();
            IEnumerable<ElementType> elementTypes = _elementService.ListTypes();
            Dictionary<Guid, ElementType> elementTypesById = elementTypes.GroupBy(t => t.ElementTypeId).ToDictionary(t => t.Key, t => t.First());

            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            List<ElementType> availableElementTypes = new List<ElementType>();
            foreach (MasterPageZoneElementType masterPageZoneElementType in masterPageZone.MasterPageZoneElementTypes)
            {
                ElementType elementType = elementTypesById[masterPageZoneElementType.ElementTypeId];
                availableElementTypes.Add(elementType);
            }
            form.FieldSets = GetFieldSets(elementTypesById, pageZone);
            form.NamedFieldSets = GetNamedFieldSets(availableElementTypes);
            form.SubmitLabel = PageResource.PageZoneAdminButtonLabel;

            // Return result
            return form;
        }

        private List<PageZoneElementInfo> GetElements(Form form)
        {
            List<PageZoneElementInfo> elements = new List<PageZoneElementInfo>();
            for (int index = 0; index < form.FieldSets.Count; index++)
            {
                FormFieldSet fieldSet = form.FieldSets[index];
                Guid elementTypeId = new Guid(((SelectListField<string>)fieldSet.Fields["elementType"]).Value);
                long pageZoneElementId = Math.Max(Convert.ToInt64(((TextField)fieldSet.Fields["name"]).Name.Split('_')[1]), 0);
                string name = ((TextField)fieldSet.Fields["name"]).Value;
                elements.Add(new PageZoneElementInfo
                {
                    ElementTypeId = elementTypeId,
                    PageZoneElementId = pageZoneElementId,
                    Name = name,
                    SortOrder = index
                });
            }
            return elements;
        }

        public FormResult PostForm(Form form)
        {
            try
            {
                // Check permissions
                _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

                // Get tenant, page and page zone identifiers
                string[] parts = form.Context.Split('|');
                long pageId = Convert.ToInt64(parts[0]);
                long pageZoneId = Convert.ToInt64(parts[1]);
                long tenantId = _authenticationService.TenantId;

                // Get page zone info
                List<PageZoneElementInfo> pageZoneElements = GetElements(form);

                // Update page zone
                _pageService.UpdateZone(tenantId, pageId, pageZoneId, pageZoneElements);

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
