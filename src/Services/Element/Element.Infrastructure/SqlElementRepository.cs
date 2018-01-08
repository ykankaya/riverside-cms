using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using Riverside.Cms.Services.Element.Domain;

namespace Riverside.Cms.Services.Element.Infrastructure
{
    public class SqlElementRepository : IElementRepository
    {
        private readonly IOptions<SqlOptions> _options;

        public SqlElementRepository(IOptions<SqlOptions> options)
        {
            _options = options;
        }

        public async Task<ElementSettings> ReadElementAsync(long tenantId, long elementId)
        {
            using (SqlConnection connection = new SqlConnection(_options.Value.SqlConnectionString))
            {
                connection.Open();

                ElementSettings elementSettings = await connection.QueryFirstOrDefaultAsync<ElementSettings>(
                    @"SELECT TenantId, ElementId, ElementTypeId, Name FROM cms.Element WHERE TenantId = @TenantId AND ElementId = @ElementId",
                    new { TenantId = tenantId, ElementId = elementId }
                );

                return elementSettings;
            }
        }
    }
}
