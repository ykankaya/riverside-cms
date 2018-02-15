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
    public class SqlShareElementRepository : IElementRepository<ShareElementSettings>
    {
        private readonly IOptions<SqlOptions> _options;

        public SqlShareElementRepository(IOptions<SqlOptions> options)
        {
            _options = options;
        }

        public async Task<ShareElementSettings> ReadElementSettingsAsync(long tenantId, long elementId)
        {
            using (SqlConnection connection = new SqlConnection(_options.Value.SqlConnectionString))
            {
                connection.Open();

                ShareElementSettings elementSettings = await connection.QueryFirstOrDefaultAsync<ShareElementSettings>(
                    @"SELECT cms.Element.TenantId, cms.Element.ElementId, cms.Element.ElementTypeId, cms.Element.Name,
                        element.Share.DisplayName, element.Share.ShareOnDigg, element.Share.ShareOnFacebook, element.Share.ShareOnGoogle, element.Share.ShareOnLinkedIn,
                        element.Share.ShareOnPinterest, element.Share.ShareOnReddit, element.Share.ShareOnStumbleUpon, element.Share.ShareOnTumblr, element.Share.ShareOnTwitter
                        FROM cms.Element INNER JOIN element.Share ON cms.Element.TenantId = element.Share.TenantId AND cms.Element.ElementId = element.Share.ElementId
                        WHERE cms.Element.TenantId = @TenantId AND cms.Element.ElementId = @ElementId",
                    new { TenantId = tenantId, ElementId = elementId }
                );

                return elementSettings;
            }
        }
    }
}
