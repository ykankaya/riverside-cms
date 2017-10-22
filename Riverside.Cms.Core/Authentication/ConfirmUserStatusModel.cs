using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Authentication
{
    public class ConfirmUserStatusModel
    {
        public long TenantId { get; set; }
        public bool SetPassword { get; set; }
        public string ConfirmKey { get; set; }
    }
}
