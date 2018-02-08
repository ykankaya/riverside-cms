using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RestSharp;
using Riverside.Cms.Utilities.Net.RestSharpExtensions;

namespace Riverside.Cms.Services.Element.Client
{
    public class PageHeaderElementService : IPageHeaderElementService
    {
        private readonly IOptions<ElementApiOptions> _options;

        public PageHeaderElementService(IOptions<ElementApiOptions> options)
        {
            _options = options;
        }

        private void CheckResponseStatus<T>(IRestResponse<T> response) where T : new()
        {
            if (response.ErrorException != null)
                throw new ElementClientException($"Element API failed with response status {response.ResponseStatus}", response.ErrorException);
        }

        public async Task<PageHeaderElementSettings> ReadElementAsync(long tenantId, long elementId)
        {
            try
            {
                RestClient client = new RestClient(_options.Value.ElementApiBaseUrl);
                RestRequest request = new RestRequest("tenants/{tenantId}/elementtypes/1cbac30c-5deb-404e-8ea8-aabc20c82aa8/elements/{elementId}", Method.GET);
                request.AddUrlSegment("tenantId", tenantId);
                request.AddUrlSegment("elementId", elementId);
                IRestResponse<PageHeaderElementSettings> response = await client.ExecuteAsync<PageHeaderElementSettings>(request);
                CheckResponseStatus(response);
                return response.Data;
            }
            catch (ElementClientException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ElementClientException("Element API failed", ex);
            }
        }

        public async Task<PageHeaderElementView> GetElementViewAsync(long tenantId, long elementId, long pageId)
        {
            try
            {
                RestClient client = new RestClient(_options.Value.ElementApiBaseUrl);
                RestRequest request = new RestRequest("tenants/{tenantId}/elementtypes/1cbac30c-5deb-404e-8ea8-aabc20c82aa8/elements/{elementId}/view", Method.GET);
                request.AddUrlSegment("tenantId", tenantId);
                request.AddUrlSegment("elementId", elementId);
                request.AddQueryParameter("pageId", pageId.ToString());
                IRestResponse<PageHeaderElementView> response = await client.ExecuteAsync<PageHeaderElementView>(request);
                CheckResponseStatus(response);
                return response.Data;
            }
            catch (ElementClientException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ElementClientException("Element API failed", ex);
            }
        }
    }
}
