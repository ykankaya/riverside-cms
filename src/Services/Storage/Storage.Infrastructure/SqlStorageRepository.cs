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

        private List<string> GetLocationFromDto(BlobDto dto)
        {
            List<string> location = new List<string>();
            if (dto.Folder1 != null)
                location.Add(dto.Folder1);
            if (dto.Folder2 != null)
                location.Add(dto.Folder2);
            if (dto.Folder3 != null)
                location.Add(dto.Folder3);
            return location;
        }

        private string GetFolderFromBlob(Blob blob, int level)
        {
            if (blob.Location == null || blob.Location.Count <= level)
                return null;
            return blob.Location[level];
        }

        private Blob GetBlobFromDto(BlobDto dto)
        {
            Blob blob = null;
            if (dto.Width.HasValue && dto.Height.HasValue)
                blob = new BlobImage { Width = dto.Width.Value, Height = dto.Height.Value };
            else
                blob = new Blob();
            blob.TenantId = dto.TenantId;
            blob.BlobId = dto.BlobId;
            blob.Size = dto.Size;
            blob.ContentType = dto.ContentType;
            blob.Location = GetLocationFromDto(dto);
            blob.Name = dto.Name;
            blob.Created = dto.Created;
            blob.Updated = dto.Updated;
            return blob;
        }

        private BlobDto GetDtoFromBlob(Blob blob)
        {
            BlobDto dto = new BlobDto
            {
                TenantId = blob.TenantId,
                BlobId = blob.BlobId,
                Size = blob.Size,
                ContentType = blob.ContentType,
                Folder1 = GetFolderFromBlob(blob, 0),
                Folder2 = GetFolderFromBlob(blob, 1),
                Folder3 = GetFolderFromBlob(blob, 2),
                Name = blob.Name,
                Created = blob.Created,
                Updated = blob.Updated
            };
            if (blob is BlobImage)
            {
                dto.Width = ((BlobImage)blob).Width;
                dto.Height = ((BlobImage)blob).Height;
            }
            return dto;
        }

        public async Task<long> CreateBlobAsync(long tenantId, Blob blob)
        {
            BlobDto dto = GetDtoFromBlob(blob);

            using (SqlConnection connection = new SqlConnection(_options.Value.SqlConnectionString))
            {
                connection.Open();
                long blobId = await connection.QuerySingleAsync<long>(
                    @"INSERT INTO Blob (TenantId, Size, ContentType, Name, Folder1, Folder2, Folder3, Width, Height, Created, Updated)
                        VALUES(@TenantId, @Size, @ContentType, @Name, @Folder1, @Folder2, @Folder3, @Width, @Height, @Created, @Updated)
                        SELECT CAST(SCOPE_IDENTITY() as bigint)",
                    dto
                );
                return blobId;
            }
        }

        public async Task<Blob> ReadBlobAsync(long tenantId, long blobId)
        {
            using (SqlConnection connection = new SqlConnection(_options.Value.SqlConnectionString))
            {
                connection.Open();

                BlobDto dto = await connection.QueryFirstOrDefaultAsync<BlobDto>(
                    @"SELECT TenantId, BlobId, Size, ContentType, Name, Folder1, Folder2, Folder3, Width, Height, Created, Updated
                        FROM Blob WHERE TenantId = @TenantId AND BlobId = @BlobId", 
                    new { TenantId = tenantId, BlobId = blobId }
                );

                return GetBlobFromDto(dto);
            }
        }
    }
}
