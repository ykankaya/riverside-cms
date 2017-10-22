using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.Forums
{
    public interface IForumAuthorizer
    {
        void AuthorizeCreateThread(CreateThreadInfo info);
        void AuthorizeCreatePost(CreatePostInfo info);
        string GetUpdatePostFunction(long userId, long postUserId);
        string GetUpdateThreadFunction(long userId, long threadUserId);
        bool UserCanCreateThread(long tenantId, long elementId, long userId);
        void AuthorizeUpdateThread(UpdateThreadInfo info);
        void AuthorizeUpdatePost(UpdatePostInfo info);
        void AuthorizeDeleteThread(DeleteThreadInfo info);
    }
}
