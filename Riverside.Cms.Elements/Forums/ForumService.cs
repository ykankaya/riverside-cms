using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Elements.Resources;
using Riverside.UI.Web;
using Riverside.Utilities.Data;
using Riverside.Utilities.Http;
using Riverside.Utilities.Mail;

namespace Riverside.Cms.Elements.Forums
{
    public class ForumService : IForumService
    {
        private IEmailService _emailService;
        private IForumAuthorizer _forumAuthorizer;
        private IForumConfigurationService _forumConfigurationService;
        private IForumRepository _forumRepository;
        private IForumValidator _forumValidator;
        private IUserRepository _userRepository;
        private IWebHelperService _webHelperService;

        private const string BlockquoteText = "> ";

        public ForumService(IEmailService emailService, IForumAuthorizer forumAuthorizer, IForumConfigurationService forumConfigurationService, IForumRepository forumRepository, IForumValidator forumValidator, IUserRepository userRepository, IWebHelperService webHelperService)
        {
            _emailService = emailService;
            _forumAuthorizer = forumAuthorizer;
            _forumConfigurationService = forumConfigurationService;
            _forumRepository = forumRepository;
            _forumValidator = forumValidator;
            _userRepository = userRepository;
            _webHelperService = webHelperService;
        }

        public long CreateThread(CreateThreadInfo info, IUnitOfWork unitOfWork = null)
        {
            // Check user permissions
            _forumAuthorizer.AuthorizeCreateThread(info);

            // Validate supplied thread details
            _forumValidator.ValidateCreateThread(info);

            // Remove extraneous white space
            info.Subject = info.Subject.Trim();
            info.Message = info.Message.Trim();

            // Create forum thread
            return _forumRepository.CreateThread(info, DateTime.UtcNow, unitOfWork);
        }

        public int GetThreadPage(long tenantId, long elementId, long threadId, long postId, IUnitOfWork unitOfWork = null)
        {
            ForumPost post = _forumRepository.GetPost(tenantId, elementId, threadId, postId, unitOfWork);
            return post.SortOrder / _forumConfigurationService.PostsPerPage;
        }

        public long CreatePost(CreatePostInfo info, IUnitOfWork unitOfWork = null)
        {
            // Check user permissions
            _forumAuthorizer.AuthorizeCreatePost(info);

            // Validate supplied post details
            _forumValidator.ValidateCreatePost(info);

            // Remove extraneous white space
            info.Message = info.Message.Trim();

            // Create forum post
            long postId = _forumRepository.CreatePost(info, DateTime.UtcNow, unitOfWork);

            // Get thread details to determine whether or not notification should be sent to thread owner
            ForumThread thread = _forumRepository.GetThread(info.TenantId, info.ElementId, info.ThreadId, unitOfWork);
            if (thread.Notify && thread.UserId != info.UserId)
            {
                // Ger details of user who started thread
                User user = _userRepository.ReadUser(info.TenantId, thread.UserId, unitOfWork);
                User postUser = _userRepository.ReadUser(info.TenantId, info.UserId, unitOfWork);

                // Get email that will be sent to thread owner
                int threadPage = GetThreadPage(info.TenantId, info.ElementId, info.ThreadId, postId, unitOfWork);
                int? page = threadPage == 0 ? (int?)null : threadPage + 1;
                Email email = _forumConfigurationService.GetCreatePostEmail(user.Email, user.Alias, postId, page, thread.Subject, postUser.Alias);

                // Send email
                _emailService.SendEmail(email);
            }

            // Return newly allocated post identifier
            return postId;
        }

        public ForumThread GetThread(long tenantId, long elementId, long threadId, IUnitOfWork unitOfWork = null)
        {
            return _forumRepository.GetThread(tenantId, elementId, threadId, unitOfWork);
        }

        public ForumThreadAndUser GetThreadAndUser(long tenantId, long elementId, long threadId, IUnitOfWork unitOfWork = null)
        {
            return _forumRepository.GetThreadAndUser(tenantId, elementId, threadId, unitOfWork);
        }

        public ForumPost GetPost(long tenantId, long elementId, long threadId, long postId, IUnitOfWork unitOfWork = null)
        {
            return _forumRepository.GetPost(tenantId, elementId, threadId, postId, unitOfWork);
        }

        public ForumPostAndUser GetPostAndUser(long tenantId, long elementId, long threadId, long postId, IUnitOfWork unitOfWork = null)
        {
            return _forumRepository.GetPostAndUser(tenantId, elementId, threadId, postId, unitOfWork);
        }

        public ForumPosts ListPosts(long tenantId, long elementId, long threadId, int pageIndex, int pageSize, IUnitOfWork unitOfWork = null)
        {
            // Increment thread views
            _forumRepository.IncrementThreadViews(tenantId, elementId, threadId, unitOfWork);

            // Return thread posts
            return _forumRepository.ListPosts(tenantId, elementId, threadId, pageIndex, pageSize, unitOfWork);
        }

        public ForumThreads ListThreads(long tenantId, long elementId, int pageIndex, int pageSize, IUnitOfWork unitOfWork = null)
        {
            return _forumRepository.ListThreads(tenantId, elementId, pageIndex, pageSize, unitOfWork);
        }

        public void UpdateThread(UpdateThreadInfo info, IUnitOfWork unitOfWork = null)
        {
            // Check user permissions
            _forumAuthorizer.AuthorizeUpdateThread(info);

            // Validate supplied thread details
            _forumValidator.ValidateUpdateThread(info);

            // Remove extraneous white space
            info.Subject = info.Subject.Trim();
            info.Message = info.Message.Trim();

            // Update forum thread
            _forumRepository.UpdateThread(info, DateTime.UtcNow, unitOfWork);
        }

        public void UpdatePost(UpdatePostInfo info, IUnitOfWork unitOfWork = null)
        {
            // Check user permissions
            _forumAuthorizer.AuthorizeUpdatePost(info);

            // Validate supplied post details
            _forumValidator.ValidateUpdatePost(info);

            // Remove extraneous white space
            info.Message = info.Message.Trim();

            // Update forum post
            _forumRepository.UpdatePost(info, DateTime.UtcNow, unitOfWork);

            // Get thread details to determine whether or not notification should be sent to thread owner TODO: Implement this
            ForumThread thread = _forumRepository.GetThread(info.TenantId, info.ElementId, info.ThreadId, unitOfWork);
            if (thread.Notify && thread.UserId != info.UserId)
            {
#if false
                // Get email that will be sent to thread owner
                Email email = _updatePostEmailFactory.CreateEmail();

                // Send email
                _sendEmailService.SendEmail(email);
#endif
            }
        }

        public void DeleteThread(DeleteThreadInfo info, IUnitOfWork unitOfWork = null)
        {
            // Check user permissions
            _forumAuthorizer.AuthorizeDeleteThread(info);

            // Validate supplied thread details
            _forumValidator.ValidateDeleteThread(info);

            // Delete thread
            _forumRepository.DeleteThread(info.TenantId, info.ElementId, info.ThreadId, unitOfWork);
        }

        public string GetQuoteMessage(string message, string alias)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(BlockquoteText + ElementResource.ForumQuoteBegin, _webHelperService.HtmlEncode(alias));
            sb.AppendLine();
            sb.AppendLine(BlockquoteText);
            message = message.Trim();
            using (StringReader sr = new StringReader(message))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(BlockquoteText + line.Trim());
                }
            }
            return sb.ToString();
        }
    }
}
