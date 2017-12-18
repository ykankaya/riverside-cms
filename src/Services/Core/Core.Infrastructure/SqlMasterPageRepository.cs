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
    }
}
