using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Domains
{
    public interface IDomainRepository
    {
        ISearchResult<Domain> Search(long tenantId, ISearchParameters parameters, IUnitOfWork unitOfWork = null);
        long Create(Domain domain, IUnitOfWork unitOfWork = null);
        Domain Read(long tenantId, long domainId, IUnitOfWork unitOfWork = null);
        Domain ReadByUrl(string url, IUnitOfWork unitOfWork = null);
        void Update(Domain domain, IUnitOfWork unitOfWork = null);
        void Delete(long tenantId, long domainId, IUnitOfWork unitOfWork = null);
        void DeleteByTenant(long tenantId, IUnitOfWork unitOfWork = null);
    }
}
