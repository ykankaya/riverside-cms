using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using Riverside.Cms.Services.Core.Domain;
using static Dapper.SqlMapper;

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

        private IEnumerable<MasterPageZone> PopulateMasterPageZones(IEnumerable<MasterPageZone> masterPageZones, IEnumerable<MasterPageZoneElementTypeDto> masterPageZoneElementTypeIds)
        {
            foreach (MasterPageZone masterPageZone in masterPageZones)
            {
                masterPageZone.ElementTypeIds = masterPageZoneElementTypeIds.Where(e => e.MasterPageZoneId == masterPageZone.MasterPageZoneId).Select(e => e.ElementTypeId);
                yield return masterPageZone;
            }
        }

        public async Task<IEnumerable<MasterPageZone>> SearchMasterPageZonesAsync(long tenantId, long masterPageId)
        {
            using (SqlConnection connection = new SqlConnection(_options.Value.SqlConnectionString))
            {
                connection.Open();

                using (GridReader gr = await connection.QueryMultipleAsync(
                    @"SELECT TenantId, MasterPageId, MasterPageZoneId, SortOrder, AdminType, ContentType, BeginRender, EndRender, Name
                        FROM cms.MasterPageZone WHERE TenantId = @TenantId AND MasterPageId = @MasterPageId ORDER BY SortOrder
                      SELECT MasterPageZoneId, ElementTypeId FROM cms.MasterPageZoneElementType
                        WHERE TenantId = @TenantId AND MasterPageId = @MasterPageId",
                    new { TenantId = tenantId, MasterPageId = masterPageId }
                    )
                )
                {
                    IEnumerable<MasterPageZone> masterPageZones = await gr.ReadAsync<MasterPageZone>();
                    IEnumerable<MasterPageZoneElementTypeDto> masterPageZoneElementTypeIds = await gr.ReadAsync<MasterPageZoneElementTypeDto>();
                    return PopulateMasterPageZones(masterPageZones, masterPageZoneElementTypeIds);
                }
            }
        }

        public async Task<MasterPageZone> ReadMasterPageZoneAsync(long tenantId, long masterPageId, long masterPageZoneId)
        {
            using (SqlConnection connection = new SqlConnection(_options.Value.SqlConnectionString))
            {
                connection.Open();

                using (GridReader gr = await connection.QueryMultipleAsync(
                    @"SELECT TenantId, MasterPageId, MasterPageZoneId, SortOrder, AdminType, ContentType, BeginRender, EndRender, Name
	                    FROM cms.MasterPageZone WHERE TenantId = @TenantId AND MasterPageId = @MasterPageId AND MasterPageZoneId = @MasterPageZoneId
                      SELECT ElementTypeId FROM cms.MasterPageZoneElementType
                        WHERE TenantId = @TenantId AND MasterPageId = @MasterPageId AND MasterPageZoneId = @MasterPageZoneId",
                    new { TenantId = tenantId, MasterPageId = masterPageId, MasterPageZoneId = masterPageZoneId }
                    )
                )
                {
                    MasterPageZone masterPageZone = await gr.ReadFirstOrDefaultAsync<MasterPageZone>();
                    masterPageZone.ElementTypeIds = await gr.ReadAsync<Guid>();
                    return masterPageZone;
                }
            }
        }

        public async Task<IEnumerable<MasterPageZoneElement>> SearchMasterPageZoneElementsAsync(long tenantId, long masterPageId, long masterPageZoneId)
        {
            using (SqlConnection connection = new SqlConnection(_options.Value.SqlConnectionString))
            {
                connection.Open();
                IEnumerable<MasterPageZoneElement> masterPageZoneElements = await connection.QueryAsync<MasterPageZoneElement>(
                    @"SELECT TenantId, MasterPageId, MasterPageZoneId, MasterPageZoneElementId, SortOrder, ElementId, BeginRender, EndRender
                        FROM cms.MasterPageZoneElement WHERE TenantId = @TenantId AND MasterPageId = @MasterPageId AND MasterPageZoneId = @MasterPageZoneId
                        ORDER BY SortOrder",
                    new { TenantId = tenantId, MasterPageId = masterPageId, MasterPageZoneId = masterPageZoneId }
                );
                return masterPageZoneElements;
            }
        }

        public async Task<MasterPageZoneElement> ReadMasterPageZoneElementAsync(long tenantId, long masterPageId, long masterPageZoneId, long masterPageZoneElementId)
        {
            using (SqlConnection connection = new SqlConnection(_options.Value.SqlConnectionString))
            {
                connection.Open();

                MasterPageZoneElement masterPageZoneElement = await connection.QueryFirstOrDefaultAsync<MasterPageZoneElement>(
                    @"SELECT TenantId, MasterPageId, MasterPageZoneId, MasterPageZoneElementId, SortOrder, ElementId, BeginRender, EndRender
                        FROM cms.MasterPageZoneElement WHERE TenantId = @TenantId AND MasterPageId = @MasterPageId AND MasterPageZoneId = @MasterPageZoneId AND
                        MasterPageZoneElementId = @MasterPageZoneElementId",
                    new { TenantId = tenantId, MasterPageId = masterPageId, MasterPageZoneId = masterPageZoneId, MasterPageZoneElementId = masterPageZoneElementId }
                );

                return masterPageZoneElement;
            }
        }
    }
}
