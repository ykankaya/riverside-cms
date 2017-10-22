using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Riverside.Cms.Core.Domains;
using Riverside.Cms.Core.Webs;
using Riverside.UI.Web;
using Riverside.Utilities.Http;

namespace Riverside.Cms.Core.Middleware
{
    /// <summary>
    /// Identifies website and domain for the current web request. If a redirect exists for the current URL, then
    /// user is redirected to the correct target page.
    /// </summary>
    public class CmsMiddleware
    {
        // Member variables
        private IDomainService _domainService;
        private IWebHelperService _webHelperService;
        private IWebService _webService;

        // Function that can process an HTTP request
        private readonly RequestDelegate _next;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="next">The next request delegate in the pipeline.</param>
        /// <param name="domainService">Retrieves domain details.</param>
        /// <param name="webHelperService">Provides access to low level web components.</param>
        /// <param name="webService">Retrieves website information.</param>
        public CmsMiddleware(RequestDelegate next, IDomainService domainService, IWebHelperService webHelperService, IWebService webService)
        {
            _next = next;
            _domainService = domainService;
            _webHelperService = webHelperService;
            _webService = webService;
        }

        /// <summary>
        /// Called in the HTTP pipeline chain of execution when ASP.NET responds to a request.
        /// </summary>
        /// <param name="context">HTTP context.</param>
        public async Task Invoke(HttpContext context)
        {
            // Get root URL (e.g. "http://mydomain.com")
            string rootUrl = _webHelperService.GetRootUrl();

            // Lookup domain details for this URL
            Domain domain = _domainService.ReadByUrl(rootUrl);
            Web web = _webService.Read(domain.TenantId);

            // Save domain details where they can be looked up throughtout the lifecycle of the HTTP request
            _webHelperService.SetItem<Domain>("RiversideCmsDomain", domain);
            _webHelperService.SetItem<Web>("RiversideCmsWeb", web);

            // Redirect if a redirect URL specified, otherwise invoke next action
            if (domain.RedirectUrl != null)
                _webHelperService.Redirect(domain.RedirectUrl);
            else
                await _next.Invoke(context);
        }
    }
}
