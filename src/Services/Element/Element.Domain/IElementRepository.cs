using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Element.Domain
{
    public interface IElementRepository<T> where T : IElementSettings
    {
        Task<T> ReadElementAsync(long tenantId, long elementId);
    }
}
