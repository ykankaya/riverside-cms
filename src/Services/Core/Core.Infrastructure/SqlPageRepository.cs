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

        public async Task<IEnumerable<PageZone>> SearchPageZonesAsync(long tenantId, long pageId)
        {
            using (SqlConnection connection = new SqlConnection(_options.Value.SqlConnectionString))
            {
                connection.Open();
                IEnumerable<PageZone> pageZones = await connection.QueryAsync<PageZone>(
                    @"SELECT TenantId, PageId, PageZoneId, MasterPageId, MasterPageZoneId
                        FROM cms.PageZone WHERE TenantId = @TenantId AND PageId = @PageId",
                    new { TenantId = tenantId, PageId = pageId }
                );
                return pageZones;
            }
        }

        public async Task<PageZone> ReadPageZoneAsync(long tenantId, long pageId, long pageZoneId)
        {
            using (SqlConnection connection = new SqlConnection(_options.Value.SqlConnectionString))
            {
                connection.Open();

                PageZone pageZone = await connection.QueryFirstOrDefaultAsync<PageZone>(
                    @"SELECT TenantId, PageId, PageZoneId, MasterPageId, MasterPageZoneId
                        FROM cms.PageZone WHERE TenantId = @TenantId AND PageId = @PageId AND PageZoneId = @PageZoneId",
                    new { TenantId = tenantId, PageId = pageId, PageZoneId = pageZoneId }
                );

                return pageZone;
            }
        }

        public async Task<PageZoneElement> ReadPageZoneElementAsync(long tenantId, long pageId, long pageZoneId, long pageZoneElementId)
        {
            using (SqlConnection connection = new SqlConnection(_options.Value.SqlConnectionString))
            {
                connection.Open();

                PageZoneElement pageZoneElement = await connection.QueryFirstOrDefaultAsync<PageZoneElement>(
                    @"SELECT TenantId, PageId, PageZoneId, PageZoneElementId, SortOrder, ElementId, MasterPageId, MasterPageZoneId, MasterPageZoneElementId
	                    FROM cms.PageZoneElement WHERE TenantId = @TenantId AND PageId = @PageId AND PageZoneId = @PageZoneId AND PageZoneElementId = @PageZoneElementId",
                    new { TenantId = tenantId, PageId = pageId, PageZoneId = pageZoneId, PageZoneElementId = pageZoneElementId }
                );

                return pageZoneElement;
            }
        }
    }
}
