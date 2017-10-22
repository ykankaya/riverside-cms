using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Mail;

namespace Riverside.Cms.Elements.Forums
{
    /// <summary>
    /// Implement this interface to provide configuration values for forums.
    /// </summary>
    public interface IForumConfigurationService
    {
        Email GetCreatePostEmail(string email, string alias, long postId, int? page, string subject, string postAlias);
        int PostsPerPage { get; }
        int ThreadsPerPage { get; }
    }
}
