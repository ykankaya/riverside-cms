using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Cms.Core.MasterPages;
using Riverside.Cms.Core.Resources;
using Riverside.Cms.Core.Uploads;
using Riverside.Utilities.Validation;
using Riverside.UI.Forms;

namespace Riverside.Cms.Core.Pages
{
    /// <summary>
    /// Deals with page form retrieval and submission.
    /// </summary>
    public class PageFormService : IFormService, IFormUploadService
    {
        // Member variables
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IFormHelperService _formHelperService;
        private IMasterPageService _masterPageService;
        private IPagePortalService _pagePortalService;
        private IPageService _pageService;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="authenticationService">Provides access to authentication related code.</param>
        /// <param name="authorizationService">Authorizatoin service.</param>
        /// <param name="formHelperService">Provides access to form helper methods for tasks such as creating form results.</param>
        /// <param name="masterPageService">Provides access to master pages.</param>
        /// <param name="pagePortalService">Page portal service.</param>
        /// <param name="pageService">Provices access to pages.</param>
        public PageFormService(IAuthenticationService authenticationService, IAuthorizationService authorizationService, IFormHelperService formHelperService, IMasterPageService masterPageService, IPagePortalService pagePortalService, IPageService pageService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _formHelperService = formHelperService;
            _masterPageService = masterPageService;
            _pagePortalService = pagePortalService;
            _pageService = pageService;
        }

        /// <summary>
        /// Returns GUID, identifying the form that this form service is associated with.
        /// </summary>
        public Guid FormId { get { return new Guid("ca31a1f4-ce6b-45fc-aa4d-058188acfa35"); } }

        /// <summary>
        /// Retrieves form for creating new page.
        /// </summary>
        /// <param name="context">Form context.</param>
        /// <returns>View model used to render form.</returns>
        private Form GetCreateForm(string context)
        {
            // Get website identifier
            long tenantId = _authenticationService.TenantId;

            // Get type of page to create
            string[] parts = context.Split('|');
            long masterPageId = Convert.ToInt64(parts[1]);

            // Get the master page on which new page will be based
            MasterPage masterPage = _masterPageService.Read(tenantId, masterPageId);

            // Get list of possible parent pages
            List<Page> parentPages = _pageService.ListMasterPageParentPages(masterPage);

            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("name", new TextField
            {
                Name = "name",
                Label = PageResource.NameLabel,
                Value = masterPage.PageName,
                Required = true,
                RequiredErrorMessage = PageResource.NameRequiredMessage,
                MaxLength = PageLengths.NameMaxLength,
                MaxLengthErrorMessage = string.Format(PageResource.NameMaxLengthMessage, "name", PageLengths.NameMaxLength)
            });
            form.Fields.Add("description", new MultiLineTextField
            {
                Name = "description",
                Label = PageResource.DescriptionLabel,
                Rows = 4
            });
            if (parentPages.Count > 1)
            {
                form.Fields.Add("parent", new SelectListField<string>
                {
                    Name = "parent",
                    Label = PageResource.ParentLabel,
                    Required = true,
                    RequiredErrorMessage = PageResource.ParentRequiredMessage,
                    Items = new List<ListFieldItem<string>> { new ListFieldItem<string> { Name = PageResource.SelectParentLabel, Value = null } }
                });
                foreach (Page page in parentPages)
                    ((SelectListField<string>)form.Fields["parent"]).Items.Add(new ListFieldItem<string> { Name = page.Name, Value = page.PageId.ToString() });
            }
            if (masterPage.Taggable)
            {
                form.Fields.Add("tags", new MultiLineTextField
                {
                    Name = "tags",
                    Label = PageResource.TagsLabel,
                    Rows = 4
                });
            }
            if (masterPage.HasImage)
            {
                form.Fields.Add("upload", new UploadField
                {
                    Name = "upload",
                    Label = PageResource.UploadLabel
                });
            }
            form.SubmitLabel = string.Format(PageResource.CreatePageButtonLabel, masterPage.Name.ToLower());

            // Return result
            return form;
        }

        /// <summary>
        /// Handles form upload.
        /// </summary>
        /// <param name="masterPageId">Master page with page image rules.</param>
        /// <param name="name">The name of the upload (e.g. "MyImage.png").</param>
        /// <param name="contentType">The type of the upload content (e.g. "image/png").</param>
        /// <param name="content">Byte buffer containing uploaded content.</param>
        /// <returns>Result of form upload post.</returns>
        private FormResult PostPageUpload(long masterPageId, string name, string contentType, byte[] content)
        {
            // Get tenant identifier, website identifier
            long tenantId = _authenticationService.TenantId;

            // Get upload model
            CreateUploadModel model = new CreateUploadModel
            {
                Content = content,
                ContentType = contentType,
                Name = name,
                TenantId = tenantId
            };

            // Create uploads, ready to be assigned to page when form submitted
            ImageUploadIds uploadIds = _pageService.PrepareImages(tenantId, masterPageId, model);

            // Return form result
            string status = string.Format("{0}|{1}|{2}", uploadIds.ThumbnailImageUploadId, uploadIds.PreviewImageUploadId, uploadIds.ImageUploadId);
            return _formHelperService.GetFormResult(status);
        }

        /// <summary>
        /// Handles form upload.
        /// </summary>
        /// <param name="id">Form identifier.</param>
        /// <param name="context">Form context.</param>
        /// <param name="name">The name of the upload (e.g. "MyImage.png").</param>
        /// <param name="contentType">The type of the upload content (e.g. "image/png").</param>
        /// <param name="content">Byte buffer containing uploaded content.</param>
        /// <returns>Result of form upload post.</returns>
        private FormResult PostCreateUpload(string id, string context, string name, string contentType, byte[] content)
        {
            string[] parts = context.Split('|');
            long masterPageId = Convert.ToInt64(parts[1]);
            return PostPageUpload(masterPageId, name, contentType, content);
        }

        /// <summary>
        /// Submits create page form.
        /// </summary>
        /// <param name="form">View model containing form definition and submitted values.</param>
        /// <returns>Result of form post.</returns>
        private FormResult PostCreateForm(Form form)
        {
            // Get website identifier
            long tenantId = _authenticationService.TenantId;

            // Get type of page to create
            string[] parts = form.Context.Split('|');
            long masterPageId = Convert.ToInt64(parts[1]);

            // Get parent page identifier, if one is specified
            long? parentPageId = null;
            if (form.Fields.ContainsKey("parent"))
                parentPageId = Convert.ToInt64(((SelectListField<string>)form.Fields["parent"]).Value);

            // Get supplied page information
            string tags = form.Fields.ContainsKey("tags") ? ((MultiLineTextField)form.Fields["tags"]).Value : null;
            PageInfo pageInfo = new PageInfo
            {
                Name = ((TextField)form.Fields["name"]).Value,
                Description = ((MultiLineTextField)form.Fields["description"]).Value,
                Tags = _pagePortalService.GetTagsFromTextString(new string[] { "\r\n", "\n" }, tags)
            };

            // Get master page, which will be used to determine if image uploads are allowed
            MasterPage masterPage = _masterPageService.Read(tenantId, masterPageId);

            // Get upload identifiers for thumbnail, preview and source images
            if (masterPage.HasImage)
            {
                string uploadIds = ((UploadField)form.Fields["upload"]).Value;
                if (!string.IsNullOrWhiteSpace(uploadIds))
                {
                    string[] uploadParts = uploadIds.Split('|');
                    pageInfo.ImageTenantId = tenantId;
                    pageInfo.ThumbnailImageUploadId = Convert.ToInt64(uploadParts[0]);
                    pageInfo.PreviewImageUploadId = Convert.ToInt64(uploadParts[1]);
                    pageInfo.ImageUploadId = Convert.ToInt64(uploadParts[2]);
                }
            }

            // Create page
            long pageId = _pageService.Create(tenantId, parentPageId, masterPageId, pageInfo);

            // Return form result
            string status = _pagePortalService.GetPageUrl(pageId, pageInfo.Name);
            return _formHelperService.GetFormResult(status);
        }

        /// <summary>
        /// Retrieves form.
        /// </summary>
        /// <param name="context">Form context.</param>
        /// <returns>View model used to render form.</returns>
        private Form GetUpdateForm(string context)
        {
            // Get website identifier
            long tenantId = _authenticationService.TenantId;

            // Get identifier of page that is being updated
            string[] parts = context.Split('|');
            long pageId = Convert.ToInt64(parts[1]);

            // Get existing page details
            Page page = _pageService.Read(tenantId, pageId);

            // Get master page, which will be used to determine if the image upload field should be displayed
            MasterPage masterPage = _masterPageService.Read(tenantId, page.MasterPageId);

            // Construct view model
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("name", new TextField
            {
                Name = "name",
                Label = PageResource.NameLabel,
                Value = page.Name,
                Required = true,
                RequiredErrorMessage = PageResource.NameRequiredMessage,
                MaxLength = PageLengths.NameMaxLength,
                MaxLengthErrorMessage = string.Format(PageResource.NameMaxLengthMessage, "name", PageLengths.NameMaxLength)
            });
            form.Fields.Add("description", new MultiLineTextField
            {
                Name = "description",
                Label = PageResource.DescriptionLabel,
                Value = page.Description,
                Rows = 4
            });
            if (masterPage.Taggable)
            {
                string tags = _pagePortalService.GetTagsAsTextString(Environment.NewLine, page.Tags);
                form.Fields.Add("tags", new MultiLineTextField
                {
                    Name = "tags",
                    Label = PageResource.TagsLabel,
                    Value = tags,
                    Rows = 4
                });
            }
            if (masterPage.HasImage)
            {
                form.Fields.Add("upload", new UploadField
                {
                    Name = "upload",
                    Label = PageResource.UploadLabel
                });
            }
            form.SubmitLabel = PageResource.UpdatePageButtonLabel;

            // Return form
            return form;
        }

        /// <summary>
        /// Handles form upload.
        /// </summary>
        /// <param name="id">Form identifier.</param>
        /// <param name="context">Form context.</param>
        /// <param name="name">The name of the upload (e.g. "MyImage.png").</param>
        /// <param name="contentType">The type of the upload content (e.g. "image/png").</param>
        /// <param name="content">Byte buffer containing uploaded content.</param>
        /// <returns>Result of form upload post.</returns>
        private FormResult PostUpdateUpload(string id, string context, string name, string contentType, byte[] content)
        {
            // Get tenant identifier, website identifier
            long tenantId = _authenticationService.TenantId;

            // Get identifier of the page we are updating
            string[] parts = context.Split('|');
            long pageId = Convert.ToInt64(parts[1]);

            // Get page in order to determine master page
            Page page = _pageService.Read(tenantId, pageId);

            // Return form result
            return PostPageUpload(page.MasterPageId, name, contentType, content);
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

            // Get identifier of the page we are updating
            string[] parts = form.Context.Split('|');
            long pageId = Convert.ToInt64(parts[1]);

            // Get fully populated page details with new name, description and tags
            Page page = _pageService.Read(tenantId, pageId);
            page.Name = ((TextField)form.Fields["name"]).Value;
            page.Description = ((MultiLineTextField)form.Fields["description"]).Value;
            string tags = form.Fields.ContainsKey("tags") ? ((MultiLineTextField)form.Fields["tags"]).Value : null;
            page.Tags = _pagePortalService.GetTagsFromTextString(new string[] { "\r\n", "\n" }, tags);

            // Get master page, which will be used to determine if image uploads are allowed
            MasterPage masterPage = _masterPageService.Read(tenantId, page.MasterPageId);

            // Get upload identifiers for thumbnail, preview and source images
            if (masterPage.HasImage)
            {
                string uploadIds = ((UploadField)form.Fields["upload"]).Value;
                if (!string.IsNullOrWhiteSpace(uploadIds))
                {
                    string[] uploadParts = uploadIds.Split('|');
                    page.ImageTenantId = tenantId;
                    page.ThumbnailImageUploadId = Convert.ToInt64(uploadParts[0]);
                    page.PreviewImageUploadId = Convert.ToInt64(uploadParts[1]);
                    page.ImageUploadId = Convert.ToInt64(uploadParts[2]);
                }
            }

            // Do the update
            _pageService.Update(page);

            // Return form result with no errors
            string status = page.ParentPageId == null ? _pagePortalService.GetHomeUrl() : _pagePortalService.GetPageUrl(pageId, page.Name);
            return _formHelperService.GetFormResult(status);
        }

        /// <summary>
        /// Retrieves form.
        /// </summary>
        /// <param name="context">Form context.</param>
        /// <returns>View model used to render form.</returns>
        public Form GetForm(string context)
        {
            // The form that we will return
            Form form = null;

            // Get action from context
            string action = context.Split('|')[0];

            // Get the correct form based on action
            switch (action)
            {
                case "create":
                    _authorizationService.AuthorizeUserForFunction(Functions.CreatePages);
                    form = GetCreateForm(context);
                    break;

                case "update":
                    _authorizationService.AuthorizeUserForFunction(Functions.UpdatePages);
                    form = GetUpdateForm(context);
                    break;
            }

            // Return the form
            return form;
        }

        /// <summary>
        /// Handles form upload.
        /// </summary>
        /// <param name="id">Form identifier.</param>
        /// <param name="context">Form context.</param>
        /// <param name="name">The name of the upload (e.g. "MyImage.png").</param>
        /// <param name="contentType">The type of the upload content (e.g. "image/png").</param>
        /// <param name="content">Byte buffer containing uploaded content.</param>
        /// <returns>Result of form upload post.</returns>
        public FormResult PostUpload(string id, string context, string name, string contentType, byte[] content)
        {
            try
            {
                // The form result
                FormResult formResult = null;

                // Split context into different parts
                string action = context.Split('|')[0];

                // Perform the correct action based on form context
                switch (action)
                {
                    case "create":
                        _authorizationService.AuthorizeUserForFunction(Functions.CreatePages);
                        formResult = PostCreateUpload(id, context, name, contentType, content);
                        break;

                    case "update":
                        _authorizationService.AuthorizeUserForFunction(Functions.UpdatePages);
                        formResult = PostUpdateUpload(id, context, name, contentType, content);
                        break;
                }

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

        /// <summary>
        /// Submits page management form.
        /// </summary>
        /// <param name="form">View model containing form definition and submitted values.</param>
        /// <returns>Result of form post.</returns>
        public FormResult PostForm(Form form)
        {
            try
            {
                // The form result
                FormResult formResult = null;

                // Split context into different parts
                string action = form.Context.Split('|')[0];

                // Perform the correct action based on form context
                switch (action)
                {
                    case "create":
                        _authorizationService.AuthorizeUserForFunction(Functions.CreatePages);
                        formResult = PostCreateForm(form);
                        break;

                    case "update":
                        _authorizationService.AuthorizeUserForFunction(Functions.UpdatePages);
                        formResult = PostUpdateForm(form);
                        break;
                }

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
