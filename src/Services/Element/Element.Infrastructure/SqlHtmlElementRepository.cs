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
    public class SqlHtmlElementRepository : IElementRepository<HtmlElementSettings>
    {
        private readonly IOptions<SqlOptions> _options;

        public SqlHtmlElementRepository(IOptions<SqlOptions> options)
        {
            _options = options;
        }

        public async Task<HtmlElementSettings> ReadElementSettingsAsync(long tenantId, long elementId)
        {
            using (SqlConnection connection = new SqlConnection(_options.Value.SqlConnectionString))
            {
                connection.Open();

                HtmlElementSettings elementSettings = await connection.QueryFirstOrDefaultAsync<HtmlElementSettings>(
                    @"SELECT cms.Element.TenantId, cms.Element.ElementId, cms.Element.ElementTypeId, cms.Element.Name,
                        element.Html.Html
                        FROM cms.Element INNER JOIN element.Html ON cms.Element.TenantId = element.Html.TenantId AND cms.Element.ElementId = element.Html.ElementId
                        WHERE cms.Element.TenantId = @TenantId AND cms.Element.ElementId = @ElementId",
                    new { TenantId = tenantId, ElementId = elementId }
                );

                return elementSettings;
            }
        }
    }
}
