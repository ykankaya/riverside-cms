using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Resources;
using Riverside.Cms.Elements.Resources;
using Riverside.Utilities.Annotations;
using Riverside.Utilities.Validation;
using Riverside.UI.Forms;

namespace Riverside.Cms.Elements.Forums
{
    public class ForumFormService : IFormService
    {
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IDataAnnotationsService _dataAnnotationsService;
        private IFormHelperService _formHelperService;
        private IForumService _forumService;
        private IForumUrlService _forumUrlService;
        private IPageService _pageService;

        public ForumFormService(IAuthenticationService authenticationService, IAuthorizationService authorizationService, IDataAnnotationsService dataAnnotationsService, IFormHelperService formHelperService, IForumService forumService, IForumUrlService forumUrlService, IPageService pageService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _dataAnnotationsService = dataAnnotationsService;
            _formHelperService = formHelperService;
            _forumService = forumService;
            _forumUrlService = forumUrlService;
            _pageService = pageService;
        }

        public Guid FormId { get { return new Guid("484192d1-5a4f-496f-981b-7e0120453949"); } }

        private Form GetCreateThreadForm(string context)
        {
            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("subject", new TextField
            {
                Name = "subject",
                Label = ElementResource.ForumSubjectLabel,
                MaxLength = ForumLengths.SubjectMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.ForumSubjectMaxLengthMessage, "subject", ForumLengths.SubjectMaxLength),
                Required = true,
                RequiredErrorMessage = ElementResource.ForumSubjectRequiredMessage
            });
            form.Fields.Add("message", new MultiLineTextField
            {
                Name = "message",
                Label = ElementResource.ForumMessageLabel,
                Required = true,
                RequiredErrorMessage = ElementResource.ForumMessageRequiredMessage,
                Rows = 10
            });
            form.Fields.Add("notify", new BooleanField
            {
                Name = "notify",
                Label = ElementResource.ForumNotifyLabel
            });
            form.SubmitLabel = ElementResource.ForumCreateThreadButtonLabel;

            // Return result
            return form;
        }

        private FormResult PostCreateThreadForm(Form form)
        {
            // Get logged on user details
            long tenantId = _authenticationService.TenantId;
            long userId = _authenticationService.GetCurrentUser().User.UserId;

            // Get page and element identifiers
            string[] parts = form.Context.Split('|');
            long pageId = Convert.ToInt64(parts[1]);
            long elementId = Convert.ToInt64(parts[2]);

            // Get information required to create new thread
            CreateThreadInfo info = new CreateThreadInfo
            {
                ElementId = elementId,
                Message = ((MultiLineTextField)form.Fields["message"]).Value,
                Notify = ((BooleanField)form.Fields["notify"]).Value,
                Subject = ((TextField)form.Fields["subject"]).Value,
                UserId = userId,
                TenantId = tenantId
            };

            // Create new thread
            long threadId = _forumService.CreateThread(info);

            // Return form result with no errors
            string status = _forumUrlService.GetThreadUrl(pageId, threadId, info.Subject);
            return _formHelperService.GetFormResult(status);
        }

        private Form GetUpdateThreadForm(string context)
        {
            // Get page, element and thread identifiers
            string[] parts = context.Split('|');
            long pageId = Convert.ToInt64(parts[1]);
            long elementId = Convert.ToInt64(parts[2]);
            long threadId = Convert.ToInt64(parts[3]);

            // Get existing thread details
            ForumThread forumThread = _forumService.GetThread(_authenticationService.TenantId, elementId, threadId);

            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("subject", new TextField
            {
                Name = "subject",
                Label = ElementResource.ForumSubjectLabel,
                MaxLength = ForumLengths.SubjectMaxLength,
                MaxLengthErrorMessage = string.Format(ElementResource.ForumSubjectMaxLengthMessage, "subject", ForumLengths.SubjectMaxLength),
                Required = true,
                RequiredErrorMessage = ElementResource.ForumSubjectRequiredMessage,
                Value = forumThread.Subject
            });
            form.Fields.Add("message", new MultiLineTextField
            {
                Name = "message",
                Label = ElementResource.ForumMessageLabel,
                Required = true,
                RequiredErrorMessage = ElementResource.ForumMessageRequiredMessage,
                Rows = 10,
                Value = forumThread.Message
            });
            form.Fields.Add("notify", new BooleanField
            {
                Name = "notify",
                Label = ElementResource.ForumNotifyLabel,
                Value = forumThread.Notify
            });
            form.SubmitLabel = ElementResource.ForumUpdateThreadButtonLabel;

            // Return result
            return form;
        }

        private FormResult PostUpdateThreadForm(Form form)
        {
            // Get logged on user details
            long tenantId = _authenticationService.TenantId;
            long userId = _authenticationService.GetCurrentUser().User.UserId;

            // Get page, element and thread identifiers
            string[] parts = form.Context.Split('|');
            long pageId = Convert.ToInt64(parts[1]);
            long elementId = Convert.ToInt64(parts[2]);
            long threadId = Convert.ToInt64(parts[3]);

            // Get information required to update existing thread
            UpdateThreadInfo info = new UpdateThreadInfo
            {
                ElementId = elementId,
                Message = ((MultiLineTextField)form.Fields["message"]).Value,
                Notify = ((BooleanField)form.Fields["notify"]).Value,
                Subject = ((TextField)form.Fields["subject"]).Value,
                TenantId = tenantId,
                ThreadId = threadId,
                UserId = userId
            };

            // Update existing thread
            _forumService.UpdateThread(info);

            // Return form result with no errors
            string status = _forumUrlService.GetThreadUrl(pageId, threadId, info.Subject);
            return _formHelperService.GetFormResult(status);
        }

        private Form GetReplyThreadForm(string context)
        {
            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("message", new MultiLineTextField
            {
                Name = "message",
                Label = ElementResource.ForumMessageLabel,
                Required = true,
                RequiredErrorMessage = ElementResource.ForumMessageRequiredMessage,
                Rows = 10
            });
            form.SubmitLabel = ElementResource.ForumReplyThreadButtonLabel;

            // Return result
            return form;
        }

        private FormResult PostReplyQuoteThreadForm(Form form)
        {
            // Get logged on user details
            long tenantId = _authenticationService.TenantId;
            long userId = _authenticationService.GetCurrentUser().User.UserId;

            // Get page, element and thread identifiers
            string[] parts = form.Context.Split('|');
            long pageId = Convert.ToInt64(parts[1]);
            long elementId = Convert.ToInt64(parts[2]);
            long threadId = Convert.ToInt64(parts[3]);

            // Get existing thread details
            ForumThread forumThread = _forumService.GetThread(tenantId, elementId, threadId);

            // Get information required to create post
            CreatePostInfo info = new CreatePostInfo
            {
                ElementId = elementId,
                Message = ((MultiLineTextField)form.Fields["message"]).Value,
                ParentPostId = null,
                ThreadId = threadId,
                UserId = userId,
                TenantId = tenantId
            };

            // Update existing thread
            long postId = _forumService.CreatePost(info);

            // Get thread page that post is on
            int page = _forumService.GetThreadPage(tenantId, info.ElementId, info.ThreadId, postId);

            // Return form result with no errors
            string status = _forumUrlService.GetThreadUrl(pageId, threadId, forumThread.Subject, page);
            return _formHelperService.GetFormResult(status);
        }

        private Form GetQuoteThreadForm(string context)
        {
            // Get page, element and thread identifiers
            string[] parts = context.Split('|');
            long pageId = Convert.ToInt64(parts[1]);
            long elementId = Convert.ToInt64(parts[2]);
            long threadId = Convert.ToInt64(parts[3]);

            // Get existing thread details
            ForumThreadAndUser forumThreadAndUser = _forumService.GetThreadAndUser(_authenticationService.TenantId, elementId, threadId);

            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("message", new MultiLineTextField
            {
                Name = "message",
                Label = ElementResource.ForumMessageLabel,
                Required = true,
                RequiredErrorMessage = ElementResource.ForumMessageRequiredMessage,
                Rows = 10,
                Value = _forumService.GetQuoteMessage(forumThreadAndUser.Thread.Message, forumThreadAndUser.User.Alias)
            });
            form.SubmitLabel = ElementResource.ForumQuoteThreadButtonLabel;

            // Return result
            return form;
        }

        private Form GetDeleteThreadForm(string context)
        {
            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.SubmitLabel = ElementResource.ForumDeleteThreadButtonLabel;

            // Return result
            return form;
        }

        private FormResult PostDeleteThreadForm(Form form)
        {
            // Get logged on user details
            long tenantId = _authenticationService.TenantId;
            long userId = _authenticationService.GetCurrentUser().User.UserId;

            // Get page, element and thread identifiers
            string[] parts = form.Context.Split('|');
            long pageId = Convert.ToInt64(parts[1]);
            long elementId = Convert.ToInt64(parts[2]);
            long threadId = Convert.ToInt64(parts[3]);

            // Get existing thread details
            Page page = _pageService.Read(tenantId, pageId);

            // Get information required to delete thread
            DeleteThreadInfo info = new DeleteThreadInfo
            {
                ElementId = elementId,
                TenantId = tenantId,
                ThreadId = threadId,
                UserId = userId
            };

            // Delete thread
            _forumService.DeleteThread(info);

            // Return form result with no errors
            string status = _forumUrlService.GetForumUrl(pageId, page.Name);
            return _formHelperService.GetFormResult(status);
        }

        private Form GetReplyPostForm(string context)
        {
            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("message", new MultiLineTextField
            {
                Name = "message",
                Label = ElementResource.ForumMessageLabel,
                Required = true,
                RequiredErrorMessage = ElementResource.ForumMessageRequiredMessage,
                Rows = 10
            });
            form.SubmitLabel = ElementResource.ForumReplyPostButtonLabel;

            // Return result
            return form;
        }

        private Form GetQuotePostForm(string context)
        {
            // Get page, element, thread and post identifiers
            string[] parts = context.Split('|');
            long pageId = Convert.ToInt64(parts[1]);
            long elementId = Convert.ToInt64(parts[2]);
            long threadId = Convert.ToInt64(parts[3]);
            long postId = Convert.ToInt64(parts[4]);

            // Get existing post details
            ForumPostAndUser forumPostAndUser = _forumService.GetPostAndUser(_authenticationService.TenantId, elementId, threadId, postId);

            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("message", new MultiLineTextField
            {
                Name = "message",
                Label = ElementResource.ForumMessageLabel,
                Required = true,
                RequiredErrorMessage = ElementResource.ForumMessageRequiredMessage,
                Rows = 10,
                Value = _forumService.GetQuoteMessage(forumPostAndUser.Post.Message, forumPostAndUser.User.Alias)
            });
            form.SubmitLabel = ElementResource.ForumQuoteThreadButtonLabel;

            // Return result
            return form;
        }

        private FormResult PostReplyQuotePostForm(Form form)
        {
            // Get logged on user details
            long tenantId = _authenticationService.TenantId;
            long userId = _authenticationService.GetCurrentUser().User.UserId;

            // Get page, element and thread identifiers
            string[] parts = form.Context.Split('|');
            long pageId = Convert.ToInt64(parts[1]);
            long elementId = Convert.ToInt64(parts[2]);
            long threadId = Convert.ToInt64(parts[3]);
            long postId = Convert.ToInt64(parts[4]);

            // Get existing thread details
            ForumThread forumThread = _forumService.GetThread(tenantId, elementId, threadId);

            // Get information required to create post
            CreatePostInfo info = new CreatePostInfo
            {
                ElementId = elementId,
                Message = ((MultiLineTextField)form.Fields["message"]).Value,
                ParentPostId = postId,
                TenantId = tenantId,
                ThreadId = threadId,
                UserId = userId,
            };

            // Create new post
            long newPostId = _forumService.CreatePost(info);

            // Get thread page that new post is on
            int page = _forumService.GetThreadPage(tenantId, info.ElementId, info.ThreadId, newPostId);

            // Return form result with no errors
            string status = _forumUrlService.GetThreadUrl(pageId, threadId, forumThread.Subject, page);
            return _formHelperService.GetFormResult(status);
        }

        private Form GetUpdatePostForm(string context)
        {
            // Get page, element and thread identifiers
            string[] parts = context.Split('|');
            long pageId = Convert.ToInt64(parts[1]);
            long elementId = Convert.ToInt64(parts[2]);
            long threadId = Convert.ToInt64(parts[3]);
            long postId = Convert.ToInt64(parts[4]);

            // Get existing post details
            ForumPost forumPost = _forumService.GetPost(_authenticationService.TenantId, elementId, threadId, postId);

            // Construct form
            Form form = new Form { Fields = new Dictionary<string, IFormField>(), Id = FormId.ToString(), Context = context };
            form.Fields.Add("message", new MultiLineTextField
            {
                Name = "message",
                Label = ElementResource.ForumMessageLabel,
                Required = true,
                RequiredErrorMessage = ElementResource.ForumMessageRequiredMessage,
                Rows = 10,
                Value = forumPost.Message
            });
            form.SubmitLabel = ElementResource.ForumUpdatePostButtonLabel;

            // Return result
            return form;
        }

        private FormResult PostUpdatePostForm(Form form)
        {
            // Get logged on user details
            long tenantId = _authenticationService.TenantId;
            long userId = _authenticationService.GetCurrentUser().User.UserId;

            // Get page, element and thread identifiers
            string[] parts = form.Context.Split('|');
            long pageId = Convert.ToInt64(parts[1]);
            long elementId = Convert.ToInt64(parts[2]);
            long threadId = Convert.ToInt64(parts[3]);
            long postId = Convert.ToInt64(parts[4]);

            // Get existing thread details
            ForumThread forumThread = _forumService.GetThread(tenantId, elementId, threadId);

            // Get information required to update post
            UpdatePostInfo info = new UpdatePostInfo
            {
                ElementId = elementId,
                Message = ((MultiLineTextField)form.Fields["message"]).Value,
                PostId = postId,
                TenantId = tenantId,
                ThreadId = threadId,
                UserId = userId
            };

            // Update post
            _forumService.UpdatePost(info);

            // Get thread page that new post is on
            int page = _forumService.GetThreadPage(tenantId, info.ElementId, info.ThreadId, postId);

            // Return form result with no errors
            string status = _forumUrlService.GetThreadUrl(pageId, threadId, forumThread.Subject, page);
            return _formHelperService.GetFormResult(status);
        }

        public Form GetForm(string context)
        {
            // Check permissions
            _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

            // The form that we will return
            Form form = null;

            // Get action from context
            string action = context.Split('|')[0];

            // Get the correct form based on action
            switch (action)
            {
                case "createthread":
                    form = GetCreateThreadForm(context);
                    break;

                case "updatethread":
                    form = GetUpdateThreadForm(context);
                    break;

                case "replythread":
                    form = GetReplyThreadForm(context);
                    break;

                case "quotethread":
                    form = GetQuoteThreadForm(context);
                    break;

                case "replypost":
                    form = GetReplyPostForm(context);
                    break;

                case "quotepost":
                    form = GetQuotePostForm(context);
                    break;

                case "updatepost":
                    form = GetUpdatePostForm(context);
                    break;

                case "deletethread":
                    form = GetDeleteThreadForm(context);
                    break;
            }

            // Return the form
            return form;
        }

        public FormResult PostForm(Form form)
        {
            try
            {
                // Check permissions
                _authorizationService.AuthorizeUserForFunction(Functions.UpdatePageElements);

                // The form result
                FormResult formResult = null;

                // Split context into different parts
                string action = form.Context.Split('|')[0];

                // Perform the correct action based on form context
                switch (action)
                {
                    case "createthread":
                        formResult = PostCreateThreadForm(form);
                        break;

                    case "updatethread":
                        formResult = PostUpdateThreadForm(form);
                        break;

                    case "replythread":
                    case "quotethread":
                        formResult = PostReplyQuoteThreadForm(form);
                        break;

                    case "replypost":
                    case "quotepost":
                        formResult = PostReplyQuotePostForm(form);
                        break;

                    case "updatepost":
                        formResult = PostUpdatePostForm(form);
                        break;

                    case "deletethread":
                        formResult = PostDeleteThreadForm(form);
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
