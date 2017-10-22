using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Forums
{
    public interface IForumValidator
    {
        void ValidateCreateThread(CreateThreadInfo info);
        void ValidateCreatePost(CreatePostInfo info);
        void ValidateUpdateThread(UpdateThreadInfo info);
        void ValidateUpdatePost(UpdatePostInfo info);
        void ValidateDeleteThread(DeleteThreadInfo info);
    }
}
