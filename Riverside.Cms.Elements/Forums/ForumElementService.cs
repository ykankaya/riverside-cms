using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.Forums
{
    public class ForumElementService : IAdvancedElementService
    {
        private IAuthenticationService _authenticationService;
        private IForumPortalService _forumPortalService;
        private IForumRepository _forumRepository;
        private IForumService _forumService;

        public ForumElementService(IAuthenticationService authenticationService, IForumPortalService forumPortalService, IForumRepository forumRepository, IForumService forumService)
        {
            _authenticationService = authenticationService;
            _forumPortalService = forumPortalService;
            _forumRepository = forumRepository;
            _forumService = forumService;
        }

        public Guid ElementTypeId
        {
            get
            {
                return new Guid("484192d1-5a4f-496f-981b-7e0120453949");
            }
        }

        public IElementSettings New(long tenantId)
        {
            return new ForumSettings { TenantId = tenantId, ElementTypeId = ElementTypeId };
        }

        public IElementInfo NewInfo(IElementSettings settings, IElementContent content)
        {
            return new ElementInfo<ForumSettings, ForumContent> { Settings = settings, Content = content };
        }

        public void Create(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _forumRepository.Create((ForumSettings)settings, unitOfWork);
        }

        public void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null)
        {
            _forumRepository.Copy(sourceTenantId, sourceElementId, destTenantId, destElementId, unitOfWork);
        }

        public void Read(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _forumRepository.Read((ForumSettings)settings, unitOfWork);
        }

        public void Update(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _forumRepository.Update((ForumSettings)settings, unitOfWork);
        }

        public void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null)
        {
            _forumRepository.Delete(tenantId, elementId, unitOfWork);
        }

        private long? GetLong(IDictionary<string, string> parameters, string key)
        {
            long id;
            string idText;
            if (parameters.TryGetValue(key, out idText) && Int64.TryParse(idText, out id))
                return id;
            return null;
        }

        private int? GetInteger(IDictionary<string, string> parameters, string key)
        {
            int id;
            string idText;
            if (parameters.TryGetValue(key, out idText) && Int32.TryParse(idText, out id))
                return id;
            return null;
        }

        public IElementContent GetContent(IElementSettings settings, IPageContext pageContext, IUnitOfWork unitOfWork = null)
        {
            // Create content
            ForumContent content = new ForumContent
            {
                Page = pageContext.Page,
                PageLinks = new List<IPageLink>()
            };

            // Get user details
            long tenantId = _authenticationService.TenantId;
            AuthenticatedUserInfo userInfo = _authenticationService.GetCurrentUser();
            long? userId = userInfo != null ? (long?)userInfo.User.UserId : null;

            // Get forum action
            string action;
            pageContext.Parameters.TryGetValue("forumaction", out action);

            // The forum action determines what information is retrieved and displayed
            switch (action)
            {
                case "createthread":
                    content.PartialViewName = "ForumCreateThread";
                    content.FormContext = string.Format("{0}|{1}|{2}", action, pageContext.Page.PageId, settings.ElementId);
                    break;

                case "updatethread":
                    long updateThreadId = GetLong(pageContext.Parameters, "threadid").Value;
                    ForumThreadAndUser forumThreadAndUser = _forumService.GetThreadAndUser(settings.TenantId, settings.ElementId, updateThreadId, unitOfWork);
                    content.ThreadAndUser = forumThreadAndUser;
                    content.PartialViewName = "ForumUpdateThread";
                    content.FormContext = string.Format("{0}|{1}|{2}|{3}", action, pageContext.Page.PageId, settings.ElementId, updateThreadId);
                    break;

                case "replythread":
                case "quotethread":
                    long replyThreadId = GetLong(pageContext.Parameters, "threadid").Value;
                    ForumThreadAndUser replyForumThreadAndUser = _forumService.GetThreadAndUser(settings.TenantId, settings.ElementId, replyThreadId, unitOfWork);
                    content.ThreadAndUser = replyForumThreadAndUser;
                    content.PartialViewName = "ForumReplyThread";
                    content.FormContext = string.Format("{0}|{1}|{2}|{3}", action, pageContext.Page.PageId, settings.ElementId, replyThreadId);
                    break;

                case "deletethread":
                    long deleteThreadId = GetLong(pageContext.Parameters, "threadid").Value;
                    ForumThreadAndUser deleteForumThreadAndUser = _forumService.GetThreadAndUser(settings.TenantId, settings.ElementId, deleteThreadId, unitOfWork);
                    content.ThreadAndUser = deleteForumThreadAndUser;
                    content.PartialViewName = "ForumDeleteThread";
                    content.FormContext = string.Format("{0}|{1}|{2}|{3}", action, pageContext.Page.PageId, settings.ElementId, deleteThreadId);
                    break;

                case "replypost":
                case "quotepost":
                    long actionThreadId = GetLong(pageContext.Parameters, "threadid").Value;
                    long actionPostId = GetLong(pageContext.Parameters, "postid").Value;
                    ForumThread forumThread = _forumService.GetThread(settings.TenantId, settings.ElementId, actionThreadId, unitOfWork);
                    content.Thread = forumThread;
                    content.PostAndUser = _forumService.GetPostAndUser(settings.TenantId, settings.ElementId, actionThreadId, actionPostId, unitOfWork);
                    content.PartialViewName = "ForumReplyPost";
                    content.FormContext = string.Format("{0}|{1}|{2}|{3}|{4}", action, pageContext.Page.PageId, settings.ElementId, actionThreadId, actionPostId);
                    break;

                case "updatepost":
                    long updatePostThreadId = GetLong(pageContext.Parameters, "threadid").Value;
                    long updatePostPostId = GetLong(pageContext.Parameters, "postid").Value;
                    ForumThread updatePostForumThread = _forumService.GetThread(settings.TenantId, settings.ElementId, updatePostThreadId, unitOfWork);
                    content.Thread = updatePostForumThread;
                    content.PostAndUser = _forumService.GetPostAndUser(settings.TenantId, settings.ElementId, updatePostThreadId, updatePostPostId, unitOfWork);
                    content.PartialViewName = "ForumUpdatePost";
                    content.FormContext = string.Format("{0}|{1}|{2}|{3}|{4}", action, pageContext.Page.PageId, settings.ElementId, updatePostThreadId, updatePostPostId);
                    break;

                case "thread":
                    int? threadPage = GetInteger(pageContext.Parameters, "page");
                    long threadId = GetLong(pageContext.Parameters, "threadid").Value;
                    content.PartialViewName = "ForumThread";
                    content.ThreadViewModel = _forumPortalService.GetThreadViewModel(content.Page, tenantId, userId, settings.ElementId, threadId, threadPage);
                    break;

                default:
                    int? threadsPage = GetInteger(pageContext.Parameters, "page");
                    content.ThreadsViewModel = _forumPortalService.GetThreadsViewModel(pageContext.Page, tenantId, userId, settings.ElementId, threadsPage);
                    content.PartialViewName = "Forum";
                    break;
            }

            // Return forum content
            return content;
        }
    }
}
