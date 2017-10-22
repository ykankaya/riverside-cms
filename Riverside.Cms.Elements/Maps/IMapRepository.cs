using Riverside.Utilities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Maps
{
    public interface IMapRepository
    {
        void Create(MapSettings settings, IUnitOfWork unitOfWork = null);
        void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null);
        void Read(MapSettings settings, IUnitOfWork unitOfWork = null);
        void Update(MapSettings settings, IUnitOfWork unitOfWork = null);
        void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null);
    }
}
