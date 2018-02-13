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
    public class SqlFooterElementRepository : IElementRepository<FooterElementSettings>
    {
        private readonly IOptions<SqlOptions> _options;

        public SqlFooterElementRepository(IOptions<SqlOptions> options)
        {
            _options = options;
        }

        public async Task<FooterElementSettings> ReadElementSettingsAsync(long tenantId, long elementId)
        {
            using (SqlConnection connection = new SqlConnection(_options.Value.SqlConnectionString))
            {
                connection.Open();

                FooterElementSettings elementSettings = await connection.QueryFirstOrDefaultAsync<FooterElementSettings>(
                    @"SELECT cms.Element.TenantId, cms.Element.ElementId, cms.Element.ElementTypeId, cms.Element.Name,
                        element.Footer.Message, element.Footer.ShowLoggedOnUserOptions, element.Footer.ShowLoggedOffUserOptions
                        FROM cms.Element INNER JOIN element.Footer ON cms.Element.TenantId = element.Footer.TenantId AND cms.Element.ElementId = element.Footer.ElementId
                        WHERE cms.Element.TenantId = @TenantId AND cms.Element.ElementId = @ElementId",
                    new { TenantId = tenantId, ElementId = elementId }
                );

                return elementSettings;
            }
        }
    }
}
