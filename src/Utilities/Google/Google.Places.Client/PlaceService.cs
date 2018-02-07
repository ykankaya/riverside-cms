using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RestSharp;
using Riverside.Cms.Utilities.RestSharpExtensions;

namespace Riverside.Cms.Utilities.Google.Places.Client
{
    public class PlaceService : IPlaceService
    {
        private readonly IOptions<PlaceApiOptions> _options;

        public PlaceService(IOptions<PlaceApiOptions> options)
        {
            _options = options;
        }

        private void CheckResponseStatus<T>(IRestResponse<T> response) where T : new()
        {
            if (response.ErrorException != null)
                throw new GoogleClientException($"Google Places API failed with response status {response.ResponseStatus}", response.ErrorException);
        }

        public async Task<Place> ReadPlaceAsync(string placeId)
        {
            try
            {
                RestClient client = new RestClient(_options.Value.PlaceApiBaseUrl);
                RestRequest request = new RestRequest("details/json", Method.GET);
                request.AddQueryParameter("key", _options.Value.PlaceApiKey);
                request.AddQueryParameter("placeid", placeId);
                IRestResponse<Place> response = await client.ExecuteAsync<Place>(request);
                CheckResponseStatus(response);
                return response.Data;
            }
            catch (GoogleClientException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new GoogleClientException("Google Places API failed", ex);
            }
        }
    }
}
