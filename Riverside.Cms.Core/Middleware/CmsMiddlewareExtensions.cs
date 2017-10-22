using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace Riverside.Cms.Core.Middleware
{
    public static class CmsMiddlewareExtensions
    {
        public static IApplicationBuilder UseCmsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CmsMiddleware>();
        }
    }
}
