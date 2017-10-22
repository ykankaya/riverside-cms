using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Forums
{
    public class ForumThreadExtended
    {
        public long PageId { get; set; }
        public ForumThread Thread { get; set; }
        public ForumUser User { get; set; }
        public long? LastPostUserId { get; set; }
        public ForumUser LastPostUser { get; set; }
    }
}
