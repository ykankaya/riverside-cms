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
    public class SqlPageHeaderElementRepository : IElementRepository<PageHeaderElementSettings>
    {
        private readonly IOptions<SqlOptions> _options;

        public SqlPageHeaderElementRepository(IOptions<SqlOptions> options)
        {
            _options = options;
        }

        public async Task<ElementSettings> ReadElementAsync(long tenantId, long elementId)
        {
            using (SqlConnection connection = new SqlConnection(_options.Value.SqlConnectionString))
            {
                connection.Open();

                PageHeaderElementSettings elementSettings = await connection.QueryFirstOrDefaultAsync<PageHeaderElementSettings>(
                    @"SELECT cms.Element.TenantId, cms.Element.ElementId, cms.Element.ElementTypeId, cms.Element.Name,
                        element.PageHeader.PageId, element.PageHeader.ShowName, element.PageHeader.ShowDescription, element.PageHeader.ShowImage,  
                        element.PageHeader.ShowCreated, element.PageHeader.ShowUpdated, element.PageHeader.ShowOccurred, element.PageHeader.ShowBreadcrumbs
                        FROM cms.Element INNER JOIN element.PageHeader ON cms.Element.TenantId = element.PageHeader.TenantId AND cms.Element.ElementId = element.PageHeader.ElementId
                        WHERE cms.Element.TenantId = @TenantId AND cms.Element.ElementId = @ElementId",
                    new { TenantId = tenantId, ElementId = elementId }
                );

                return elementSettings;
            }
        }
    }
}
