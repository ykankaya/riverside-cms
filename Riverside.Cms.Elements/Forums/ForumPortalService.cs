using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Riverside.Cms.Core.Administration;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Cms.Core.Extensions;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Elements.Resources;
using Riverside.UI.Controls;
using Riverside.UI.Routes;

namespace Riverside.Cms.Elements.Forums
{
    public class ForumPortalService : IForumPortalService
    {
        private IActionContextAccessor _actionContextAccessor;
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IForumAuthorizer _forumAuthorizer;
        private IForumConfigurationService _forumConfigurationService;
        private IForumService _forumService;
        private IUrlHelperFactory _urlHelperFactory;

        public ForumPortalService(IActionContextAccessor actionContextAccessor, IAuthenticationService authenticationService, IAuthorizationService authorizationService, IForumAuthorizer forumAuthorizer, IForumConfigurationService forumConfigurationService, IForumService forumService, IUrlHelperFactory urlHelperFactory)
        {
            _actionContextAccessor = actionContextAccessor;
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _forumAuthorizer = forumAuthorizer;
            _forumConfigurationService = forumConfigurationService;
            _forumService = forumService;
            _urlHelperFactory = urlHelperFactory;
        }

        private string GetAuthenticatedUrl(IUrlHelper urlHelper, string url, bool authenticated)
        {
            if (authenticated)
                return url;
            return urlHelper.AdminUrl(AdministrationAction.LogonUser, new { returnurl = url });
        }

        private string GetCreateThreadUrl(Page page, bool authenticated)
        {
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
            string url = urlHelper.PageUrl(page, ElementResource.ForumCreateThreadButtonLabel, new { forumaction = "createthread" });
            return GetAuthenticatedUrl(urlHelper, url, authenticated);
        }

        private string GetThreadUrl(Page page, bool authenticated, ForumAction action, ForumThread thread)
        {
            string url = null;
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
            switch (action)
            {
                case ForumAction.ReplyThread:
                    url = urlHelper.PageUrl(page, thread.Subject, new { forumaction = "replythread", threadid = thread.ThreadId });
                    break;

                case ForumAction.QuoteThread:
                    url = urlHelper.PageUrl(page, thread.Subject, new { forumaction = "quotethread", threadid = thread.ThreadId });
                    break;

                case ForumAction.UpdateThread:
                    url = urlHelper.PageUrl(page, thread.Subject, new { forumaction = "updatethread", threadid = thread.ThreadId });
                    break;

                case ForumAction.DeleteThread:
                    url = urlHelper.PageUrl(page, thread.Subject, new { forumaction = "deletethread", threadid = thread.ThreadId });
                    break;
            }
            return GetAuthenticatedUrl(urlHelper, url, authenticated);
        }

        private string GetPostUrl(Page page, bool authenticated, ForumAction action, ForumThread thread, ForumPost post)
        {
            string url = null;
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
            switch (action)
            {
                case ForumAction.ReplyPost:
                    url = urlHelper.PageUrl(page, thread.Subject, new { forumaction = "replypost", threadid = thread.ThreadId, postid = post.PostId });
                    break;

                case ForumAction.QuotePost:
                    url = urlHelper.PageUrl(page, thread.Subject, new { forumaction = "quotepost", threadid = thread.ThreadId, postid = post.PostId });
                    break;

                case ForumAction.UpdatePost:
                    url = urlHelper.PageUrl(page, thread.Subject, new { forumaction = "updatepost", threadid = thread.ThreadId, postid = post.PostId });
                    break;
            }
            return GetAuthenticatedUrl(urlHelper, url, authenticated);
        }

        public ThreadsViewModel GetThreadsViewModel(Page page, long tenantId, long? userId, long elementId, int? pageIndex)
        {
            int threadsPerPage = _forumConfigurationService.ThreadsPerPage;
            ForumThreads threads = _forumService.ListThreads(tenantId, elementId, pageIndex == null ? 0 : (int)pageIndex - 1, threadsPerPage);
            UrlParameters urlParameters = new UrlParameters { RouteName = "ReadPage", RouteValues = new { pageid = page.PageId, description = page.Name } };
            ThreadsViewModel viewModel = new ThreadsViewModel
            {
                Threads = threads,
                Pager = new Pager { PageIndex = pageIndex == null ? 1 : (int)pageIndex, Total = threads.Total, PageSize = threadsPerPage, UrlParameters = urlParameters },
                PageCounts = new List<int>(),
                ShowCreateThread = (userId == null) || _forumAuthorizer.UserCanCreateThread(tenantId, elementId, userId.Value)
            };
            if (viewModel.ShowCreateThread)
                viewModel.CreateThreadUrl = GetCreateThreadUrl(page, userId.HasValue);
            int postsPerPage = _forumConfigurationService.PostsPerPage;
            foreach (ForumThreadExtended threadExtended in threads)
            {
                viewModel.PageCounts.Add(postsPerPage == 0 ? 1 : ((threadExtended.Thread.Replies - 1) / postsPerPage) + 1);
            }
            return viewModel;
        }

        public ThreadViewModel GetThreadViewModel(Page page, long tenantId, long? userId, long elementId, long threadId, int? pageIndex)
        {
            int pageSize = _forumConfigurationService.PostsPerPage;
            ForumPosts posts = _forumService.ListPosts(tenantId, elementId, threadId, pageIndex == null ? 0 : (int)pageIndex - 1, pageSize);
            UrlParameters urlParameters = new UrlParameters { RouteName = "ReadPage", RouteValues = new { pageid = page.PageId, description = page.Name, threadid = threadId, forumaction = "thread" } };
            ForumThreadAndUser threadAndUser = _forumService.GetThreadAndUser(tenantId, elementId, threadId);
            List<PostViewModel> postViewModels = new List<PostViewModel>();
            foreach (ForumPostAndUser postAndUser in posts)
            {
                PostViewModel postViewModel = new PostViewModel
                {
                    PostAndUser = postAndUser,
                    ShowUpdatePost = (userId != null) && (userId.Value == postAndUser.Post.UserId || _authorizationService.UserInFunction(Functions.UpdatePageElements))
                };
                postViewModel.ReplyPostUrl = GetPostUrl(page, userId.HasValue, ForumAction.ReplyPost, threadAndUser.Thread, postAndUser.Post);
                postViewModel.QuotePostUrl = GetPostUrl(page, userId.HasValue, ForumAction.QuotePost, threadAndUser.Thread, postAndUser.Post);
                if (postViewModel.ShowUpdatePost)
                    postViewModel.UpdatePostUrl = GetPostUrl(page, userId.HasValue, ForumAction.UpdatePost, threadAndUser.Thread, postAndUser.Post);
                postViewModels.Add(postViewModel);
            }
            ThreadViewModel viewModel = new ThreadViewModel
            {
                PostViewModels = postViewModels,
                ThreadAndUser = threadAndUser,
                Pager = new Pager { PageIndex = pageIndex == null ? 1 : (int)pageIndex, Total = posts.Total, PageSize = pageSize, UrlParameters = urlParameters },
                DisplayThreadDetails = pageIndex == null || pageIndex == 1,
                ShowUpdateThread = (userId != null) && (userId.Value == threadAndUser.Thread.UserId || _authorizationService.UserInFunction(Functions.UpdatePageElements)),
                ShowDeleteThread = (userId != null) && _authorizationService.UserInFunction(Functions.UpdatePageElements)
            };
            viewModel.ReplyThreadUrl = GetThreadUrl(page, userId.HasValue, ForumAction.ReplyThread, threadAndUser.Thread);
            viewModel.QuoteThreadUrl = GetThreadUrl(page, userId.HasValue, ForumAction.QuoteThread, threadAndUser.Thread);
            if (viewModel.ShowUpdateThread)
                viewModel.UpdateThreadUrl = GetThreadUrl(page, userId.HasValue, ForumAction.UpdateThread, threadAndUser.Thread);
            if (viewModel.ShowDeleteThread)
                viewModel.DeleteThreadUrl = GetThreadUrl(page, userId.HasValue, ForumAction.DeleteThread, threadAndUser.Thread);
            return viewModel;
        }
    }
}
