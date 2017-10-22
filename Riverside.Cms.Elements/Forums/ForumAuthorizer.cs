using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authorization;
using Riverside.Utilities.Authorization;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.Forums
{
    public class ForumAuthorizer : IForumAuthorizer
    {
        private IForumRepository _forumRepository;
        private IFunctionAuthorizer _functionAuthorizer;

        public ForumAuthorizer(IForumRepository forumRepository, IFunctionAuthorizer functionAuthorizer)
        {
            _forumRepository = forumRepository;
            _functionAuthorizer = functionAuthorizer;
        }

        public void AuthorizeCreateThread(CreateThreadInfo info)
        {
            // Check user has correct role and function membership
            _functionAuthorizer.Authorize(new UserFunction { Function = ForumFunctions.ForumUser, UserId = info.UserId, TenantId = info.TenantId });

            // Get forum details
            ForumSettings forumSettings = new ForumSettings { TenantId = info.TenantId, ElementId = info.ElementId };
            _forumRepository.Read(forumSettings);

            // Check that forum owner and thread starter are the same person if forum has owner only threads set true
            if ((forumSettings.OwnerOnlyThreads) && (forumSettings.OwnerUserId != info.UserId || forumSettings.OwnerTenantId != info.TenantId))
                throw new AuthorizationException(string.Format("User {0} not authorized to create thread in forum {1}", info.UserId, info.ElementId));
        }

        public void AuthorizeCreatePost(CreatePostInfo info)
        {
            // Check user has minimum permissions required
            _functionAuthorizer.Authorize(new UserFunction { Function = ForumFunctions.ForumUser, UserId = info.UserId, TenantId = info.TenantId });
        }

        public string GetUpdatePostFunction(long userId, long postUserId)
        {
            return postUserId == userId ? ForumFunctions.ForumUser : ForumFunctions.ForumAdmin;
        }

        public string GetUpdateThreadFunction(long userId, long threadUserId)
        {
            return threadUserId == userId ? ForumFunctions.ForumUser : ForumFunctions.ForumAdmin;
        }

        public bool UserCanCreateThread(long tenantId, long elementId, long userId)
        {
            // If forum has owner only threads, then user creating thread must be owner
            ForumSettings forumSettings = new ForumSettings { TenantId = tenantId, ElementId = elementId };
            _forumRepository.Read(forumSettings);

            // Check that forum owner and thread starter are the same person if forum has owner only threads set true
            return (!forumSettings.OwnerOnlyThreads) || (forumSettings.OwnerOnlyThreads && forumSettings.OwnerUserId == userId && forumSettings.OwnerTenantId == tenantId);
        }

        public void AuthorizeUpdateThread(UpdateThreadInfo info)
        {
            // Retrieve forum thread details
            ForumThread thread = _forumRepository.GetThread(info.TenantId, info.ElementId, info.ThreadId);

            // User can update thread if they created thread or if they are an administrator
            _functionAuthorizer.Authorize(new UserFunction { Function = GetUpdateThreadFunction(info.UserId, thread.UserId), UserId = info.UserId, TenantId = info.TenantId });
        }

        public void AuthorizeUpdatePost(UpdatePostInfo info)
        {
            // Retrieve forum thread details
            ForumPost post = _forumRepository.GetPost(info.TenantId, info.ElementId, info.ThreadId, info.PostId);

            // User can update post if they created post or if they are an administrator
            _functionAuthorizer.Authorize(new UserFunction { Function = GetUpdatePostFunction(info.UserId, post.UserId), UserId = info.UserId, TenantId = info.TenantId });
        }

        public void AuthorizeDeleteThread(DeleteThreadInfo info)
        {
            // Check user has minimum permissions required
            _functionAuthorizer.Authorize(new UserFunction { Function = ForumFunctions.ForumAdmin, UserId = info.UserId, TenantId = info.TenantId });
        }
    }
}
