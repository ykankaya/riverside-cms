using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Domains;
using Riverside.UI.Controls;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Domains
{
    public interface IDomainPortalService
    {
        Grid<Domain> Search(long tenantId, int? page, string search, bool modelStateValid);
        DomainViewModel Create();
        DomainViewModel ReadUpdateDelete(long tenantId, long domainId, DataAction action);
    }
}
