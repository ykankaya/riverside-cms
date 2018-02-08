using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RestSharp;
using Riverside.Cms.Utilities.Net.RestSharpExtensions;

namespace Riverside.Cms.Services.Core.Client
{
    public class MasterPageService : IMasterPageService
    {
        private readonly IOptions<CoreApiOptions> _options;

        public MasterPageService(IOptions<CoreApiOptions> options)
        {
            _options = options;
        }

        private void CheckResponseStatus<T>(IRestResponse<T> response) where T : new()
        {
            if (response.ErrorException != null)
                throw new CoreClientException($"Core API failed with response status {response.ResponseStatus}", response.ErrorException);
        }

        public async Task<MasterPage> ReadMasterPageAsync(long tenantId, long masterPageId)
        {
            try
            {
                RestClient client = new RestClient(_options.Value.ApiBaseUrl);
                RestRequest request = new RestRequest(_options.Value.ApiMasterPageUrl, Method.GET);
                request.AddUrlSegment("tenantId", tenantId);
                request.AddUrlSegment("masterPageId", masterPageId);
                IRestResponse<MasterPage> response = await client.ExecuteAsync<MasterPage>(request);
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

        public async Task<List<MasterPageZone>> SearchMasterPageZonesAsync(long tenantId, long masterPageId)
        {
            try
            {
                RestClient client = new RestClient(_options.Value.ApiBaseUrl);
                RestRequest request = new RestRequest(_options.Value.ApiMasterPageZonesUrl, Method.GET);
                request.AddUrlSegment("tenantId", tenantId);
                request.AddUrlSegment("masterPageId", masterPageId);
                IRestResponse<List<MasterPageZone>> response = await client.ExecuteAsync<List<MasterPageZone>>(request);
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

        public async Task<MasterPageZone> ReadMasterPageZoneAsync(long tenantId, long masterPageId, long masterPageZoneId)
        {
            try
            {
                RestClient client = new RestClient(_options.Value.ApiBaseUrl);
                RestRequest request = new RestRequest(_options.Value.ApiMasterPageZoneUrl, Method.GET);
                request.AddUrlSegment("tenantId", tenantId);
                request.AddUrlSegment("masterPageId", masterPageId);
                request.AddUrlSegment("masterPageZoneId", masterPageZoneId);
                IRestResponse<MasterPageZone> response = await client.ExecuteAsync<MasterPageZone>(request);
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

        public async Task<List<MasterPageZoneElement>> SearchMasterPageZoneElementsAsync(long tenantId, long masterPageId, long masterPageZoneId)
        {
            try
            {
                RestClient client = new RestClient(_options.Value.ApiBaseUrl);
                RestRequest request = new RestRequest(_options.Value.ApiMasterPageZoneElementsUrl, Method.GET);
                request.AddUrlSegment("tenantId", tenantId);
                request.AddUrlSegment("masterPageId", masterPageId);
                request.AddUrlSegment("masterPageZoneId", masterPageZoneId);
                IRestResponse<List<MasterPageZoneElement>> response = await client.ExecuteAsync<List<MasterPageZoneElement>>(request);
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

        public async Task<MasterPageZoneElement> ReadMasterPageZoneElementAsync(long tenantId, long masterPageId, long masterPageZoneId, long masterPageZoneElementId)
        {
            try
            {
                RestClient client = new RestClient(_options.Value.ApiBaseUrl);
                RestRequest request = new RestRequest(_options.Value.ApiMasterPageZoneElementUrl, Method.GET);
                request.AddUrlSegment("tenantId", tenantId);
                request.AddUrlSegment("masterPageId", masterPageId);
                request.AddUrlSegment("masterPageZoneId", masterPageZoneId);
                request.AddUrlSegment("masterPageZoneElementId", masterPageZoneElementId);
                IRestResponse<MasterPageZoneElement> response = await client.ExecuteAsync<MasterPageZoneElement>(request);
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
