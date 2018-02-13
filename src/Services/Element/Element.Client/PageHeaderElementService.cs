using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RestSharp;
using Riverside.Cms.Services.Core.Client;
using Riverside.Cms.Utilities.Net.RestSharpExtensions;

namespace Riverside.Cms.Services.Element.Client
{
    public class PageHeaderElementSettings : ElementSettings
    {
        public long? PageId { get; set; }
        public bool ShowName { get; set; }
        public bool ShowDescription { get; set; }
        public bool ShowImage { get; set; }
        public bool ShowCreated { get; set; }
        public bool ShowUpdated { get; set; }
        public bool ShowOccurred { get; set; }
        public bool ShowBreadcrumbs { get; set; }
    }

    public class PageHeaderElementContent : IElementContent
    {
        public Page Page { get; set; }
    }

    public interface IPageHeaderElementService : IElementSettingsService<PageHeaderElementSettings>, IElementContentService<PageHeaderElementContent>
    {
    }

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

        public async Task<PageHeaderElementSettings> ReadElementSettingsAsync(long tenantId, long elementId)
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

        public async Task<PageHeaderElementContent> ReadElementContentAsync(long tenantId, long elementId, long pageId)
        {
            try
            {
                RestClient client = new RestClient(_options.Value.ElementApiBaseUrl);
                RestRequest request = new RestRequest("tenants/{tenantId}/elementtypes/1cbac30c-5deb-404e-8ea8-aabc20c82aa8/elements/{elementId}/content", Method.GET);
                request.AddUrlSegment("tenantId", tenantId);
                request.AddUrlSegment("elementId", elementId);
                request.AddQueryParameter("pageId", pageId.ToString());
                IRestResponse<PageHeaderElementContent> response = await client.ExecuteAsync<PageHeaderElementContent>(request);
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
