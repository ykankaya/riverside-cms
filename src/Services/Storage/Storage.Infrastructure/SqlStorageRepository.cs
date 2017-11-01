using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using Riverside.Cms.Services.Storage.Domain;

namespace Riverside.Cms.Services.Storage.Infrastructure
{
    public class SqlStorageRepository : IStorageRepository
    {
        private readonly IOptions<SqlOptions> _options;

        public SqlStorageRepository(IOptions<SqlOptions> options)
        {
            _options = options;
        }

        public async Task<Blob> ReadBlobAsync(long tenantId, long uploadId)
        {
            using (SqlConnection connection = new SqlConnection(_options.Value.SqlConnectionString))
            {
                connection.Open();

                return await connection.QueryFirstOrDefaultAsync<Blob>(
                    @"SELECT cms.Upload.TenantId, cms.Upload.UploadId, cms.Upload.Name, cms.Upload.Size, cms.Upload.Created, cms.Upload.Updated
                        FROM cms.Upload
                        WHERE cms.Upload.TenantId = @TenantId AND cms.Upload.UploadId = @UploadId", 
                    new { TenantId = tenantId, UploadId = uploadId }
                );
            }
        }
    }
}
