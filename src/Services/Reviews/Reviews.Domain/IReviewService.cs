using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Reviews.Domain
{
    public interface IReviewService
    {
        Task UpdateReviews(long tenantId, string placeId);
        Task<IEnumerable<TenantPlace>> ListTenantPlaces();
    }
}
