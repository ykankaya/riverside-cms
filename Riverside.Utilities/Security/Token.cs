using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Security
{
    public class Token
    {
        public Guid Value { get; set; }
        public DateTime Expiry { get; set; }
    }
}
