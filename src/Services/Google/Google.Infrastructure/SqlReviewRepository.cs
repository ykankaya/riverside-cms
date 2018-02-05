using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Riverside.Cms.Services.Google.Domain;
using static Dapper.SqlMapper;

namespace Riverside.Cms.Services.Google.Infrastructure
{
    public class SqlReviewRepository : IReviewRepository
    {
        private readonly IOptions<SqlOptions> _options;

        public SqlReviewRepository(IOptions<SqlOptions> options)
        {
            _options = options;
        }

        public async Task<IEnumerable<TenantPlace>> ListTenantPlaces()
        {
            using (SqlConnection connection = new SqlConnection(_options.Value.SqlConnectionString))
            {
                connection.Open();
                IEnumerable<TenantPlace> tenantPlaces = await connection.QueryAsync<TenantPlace>(
                    @"SELECT TenantId, PlaceId FROM TenantPlace ORDER BY TenantId"
                );
                return tenantPlaces;
            }
        }

        public Task UpdateReviews(long tenantId, IEnumerable<Review> reviews)
        {
            throw new NotImplementedException();
        }
    }
}
