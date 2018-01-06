using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RestSharp;
using Riverside.Cms.Utilities.RestSharpExtensions;

namespace Riverside.Cms.Services.Core.Client
{
    public class PageViewService : IPageViewService
    {
        private readonly IOptions<ApiOptions> _options;

        public PageViewService(IOptions<ApiOptions> options)
        {
            _options = options;
        }

        private void CheckResponseStatus<T>(IRestResponse<T> response) where T : new()
        {
            if (response.ErrorException != null)
                throw new CoreClientException($"Core API failed with response status {response.ResponseStatus}", response.ErrorException);
        }

        public async Task<PageView> ReadPageViewAsync(long tenantId, long pageId)
        {
            try
            {
                RestClient client = new RestClient(_options.Value.ApiBaseUrl);
                RestRequest request = new RestRequest("tenants/{tenantId}/pageviews/{pageId}", Method.GET);
                request.AddUrlSegment("tenantId", tenantId);
                request.AddUrlSegment("pageId", pageId);
                IRestResponse<PageView> response = await client.ExecuteAsync<PageView>(request);
                CheckResponseStatus(response);
                return response.Data;
            }
            catch (CoreClientException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CoreClientException("Core API failed", ex);
            }
        }
    }
}
