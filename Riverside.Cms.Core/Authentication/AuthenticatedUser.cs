using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Authentication
{
    public class AuthenticatedUser
    {
        public long TenantId { get; set; }
        public long UserId { get; set; }
        public string Alias { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
