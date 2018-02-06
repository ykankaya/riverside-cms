using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Riverside.Cms.Services.Google.Places.Client;
using Riverside.Cms.Services.Reviews.Domain;
using Riverside.Cms.Services.Reviews.Infrastructure;
using static Dapper.SqlMapper;

namespace ConsoleHelperApp
{
    class Program
    {
        class ApiOptions : IOptions<PlaceApiOptions>
        {
            public PlaceApiOptions Value => new PlaceApiOptions { PlaceApiBaseUrl = "https://maps.googleapis.com/maps/api/place/", PlaceApiKey = "Your API key goes here" };
        }

        class DbOptions : IOptions<SqlOptions>
        {
            public SqlOptions Value => new SqlOptions { SqlConnectionString = "Your database connection string goes here" };
        }

        // Credit: https://stackoverflow.com/questions/47588531/error-message-cs5001-program-does-not-contain-a-static-main-method-suitable-f
        static async Task Main(string[] args)
        {
            IOptions<PlaceApiOptions> apiOptions = new ApiOptions();
            IPlaceService placeService = new PlaceService(apiOptions);
            IOptions<SqlOptions> dbOptions = new DbOptions();
            IReviewRepository reviewRepository = new SqlReviewRepository(dbOptions);
            IReviewService reviewService = new ReviewService(placeService, reviewRepository);
            IEnumerable<TenantPlace> tenantPlaces = await reviewService.ListTenantPlaces();
            foreach (TenantPlace tenantPlace in tenantPlaces)
                await reviewService.UpdateReviews(tenantPlace.TenantId, tenantPlace.PlaceId);
        }
    }
}
