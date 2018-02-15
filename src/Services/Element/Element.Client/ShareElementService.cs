using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RestSharp;
using Riverside.Cms.Utilities.Net.RestSharpExtensions;

namespace Riverside.Cms.Services.Element.Client
{
    public class ShareElementSettings : ElementSettings
    {
        public string DisplayName { get; set; }
        public bool ShareOnDigg { get; set; }
        public bool ShareOnFacebook { get; set; }
        public bool ShareOnGoogle { get; set; }
        public bool ShareOnLinkedIn { get; set; }
        public bool ShareOnPinterest { get; set; }
        public bool ShareOnReddit { get; set; }
        public bool ShareOnStumbleUpon { get; set; }
        public bool ShareOnTumblr { get; set; }
        public bool ShareOnTwitter { get; set; }
    }

    public class ShareElementContent : IElementContent
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Via { get; set; }
        public string Hashtags { get; set; }
        public string Image { get; set; }
        public string IsVideo { get; set; }
    }

    public interface IShareElementService : IElementSettingsService<ShareElementSettings>, IElementContentService<ShareElementContent>
    {
    }

    public class ShareElementService : IShareElementService
    {
        private readonly IOptions<ElementApiOptions> _options;

        public ShareElementService(IOptions<ElementApiOptions> options)
        {
            _options = options;
        }

        private void CheckResponseStatus<T>(IRestResponse<T> response) where T : new()
        {
            if (response.ErrorException != null)
                throw new ElementClientException($"Element API failed with response status {response.ResponseStatus}", response.ErrorException);
        }

        public async Task<ShareElementSettings> ReadElementSettingsAsync(long tenantId, long elementId)
        {
            try
            {
                RestClient client = new RestClient(_options.Value.ElementApiBaseUrl);
                RestRequest request = new RestRequest("tenants/{tenantId}/elementtypes/cf0d7834-54fb-4a6e-86db-0f238f8b1ac1/elements/{elementId}", Method.GET);
                request.AddUrlSegment("tenantId", tenantId);
                request.AddUrlSegment("elementId", elementId);
                IRestResponse<ShareElementSettings> response = await client.ExecuteAsync<ShareElementSettings>(request);
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

        public async Task<ShareElementContent> ReadElementContentAsync(long tenantId, long elementId, long pageId)
        {
            try
            {
                RestClient client = new RestClient(_options.Value.ElementApiBaseUrl);
                RestRequest request = new RestRequest("tenants/{tenantId}/elementtypes/cf0d7834-54fb-4a6e-86db-0f238f8b1ac1/elements/{elementId}/content", Method.GET);
                request.AddUrlSegment("tenantId", tenantId);
                request.AddUrlSegment("elementId", elementId);
                request.AddQueryParameter("pageId", pageId.ToString());
                IRestResponse<ShareElementContent> response = await client.ExecuteAsync<ShareElementContent>(request);
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
