using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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

        private List<string> GetPathSegmentsFromFolders(string folder1, string folder2, string folder3)
        {
            List<string> pathSegments = new List<string>();
            if (folder1 != null)
                pathSegments.Add(folder1);
            if (folder2 != null)
                pathSegments.Add(folder2);
            if (folder3 != null)
                pathSegments.Add(folder3);
            return pathSegments;
        }

        private string GetFolderFromPathSegments(List<string> pathSegments, int level)
        {
            if (pathSegments == null || pathSegments.Count <= level)
                return null;
            return pathSegments[level];
        }

        private List<string> GetPathSegmentsFromPath(string path)
        {
            return path == string.Empty ? new List<string>() : path.Split('/').ToList();
        }

        private string GetPathFromPathSegments(List<string> pathSegments)
        {
            return string.Join("/", pathSegments);
        }

        private Blob GetBlobFromDto(BlobDto dto)
        {
            Blob blob = null;
            if (dto == null)
                return blob;
            if (dto.Width.HasValue && dto.Height.HasValue)
                blob = new BlobImage { Width = dto.Width.Value, Height = dto.Height.Value };
            else
                blob = new Blob();
            List<string> pathSegments = GetPathSegmentsFromFolders(dto.Folder1, dto.Folder2, dto.Folder3);
            blob.TenantId = dto.TenantId;
            blob.BlobId = dto.BlobId;
            blob.Size = dto.Size;
            blob.ContentType = dto.ContentType;
            blob.Path = GetPathFromPathSegments(pathSegments);
            blob.Name = dto.Name;
            blob.Created = dto.Created;
            blob.Updated = dto.Updated;
            return blob;
        }

        private BlobDto GetDtoFromBlob(Blob blob)
        {
            if (blob == null)
                return null;
            List<string> pathSegments = GetPathSegmentsFromPath(blob.Path);
            BlobDto dto = new BlobDto
            {
                TenantId = blob.TenantId,
                BlobId = blob.BlobId,
                Size = blob.Size,
                ContentType = blob.ContentType,
                Folder1 = GetFolderFromPathSegments(pathSegments, 0),
                Folder2 = GetFolderFromPathSegments(pathSegments, 1),
                Folder3 = GetFolderFromPathSegments(pathSegments, 2),
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

        private string GetWhereClause(string folder, int level)
        {
            return string.Format("Folder{0} {1} {2}", level + 1, folder == null ? "IS" : "=", folder == null ? "NULL" : "@Folder" + (level + 1));
        }

        public async Task<IEnumerable<Blob>> SearchBlobsAsync(long tenantId, string path)
        {
            List<string> pathSegments = GetPathSegmentsFromPath(path);
            string folder1 = GetFolderFromPathSegments(pathSegments, 0);
            string folder2 = GetFolderFromPathSegments(pathSegments, 1);
            string folder3 = GetFolderFromPathSegments(pathSegments, 2);
            using (SqlConnection connection = new SqlConnection(_options.Value.SqlConnectionString))
            {
                connection.Open();
                IEnumerable<BlobDto> dtos = await connection.QueryAsync<BlobDto>(
                    "SELECT TenantId, BlobId, Size, ContentType, Name, Folder1, Folder2, Folder3, Width, Height, Created, Updated FROM Blob " +
                    "WHERE TenantId = @TenantId AND " + GetWhereClause(folder1, 0) + " AND " + GetWhereClause(folder2, 1) + " AND " + GetWhereClause(folder3, 2) +
                    " ORDER BY Folder1, Folder2, Folder3",
                    new { TenantId = tenantId, Folder1 = folder1, Folder2 = folder2, Folder3 = folder3 }
                );
                List<Blob> blobs = new List<Blob>();
                foreach (BlobDto dto in dtos)
                    blobs.Add(GetBlobFromDto(dto));
                return blobs;
            }
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

        public async Task DeleteBlobAsync(long tenantId, long blobId)
        {
            using (SqlConnection connection = new SqlConnection(_options.Value.SqlConnectionString))
            {
                connection.Open();

                await connection.ExecuteAsync(
                    @"DELETE FROM Blob WHERE TenantId = @TenantId AND BlobId = @BlobId",
                    new { TenantId = tenantId, BlobId = blobId }
                );
            }
        }
    }
}
