using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.Forums
{
    public interface IForumService
    {
        long CreateThread(CreateThreadInfo info, IUnitOfWork unitOfWork = null);
        long CreatePost(CreatePostInfo info, IUnitOfWork unitOfWork = null);
        int GetThreadPage(long tenantId, long elementId, long threadId, long postId, IUnitOfWork unitOfWork = null);
        ForumThread GetThread(long tenantId, long elementId, long threadId, IUnitOfWork unitOfWork = null);
        ForumThreadAndUser GetThreadAndUser(long tenantId, long elementId, long threadId, IUnitOfWork unitOfWork = null);
        ForumPost GetPost(long tenantId, long elementId, long threadId, long postId, IUnitOfWork unitOfWork = null);
        ForumPostAndUser GetPostAndUser(long tenantId, long elementId, long threadId, long postId, IUnitOfWork unitOfWork = null);
        ForumPosts ListPosts(long tenantId, long elementId, long threadId, int pageIndex, int pageSize, IUnitOfWork unitOfWork = null);
        ForumThreads ListThreads(long tenantId, long elementId, int pageIndex, int pageSize, IUnitOfWork unitOfWork = null);
        void UpdateThread(UpdateThreadInfo info, IUnitOfWork unitOfWork = null);
        void UpdatePost(UpdatePostInfo info, IUnitOfWork unitOfWork = null);
        void DeleteThread(DeleteThreadInfo info, IUnitOfWork unitOfWork = null);
        string GetQuoteMessage(string message, string alias);
    }
}
