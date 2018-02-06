using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Services.Google.Places.Client;

namespace Riverside.Cms.Services.Reviews.Domain
{
    public class ReviewService : IReviewService
    {
        private IPlaceService _placeService;
        private IReviewRepository _reviewRepository;

        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public ReviewService(IPlaceService placeService, IReviewRepository reviewRepository)
        {
            _placeService = placeService;
            _reviewRepository = reviewRepository;
        }

        public Task<IEnumerable<TenantPlace>> ListTenantPlaces()
        {
            return _reviewRepository.ListTenantPlaces();
        }

        // Credit: https://stackoverflow.com/questions/2883576/how-do-you-convert-epoch-time-in-c
        public static DateTime FromUnixTime(long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }

        public async Task UpdateReviews(long tenantId, string placeId)
        {
            Place place = await _placeService.ReadPlaceAsync(placeId);

            List<Review> reviews = new List<Review>();
            foreach (PlaceReview placeReview in place.Result.Reviews)
            {
                Review review = new Review
                {
                    AuthorName = placeReview.AuthorName,
                    AuthorUrl = placeReview.AuthorUrl,
                    ProfilePhotoUrl = placeReview.ProfilePhotoUrl,
                    Rating = placeReview.Rating,
                    TenantId = tenantId,
                    Text = placeReview.Text,
                    Time = FromUnixTime(placeReview.Time)
                };
                reviews.Add(review);
            }

            await _reviewRepository.UpdateReviews(tenantId, reviews);
        }
    }
}
