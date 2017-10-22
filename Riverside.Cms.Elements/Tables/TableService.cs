using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Resources;
using Riverside.Cms.Elements.Resources;
using Riverside.UI.Forms;
using Riverside.Utilities.Data;
using Riverside.Utilities.Text;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Elements.Tables
{
    /// <summary>
    /// Determines table element functionality.
    /// </summary>
    public class TableService : IAdvancedElementService, IFormService
    {
        // Member variables
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IFormHelperService _formHelperService;
        private IStringService _stringService;
        private ITableRepository _tableRepository;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="authenticationService">Retrieve logged on user details.</param>
        /// <param name="authorizationService">Authorization service.</param>
        /// <param name="formHelperService">Form helper.</param>
        /// <param name="stringService">Used to retrieve values from line in a CSV file.</param>
        /// <param name="tableRepository">Table repository.</param>
        public TableService(IAuthenticationService authenticationService, IAuthorizationService authorizationService, IFormHelperService formHelperService, IStringService stringService, ITableRepository tableRepository)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _formHelperService = formHelperService;
            _stringService = stringService;
            _tableRepository = tableRepository;
        }

        /// <summary>
        /// Identifies table elements.
        /// </summary>
        public Guid ElementTypeId
        {
            get
            {
                return new Guid("252ca19c-d085-4e0d-b70b-da3e1098f51b");
            }
        }

        /// <summary>
        /// Creates a new table settings object.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <returns>Newly created table settings object.</returns>
        public IElementSettings New(long tenantId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Column 1,Column 2,Column 3");
            sb.AppendLine();
            sb.Append("Value 1,Value 2,Value 3");
            return new TableSettings
            {
                TenantId = tenantId,
                ElementTypeId = ElementTypeId,
                DisplayName = string.Empty,
                Preamble = string.Empty,
                ShowHeaders = true,
                Rows = sb.ToString()
            };
        }

        /// <summary>
        /// Creates a new element info object.
        /// </summary>
        /// <param name="settings">Element settings.</param>
        /// <param name="content">Element content.</param>
        /// <returns>Element info.</returns>
        public IElementInfo NewInfo(IElementSettings settings, IElementContent content)
        {
            return new ElementInfo<TableSettings, TableContent> { Settings = settings, Content = content };
        }

        /// <summary>
        /// Creates a new table element.
        /// </summary>
        /// <param name="settings">Element settings.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Create(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _tableRepository.Create((TableSettings)settings, unitOfWork);
        }

        /// <summary>
        /// Copies a table element.
        /// </summary>
        /// <param name="sourceTenantId">Source tenant identifier.</param>
        /// <param name="sourceElementId">Source element identifier.</param>
        /// <param name="destTenantId">Destination tenant identifier.</param>
        /// <param name="destElementId">Destination element identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null)
        {
            _tableRepository.Copy(sourceTenantId, sourceElementId, destTenantId, destElementId, unitOfWork);
        }

        /// <summary>
        /// Gets a table element.
        /// </summary>
        /// <param name="settings">Element settings.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Read(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _tableRepository.Read((TableSettings)settings, unitOfWork);
        }

        /// <summary>
        /// Updates a table element.
        /// </summary>
        /// <param name="settings">Element settings.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Update(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _tableRepository.Update((TableSettings)settings, unitOfWork);
        }

        /// <summary>
        /// Deletes a table element.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="elementId">Element identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null)
        {
            _tableRepository.Delete(tenantId, elementId, unitOfWork);
        }

        /// <summary>
        /// Get table content from table settings.
        /// </summary>
        /// <param name="settings">Table settings.</param>
        /// <param name="pageContext">Page context.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Element content.</returns>
        public IElementContent GetContent(IElementSettings settings, IPageContext pageContext, IUnitOfWork unitOfWork = null)
        {
            TableSettings tableSettings = (TableSettings)settings;
            TableContent content = new TableContent { PartialViewName = "Table", Rows = new List<List<string>>() };
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(tableSettings.Rows)))
            {
                using (StreamReader sr = new StreamReader(ms))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        List<string> values = _stringService.GetCsvValues(line);
                        if (tableSettings.ShowHeaders && content.Headers == null)
                            content.Headers = values;
                        else
                            content.Rows.Add(values);
                    }
                }
            }
            return content;
        }

        /// <summary>
        /// Returns identifier of form used to administer table elements.
        /// </summary>
        public Guid FormId { get { return new Guid("252ca19c-d085-4e0d-b70b-da3e1098f51b"); } }

        /// <summary>
        /// Retrieves form for table administration.
        /// </summary>
        /// <param name="context">Form context.</param>
        /// <returns>Form.</returns>
        public Form GetForm(string context)
        {
            // Check permissions
            _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

            // Get page and element identifiers
            string[] parts = context.Split('|');
            long pageId = Convert.ToInt64(parts[0]);
            long elementId = Convert.ToInt64(parts[1]);

            // Get current settings
            Guid elementTypeId = FormId;
            TableSettings settings = (TableSettings)New(_authenticationService.TenantId);
            settings.ElementId = elementId;
            Read(settings);

            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("displayName", new TextField
            {
                Name = "displayName",
                Label = ElementResource.TableDisplayNameLabel,
                MaxLength = TableLengths.DisplayNameMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.TableDisplayNameMaxLengthMessage, "displayName", TableLengths.DisplayNameMaxLength),
                Value = settings.DisplayName
            });
            form.Fields.Add("preamble", new MultiLineTextField
            {
                Name = "preamble",
                Label = ElementResource.TablePreambleLabel,
                Value = settings.Preamble,
                Rows = 2
            });
            form.Fields.Add("showHeaders", new BooleanField
            {
                Name = "showHeaders",
                Label = ElementResource.TableShowHeadersLabel,
                Value = settings.ShowHeaders
            });
            form.Fields.Add("rows", new MultiLineTextField
            {
                Name = "rows",
                Label = ElementResource.TableRowsLabel,
                Value = settings.Rows,
                Rows = 8
            });
            form.SubmitLabel = ElementResource.TableButtonLabel;

            // Return result
            return form;
        }

        /// <summary>
        /// Submits form to update table element.
        /// </summary>
        /// <param name="form">View model containing form definition and submitted values.</param>
        /// <returns>Result of form post.</returns>
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

                // Get updated settings
                TableSettings settings = (TableSettings)New(_authenticationService.TenantId);
                settings.ElementId = elementId;
                settings.DisplayName = string.IsNullOrWhiteSpace(((TextField)form.Fields["displayName"]).Value) ? string.Empty : ((TextField)form.Fields["displayName"]).Value;
                settings.Preamble = string.IsNullOrWhiteSpace(((MultiLineTextField)form.Fields["preamble"]).Value) ? string.Empty : ((MultiLineTextField)form.Fields["preamble"]).Value;
                settings.ShowHeaders = ((BooleanField)form.Fields["showHeaders"]).Value;
                settings.Rows = string.IsNullOrWhiteSpace(((MultiLineTextField)form.Fields["rows"]).Value) ? string.Empty : ((MultiLineTextField)form.Fields["rows"]).Value;

                // Perform the update
                Update(settings);

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
