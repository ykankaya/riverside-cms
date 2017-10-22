using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Authorization
{
    public class UserFunction
    {
        public long TenantId { get; set; }
        public long UserId { get; set; }
        public string Function { get; set; }
    }
}
