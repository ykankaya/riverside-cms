using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.UI.Routes;
using Riverside.Utilities.Http;

namespace Riverside.Cms.Elements.Forums
{
    public interface IForumUrlService
    {
        UrlParameters GetCreatePostUrlParameters(long postId, int? page);
        string GetThreadUrl(long pageId, long threadId, string subject, int? page = null);
        string GetForumUrl(long pageId, string name);
    }
}
