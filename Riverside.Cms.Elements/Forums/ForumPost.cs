using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Forums
{
    public class ForumPost
    {
        public long TenantId { get; set; }
        public long ElementId { get; set; }
        public long ThreadId { get; set; }
        public long PostId { get; set; }
        public long? ParentPostId { get; set; }
        public long UserId { get; set; }
        public string Message { get; set; }
        public int SortOrder { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
