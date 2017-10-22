using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Pages;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.Forums
{
    public interface IForumRepository
    {
        void Create(ForumSettings settings, IUnitOfWork unitOfWork = null);
        long CreateThread(CreateThreadInfo info, DateTime created, IUnitOfWork unitOfWork = null);
        long CreatePost(CreatePostInfo info, DateTime created, IUnitOfWork unitOfWork = null);
        void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null);
        void Read(ForumSettings settings, IUnitOfWork unitOfWork = null);
        void Update(ForumSettings settings, IUnitOfWork unitOfWork = null);
        void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null);
        ForumThread GetThread(long tenantId, long elementId, long threadId, IUnitOfWork unitOfWork = null);
        ForumThreadAndUser GetThreadAndUser(long tenantId, long elementId, long threadId, IUnitOfWork unitOfWork = null);
        ForumPostAndUser GetPostAndUser(long tenantId, long elementId, long threadId, long postId, IUnitOfWork unitOfWork = null);
        ForumPost GetPost(long tenantId, long elementId, long threadId, long postId, IUnitOfWork unitOfWork = null);
        void IncrementThreadViews(long tenantId, long elementId, long threadId, IUnitOfWork unitOfWork = null);
        ForumPosts ListPosts(long tenantId, long elementId, long threadId, int pageIndex, int pageSize, IUnitOfWork unitOfWork = null);
        ForumThreads ListThreads(long tenantId, long elementId, int pageIndex, int pageSize, IUnitOfWork unitOfWork = null);
        ForumThreads ListLatestThreads(long tenantId, long? pageId, int pageSize, bool recursive, IUnitOfWork unitOfWork = null);
        ForumThreads ListTaggedLatestThreads(long tenantId, long? pageId, int pageSize, IList<Tag> tags, bool recursive, IUnitOfWork unitOfWork = null);
        IDictionary<long, int> ListPageForumCounts(long tenantId, IEnumerable<Page> pages, IUnitOfWork unitOfWork = null);
        void UpdateThread(UpdateThreadInfo info, DateTime updated, IUnitOfWork unitOfWork = null);
        void UpdatePost(UpdatePostInfo info, DateTime updated, IUnitOfWork unitOfWork = null);
        void DeleteThread(long tenantId, long elementId, long threadId, IUnitOfWork unitOfWork = null);
    }
}
