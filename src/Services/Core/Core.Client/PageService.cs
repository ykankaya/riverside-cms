using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RestSharp;

namespace Core.Client
{
    public class PageService : IPageService
    {
        private readonly IOptions<ApiOptions> _options;

        public PageService(IOptions<ApiOptions> options)
        {
            _options = options;
        }

        private void CheckResponseStatus<T>(IRestResponse<T> response) where T : new()
        {
            if (response.ErrorException != null)
                throw new CoreClientException($"Core API failed with response status {response.ResponseStatus}", response.ErrorException);
        }

        public async Task<Page> ReadPageAsync(long tenantId, long pageId)
        {
            try
            {
                RestClient client = new RestClient(_options.Value.ApiBaseUrl);
                RestRequest request = new RestRequest(_options.Value.ApiPageUrl, Method.GET);
                request.AddUrlSegment("tenantId", tenantId);
                request.AddUrlSegment("pageId", pageId);
                IRestResponse<Page> response = await client.ExecuteAsync<Page>(request);
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

        public async Task<List<PageZone>> SearchPageZonesAsync(long tenantId, long pageId)
        {
            try
            {
                RestClient client = new RestClient(_options.Value.ApiBaseUrl);
                RestRequest request = new RestRequest(_options.Value.ApiPageZonesUrl, Method.GET);
                request.AddUrlSegment("tenantId", tenantId);
                request.AddUrlSegment("pageId", pageId);
                IRestResponse<List<PageZone>> response = await client.ExecuteAsync<List<PageZone>>(request);
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

        public async Task<PageZone> ReadPageZoneAsync(long tenantId, long pageId, long pageZoneId)
        {
            try
            {
                RestClient client = new RestClient(_options.Value.ApiBaseUrl);
                RestRequest request = new RestRequest(_options.Value.ApiPageZoneUrl, Method.GET);
                request.AddUrlSegment("tenantId", tenantId);
                request.AddUrlSegment("pageId", pageId);
                request.AddUrlSegment("pageZoneId", pageZoneId);
                IRestResponse<PageZone> response = await client.ExecuteAsync<PageZone>(request);
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

        public async Task<List<PageZoneElement>> SearchPageZoneElementsAsync(long tenantId, long pageId, long pageZoneId)
        {
            try
            {
                RestClient client = new RestClient(_options.Value.ApiBaseUrl);
                RestRequest request = new RestRequest(_options.Value.ApiPageZoneElementsUrl, Method.GET);
                request.AddUrlSegment("tenantId", tenantId);
                request.AddUrlSegment("pageId", pageId);
                request.AddUrlSegment("pageZoneId", pageZoneId);
                IRestResponse<List<PageZoneElement>> response = await client.ExecuteAsync<List<PageZoneElement>>(request);
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

        public async Task<PageZoneElement> ReadPageZoneElementAsync(long tenantId, long pageId, long pageZoneId, long pageZoneElementId)
        {
            try
            {
                RestClient client = new RestClient(_options.Value.ApiBaseUrl);
                RestRequest request = new RestRequest(_options.Value.ApiPageZoneElementUrl, Method.GET);
                request.AddUrlSegment("tenantId", tenantId);
                request.AddUrlSegment("pageId", pageId);
                request.AddUrlSegment("pageZoneId", pageZoneId);
                request.AddUrlSegment("pageZoneElementId", pageZoneElementId);
                IRestResponse<PageZoneElement> response = await client.ExecuteAsync<PageZoneElement>(request);
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
