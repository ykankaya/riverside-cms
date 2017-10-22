using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Tenants
{
    public class Tenant
    {
        public long TenantId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
