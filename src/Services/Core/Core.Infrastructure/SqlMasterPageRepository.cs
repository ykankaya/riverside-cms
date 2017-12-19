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
    public class SqlMasterPageRepository : IMasterPageRepository
    {
        private readonly IOptions<SqlOptions> _options;

        public SqlMasterPageRepository(IOptions<SqlOptions> options)
        {
            _options = options;
        }

        public async Task<MasterPage> ReadMasterPageAsync(long tenantId, long masterPageId)
        {
            using (SqlConnection connection = new SqlConnection(_options.Value.SqlConnectionString))
            {
                connection.Open();

                MasterPage masterPage = await connection.QueryFirstOrDefaultAsync<MasterPage>(
                    @"SELECT TenantId, MasterPageId, Name, PageName, PageDescription, AncestorPageId, AncestorPageLevel, PageType, HasOccurred, HasImage,
                        ThumbnailImageWidth, ThumbnailImageHeight, ThumbnailImageResizeMode, PreviewImageWidth, PreviewImageHeight, PreviewImageResizeMode, ImageMinWidth, ImageMinHeight,
	                    Creatable, Deletable, Taggable, Administration, BeginRender, EndRender
                        FROM cms.MasterPage WHERE TenantId = @TenantId AND MasterPageId = @MasterPageId",
                    new { TenantId = tenantId, MasterPageId = masterPageId }
                );

                return masterPage;
            }
        }

        public async Task<MasterPageZone> ReadMasterPageZoneAsync(long tenantId, long masterPageId, long masterPageZoneId)
        {
            using (SqlConnection connection = new SqlConnection(_options.Value.SqlConnectionString))
            {
                connection.Open();

                MasterPageZone masterPageZone = await connection.QueryFirstOrDefaultAsync<MasterPageZone>(
                    @"SELECT TenantId, MasterPageId, MasterPageZoneId, SortOrder, AdminType, ContentType, BeginRender, EndRender, Name
	                    FROM cms.MasterPageZone WHERE TenantId = @TenantId AND MasterPageId = @MasterPageId AND MasterPageZoneId = @MasterPageZoneId",
                    new { TenantId = tenantId, MasterPageId = masterPageId, MasterPageZoneId = masterPageZoneId }
                );

                return masterPageZone;
            }
        }
    }
}
