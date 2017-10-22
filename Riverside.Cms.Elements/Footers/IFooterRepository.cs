using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.Footers
{
    public interface IFooterRepository
    {
        void Create(FooterSettings settings, IUnitOfWork unitOfWork = null);
        void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null);
        void Read(FooterSettings settings, IUnitOfWork unitOfWork = null);
        void Update(FooterSettings settings, IUnitOfWork unitOfWork = null);
        void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null);
    }
}
