using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Authentication
{
    public class ResetPasswordStatusModel
    {
        public long TenantId { get; set; }
        public string ResetPasswordKey { get; set; }
    }
}
