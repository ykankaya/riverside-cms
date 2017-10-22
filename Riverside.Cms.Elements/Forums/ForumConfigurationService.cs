using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Riverside.Cms.Elements.Resources;
using Riverside.UI.Routes;
using Riverside.UI.Web;
using Riverside.Utilities.Http;
using Riverside.Utilities.Mail;
using Riverside.Utilities.Text;

namespace Riverside.Cms.Elements.Forums
{
    public class ForumConfigurationService : IForumConfigurationService
    {
        private static int _postsPerPage;
        private static int _threadsPerPage;

        private const int DEFAULT_POSTS_PER_PAGE = 30;
        private const int DEFAULT_THREADS_PER_PAGE = 30;

        private const string ALIAS_KEYWORD = "%ALIAS%";
        private const string EMAIL_KEYWORD = "%EMAIL%";
        private const string FORUM_URL = "%FORUMURL%";
        private const string THREAD_SUBJECT = "%THREADSUBJECT%";
        private const string POST_ALIAS = "%POSTALIAS%";

        private IEmailConfigurationService _emailConfigurationService;
        private IForumUrlService _forumUrlService;
        private IOptions<ForumOptions> _options;
        private IStringService _stringService;
        private IWebHelperService _webHelperService;

        public ForumConfigurationService(IEmailConfigurationService emailConfigurationService, IForumUrlService forumUrlService, IOptions<ForumOptions> options, IStringService stringService, IWebHelperService webHelperService)
        {
            _emailConfigurationService = emailConfigurationService;
            _forumUrlService = forumUrlService;
            _options = options;
            _stringService = stringService;
            _webHelperService = webHelperService;
        }

        private IEnumerable<KeyValuePair<string, string>> GetCreatePostSubstitutions(string email, string alias, long postId, int? page, string subject, string postAlias)
        {
            UrlParameters urlParameters = _forumUrlService.GetCreatePostUrlParameters(postId, page);
            string forumUrl = _webHelperService.GetUrl(urlParameters);
            Dictionary<string, string> substitutions = new Dictionary<string, string>();
            substitutions.Add(ALIAS_KEYWORD, alias);
            substitutions.Add(EMAIL_KEYWORD, email);
            substitutions.Add(FORUM_URL, forumUrl);
            substitutions.Add(THREAD_SUBJECT, subject);
            substitutions.Add(POST_ALIAS, postAlias);
            return substitutions;
        }

        private EmailContent GetCreatePostEmailContent(string email, string alias, long postId, int? page, string subject, string postAlias)
        {
            EmailContent content = new EmailContent
            {
                Subject = ElementResource.ForumCreatePostEmailSubject,
                PlainTextBody = ElementResource.ForumCreatePostEmailPlainTextBody,
                HtmlBody = ElementResource.ForumCreatePostEmailHtmlBody
            };
            IEnumerable<KeyValuePair<string, string>> substitutions = GetCreatePostSubstitutions(email, alias, postId, page, subject, postAlias);
            content.Subject = _stringService.SubstituteKeywords(content.Subject, substitutions, false);
            content.HtmlBody = _stringService.SubstituteKeywords(content.HtmlBody, substitutions, true);
            content.PlainTextBody = _stringService.SubstituteKeywords(content.PlainTextBody, substitutions, false);
            return content;
        }

        public Email GetCreatePostEmail(string email, string alias, long postId, int? page, string subject, string postAlias)
        {
            // Get to recipients
            EmailAddress toEmailAddress = new EmailAddress { Email = email, DisplayName = alias };
            IEnumerable<EmailAddress> toAddresses = Enumerable.Repeat(toEmailAddress, 1);
            IEnumerable<EmailAddress> configurationToAddresses = _emailConfigurationService.GetToEmailRecipients();
            if (configurationToAddresses != null)
                toAddresses.Concat(configurationToAddresses);

            // Get email content
            EmailContent content = GetCreatePostEmailContent(email, alias, postId, page, subject, postAlias);

            // Return resulting email
            return new Email
            {
                Content = content,
                FromAddress = _emailConfigurationService.GetFromEmailRecipient(),
                ToAddresses = toAddresses,
                ReplyToAddress = _emailConfigurationService.GetReplyToEmailRecipient(),
                BccAddresses = _emailConfigurationService.GetBccEmailRecipients()
            };
        }

        public int PostsPerPage
        {
            get
            {
                if (_postsPerPage == 0)
                {
                    if (_options.Value.PostsPerPage != 0)
                        _postsPerPage = _options.Value.PostsPerPage;
                    else
                        _postsPerPage = DEFAULT_POSTS_PER_PAGE;
                }
                return _postsPerPage;
            }
        }

        public int ThreadsPerPage
        {
            get
            {
                if (_threadsPerPage == 0)
                {
                    if (_options.Value.ThreadsPerPage != 0)
                        _threadsPerPage = _options.Value.ThreadsPerPage;
                    else
                        _threadsPerPage = DEFAULT_THREADS_PER_PAGE;
                }
                return _threadsPerPage;
            }
        }
    }
}
