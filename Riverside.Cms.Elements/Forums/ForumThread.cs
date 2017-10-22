using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Forums
{
    public class ForumThread
    {
        public long TenantId { get; set; }
        public long ElementId { get; set; }
        public long ThreadId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool Notify { get; set; }
        public int Views { get; set; }
        public int Replies { get; set; }
        public long? LastPostId { get; set; }
        public long UserId { get; set; }
        public DateTime LastMessageCreated { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
