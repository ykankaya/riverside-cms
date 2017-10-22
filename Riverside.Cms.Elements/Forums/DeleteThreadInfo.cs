using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Forums
{
    public class DeleteThreadInfo
    {
        public long TenantId { get; set; }
        public long ElementId { get; set; }
        public long ThreadId { get; set; }
        public long UserId { get; set; }
    }
}
