using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Forums
{
    public class PostViewModel
    {
        public ForumPostAndUser PostAndUser { get; set; }
        public bool ShowUpdatePost { get; set; }
        public string ReplyPostUrl { get; set; }
        public string QuotePostUrl { get; set; }
        public string UpdatePostUrl { get; set; }
    }
}
