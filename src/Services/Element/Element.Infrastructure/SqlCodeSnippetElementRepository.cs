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
    public class SqlCodeSnippetElementRepository : IElementRepository<CodeSnippetElementSettings>
    {
        private readonly IOptions<SqlOptions> _options;

        public SqlCodeSnippetElementRepository(IOptions<SqlOptions> options)
        {
            _options = options;
        }

        public async Task<CodeSnippetElementSettings> ReadElementSettingsAsync(long tenantId, long elementId)
        {
            using (SqlConnection connection = new SqlConnection(_options.Value.SqlConnectionString))
            {
                connection.Open();

                CodeSnippetElementSettings elementSettings = await connection.QueryFirstOrDefaultAsync<CodeSnippetElementSettings>(
                    @"SELECT cms.Element.TenantId, cms.Element.ElementId, cms.Element.ElementTypeId, cms.Element.Name,
                        element.CodeSnippet.Code, element.CodeSnippet.Language
                        FROM cms.Element INNER JOIN element.CodeSnippet ON cms.Element.TenantId = element.CodeSnippet.TenantId AND cms.Element.ElementId = element.CodeSnippet.ElementId
                        WHERE cms.Element.TenantId = @TenantId AND cms.Element.ElementId = @ElementId",
                    new { TenantId = tenantId, ElementId = elementId }
                );

                return elementSettings;
            }
        }
    }
}
