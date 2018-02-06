using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Reviews.Domain
{
    public interface IReviewRepository
    {
        Task UpdateReviews(long tenantId, IEnumerable<Review> reviews);
        Task<IEnumerable<TenantPlace>> ListTenantPlaces();
    }
}
