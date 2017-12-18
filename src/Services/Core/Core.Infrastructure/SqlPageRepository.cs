using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using Riverside.Cms.Services.Core.Domain;

namespace Riverside.Cms.Services.Core.Infrastructure
{
    public class SqlPageRepository : IPageRepository
    {
        private readonly IOptions<SqlOptions> _options;

        public SqlPageRepository(IOptions<SqlOptions> options)
        {
            _options = options;
        }

        public async Task<Page> ReadPageAsync(long tenantId, long pageId)
        {
            using (SqlConnection connection = new SqlConnection(_options.Value.SqlConnectionString))
            {
                connection.Open();

                Page page = await connection.QueryFirstOrDefaultAsync<Page>(
                    @"SELECT TenantId, PageId, ParentPageId, Name, Description, Created, Updated, Occurred, MasterPageId,
                        ImageUploadId, PreviewImageUploadId, ThumbnailImageUploadId
                        FROM cms.Page WHERE TenantId = @TenantId AND PageId = @PageId",
                    new { TenantId = tenantId, PageId = pageId }
                );

                return page;
            }
        }
    }
}
