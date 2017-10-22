using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Resources;
using Riverside.Cms.Elements.Resources;
using Riverside.UI.Forms;
using Riverside.UI.Web;
using Riverside.Utilities.Data;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Elements.Testimonials
{
    /// <summary>
    /// Implements custom functionality for the testimonials element.
    /// </summary>
    public class TestimonialService : ITestimonialService
    {
        // Member variables
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IFormHelperService _formHelperService;
        private ITestimonialRepository _testimonialRepository;
        private IWebHelperService _webHelperService;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="authenticationService">Provides access to authentication features.</param>
        /// <param name="authorizationService">Authorization service.</param>
        /// <param name="formHelperService">Provides assistance with forms.</param>
        /// <param name="testimonialRepository">Repository for administering testimonials in underlying storage.</param>
        /// <param name="webHelperService">Web helper service.</param>
        public TestimonialService(IAuthenticationService authenticationService, IAuthorizationService authorizationService, IFormHelperService formHelperService, ITestimonialRepository testimonialRepository, IWebHelperService webHelperService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _formHelperService = formHelperService;
            _testimonialRepository = testimonialRepository;
            _webHelperService = webHelperService;
        }

        /// <summary>
        /// Returns GUID, identifying the type of element that this custom element service is associated with.
        /// </summary>
        public Guid ElementTypeId
        {
            get
            {
                return new Guid("eb479ac9-8c79-4fae-817a-e77fd3dbf05b");
            }
        }

        /// <summary>
        /// Creates a new instance of a type of element.
        /// </summary>
        /// <param name="tenantId">Identifies the tenant that newly created element settings belong to.</param>
        /// <returns>Newly created element instance.</returns>
        public IElementSettings New(long tenantId)
        {
            return new TestimonialSettings
            {
                TenantId = tenantId,
                ElementTypeId = ElementTypeId,
                DisplayName = string.Empty,
                Preamble = string.Empty,
                Comments = new List<TestimonialComment>() {
                    new TestimonialComment {
                        TenantId    = tenantId,
                        Author      = ElementResource.TestimonialDefaultAuthor,
                        AuthorTitle = ElementResource.TestimonialDefaultAuthorTitle,
                        Comment     = ElementResource.TestimonialDefaultComment,
                        CommentDate = ElementResource.TestimonialDefaultCommentDate
                    }
                }
            };
        }

        /// <summary>
        /// Creates and returns strongly typed element info instance, populated with supplied element settings and content.
        /// </summary>
        /// <param name="settings">Element settings.</param>
        /// <param name="content">Element content.</param>
        /// <returns>An element info object containing settings and content.</returns>
        public IElementInfo NewInfo(IElementSettings settings, IElementContent content)
        {
            return new ElementInfo<TestimonialSettings, TestimonialContent> { Settings = settings, Content = content };
        }

        /// <summary>
        /// Creates a new element.
        /// </summary>
        /// <param name="settings">New element settings.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Create(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _testimonialRepository.Create((TestimonialSettings)settings, unitOfWork);
        }

        /// <summary>
        /// Copies testimonial specific data from source element to destination element.
        /// </summary>
        /// <param name="sourceTenantId">Source tenant identifier.</param>
        /// <param name="sourceElementId">Identifies source element.</param>
        /// <param name="destTenantId">Destination tenant identifier.</param>
        /// <param name="destElementId">Identifies destination element.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null)
        {
            _testimonialRepository.Copy(sourceTenantId, sourceElementId, destTenantId, destElementId, unitOfWork);
        }

        /// <summary>
        /// Populates element settings.
        /// </summary>
        /// <param name="settings">Element settings to be populated.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Read(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _testimonialRepository.Read((TestimonialSettings)settings, unitOfWork);
        }

        /// <summary>
        /// Updates an element's details.
        /// </summary>
        /// <param name="settings">Updated element settings.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Update(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            // Ensure sort orders are correct
            TestimonialSettings testimonialSettings = (TestimonialSettings)settings;
            for (int index = 0; index < testimonialSettings.Comments.Count; index++)
            {
                TestimonialComment testimonialComment = testimonialSettings.Comments[index];
                testimonialComment.SortOrder = index;
                testimonialComment.AuthorTitle = string.IsNullOrWhiteSpace(testimonialComment.AuthorTitle) ? string.Empty : testimonialComment.AuthorTitle.Trim();
                testimonialComment.CommentDate = string.IsNullOrWhiteSpace(testimonialComment.CommentDate) ? string.Empty : testimonialComment.CommentDate.Trim();
            }

            // Do the update
            _testimonialRepository.Update((TestimonialSettings)settings, unitOfWork);
        }

        /// <summary>
        /// Deletes an element.
        /// </summary>
        /// <param name="tenantId">The tenant that element to delete belongs to.</param>
        /// <param name="elementId">Identifies the element to delete.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null)
        {
            _testimonialRepository.Delete(tenantId, elementId, unitOfWork);
        }

        /// <summary>
        /// Retrieves dynamic element content.
        /// </summary>
        /// <param name="settings">Contains element settings.</param>
        /// <param name="pageContext">Page context.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Element content.</returns>
        public IElementContent GetContent(IElementSettings settings, IPageContext pageContext, IUnitOfWork unitOfWork = null)
        {
            IDictionary<object, object> items = _webHelperService.GetItems();
            return new TestimonialContent { PartialViewName = "Testimonial", Items = items };
        }

        /// <summary>
        /// Returns GUID, identifying the form that this form service is associated with.
        /// </summary>
        public Guid FormId
        {
            get
            {
                return new Guid("eb479ac9-8c79-4fae-817a-e77fd3dbf05b");
            }
        }

        /// <summary>
        /// Retrieves form for creating or updating testimonial comments.
        /// </summary>
        /// <param name="context">Form context.</param>
        /// <returns>Form object.</returns>
        private Form GetTestimonialCommentForm(string context)
        {
            // Construct form data, consisting of testimonial comment form action button labels
            TestimonialCommentFormData formData = new TestimonialCommentFormData { Labels = new Dictionary<string, string>() };
            formData.Labels.Add("update", ElementResource.TestimonialUpdateCommentButtonLabel);
            formData.Labels.Add("create", ElementResource.TestimonialCreateCommentButtonLabel);
            formData.TestimonialComment = new TestimonialComment();

            // Construct form
            string data = JsonConvert.SerializeObject(formData);
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context, Data = data };
            form.Fields.Add("comment", new MultiLineTextField
            {
                Name = "comment",
                Label = ElementResource.TestimonialCommentLabel,
                Required = true,
                RequiredErrorMessage = ElementResource.TestimonialCommentRequiredMessage,
                Rows = 4
            });
            form.Fields.Add("author", new TextField
            {
                Name = "author",
                Label = ElementResource.TestimonialAuthorLabel,
                Required = true,
                RequiredErrorMessage = ElementResource.TestimonialAuthorRequiredMessage,
                MaxLength = TestimonialLengths.AuthorMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.TestimonialAuthorMaxLengthMessage, "author", TestimonialLengths.AuthorMaxLength)
            });
            form.Fields.Add("authorTitle", new TextField
            {
                Name = "authorTitle",
                Label = ElementResource.TestimonialAuthorTitleLabel,
                MaxLength = TestimonialLengths.AuthorTitleMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.TestimonialAuthorTitleMaxLengthMessage, "authorTitle", TestimonialLengths.AuthorTitleMaxLength)
            });
            form.Fields.Add("commentDate", new TextField
            {
                Name = "commentDate",
                Label = ElementResource.TestimonialCommentDateLabel,
                MaxLength = TestimonialLengths.CommentDateMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.TestimonialCommentDateMaxLengthMessage, "commentDate", TestimonialLengths.CommentDateMaxLength)
            });

            // Return result
            return form;
        }

        /// <summary>
        /// Retrieves form for testimonial update.
        /// </summary>
        /// <param name="context">Form context.</param>
        /// <returns>Form object.</returns>
        private Form GetTestimonialForm(string context)
        {
            // Get tenant identifier
            long tenantId = _authenticationService.TenantId;

            // Get page and element identifiers
            string[] parts = context.Split('|');
            long pageId = Convert.ToInt64(parts[0]);
            long elementId = Convert.ToInt64(parts[1]);

            // Get current settings
            Guid elementTypeId = FormId;
            TestimonialSettings settings = (TestimonialSettings)New(_authenticationService.TenantId);
            settings.ElementId = elementId;
            Read(settings);

            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context, SubmitLabel = ElementResource.TestimonialUpdateButtonLabel };
            form.Fields.Add("displayName", new TextField
            {
                Name = "displayName",
                Label = ElementResource.TestimonialDisplayNameLabel,
                MaxLength = TestimonialLengths.DisplayNameMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.TestimonialDisplayNameMaxLengthMessage, "displayName", TestimonialLengths.DisplayNameMaxLength)
            });
            form.Fields.Add("preamble", new MultiLineTextField
            {
                Name = "preamble",
                Label = ElementResource.TestimonialPreambleLabel,
                Rows = 4
            });

            // Create sub forms
            form.SubForms = new Dictionary<string, Form>();
            form.SubForms.Add("testimonialComment", GetTestimonialCommentForm(context));

            // Set testimonial settings as form data
            form.Data = JsonConvert.SerializeObject(settings);

            // Return form
            return form;
        }

        /// <summary>
        /// Retrieves form.
        /// </summary>
        /// <param name="context">Form context.</param>
        /// <returns>View model used to render form.</returns>
        public Form GetForm(string context)
        {
            // Check permissions
            _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

            // The form that we will return
            return GetTestimonialForm(context);
        }

        /// <summary>
        /// Performs update of testimonial given submitted form data.
        /// </summary>
        /// <param name="form">Form containing updated testimonial data.</param>
        /// <returns>Result of form post.</returns>
        private FormResult UpdateTestimonial(Form form)
        {
            // Get master page details
            string[] parts = form.Context.Split('|');
            long tenantId = _authenticationService.TenantId;
            long pageId = Convert.ToInt64(parts[0]);
            long elementId = Convert.ToInt64(parts[1]);
            TestimonialSettings settings = JsonConvert.DeserializeObject<TestimonialSettings>(form.Data);
            settings.TenantId = tenantId;
            settings.ElementId = elementId;

            // Do the update
            Update(settings);

            // Return form result with no errors
            return _formHelperService.GetFormResult();
        }

        /// <summary>
        /// Submits form.
        /// </summary>
        /// <param name="form">View model containing form definition and submitted values.</param>
        /// <returns>Result of form post.</returns>
        public FormResult PostForm(Form form)
        {
            try
            {
                // Check permissions
                _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

                // The form result
                FormResult formResult = UpdateTestimonial(form);

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
