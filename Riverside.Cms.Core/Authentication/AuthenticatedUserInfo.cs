using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Core.Authentication
{
    public class AuthenticatedUserInfo
    {
        public bool RememberMe { get; set; }
        public AuthenticatedUser User { get; set; }
    }
}
