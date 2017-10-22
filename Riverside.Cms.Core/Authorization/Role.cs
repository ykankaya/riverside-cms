using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Authorization
{
    public class Role
    {
        public long RoleId { get; set; }
        public string Name { get; set; }
        public List<Function> Functions { get; set; }
    }
}
