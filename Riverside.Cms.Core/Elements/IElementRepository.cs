using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Elements
{
    public interface IElementRepository
    {
        long Create(IElementSettings settings, IUnitOfWork unitOfWork = null);
        long Copy(long sourceTenantId, long sourceElementId, long destTenantId, IUnitOfWork unitOfWork = null);
        void Read(IElementSettings settings, IUnitOfWork unitOfWork = null);
        IElementSettings Read(long tenantId, long elementId, IUnitOfWork unitOfWork = null);
        void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null);
        IEnumerable<ElementType> ListTypes(IUnitOfWork unitOfWork = null);
        IEnumerable<IElementSettings> ListElements(long tenantId, IUnitOfWork unitOfWork = null);
    }
}
