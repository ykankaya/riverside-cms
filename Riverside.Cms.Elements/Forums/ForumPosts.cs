using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Forums
{
    public class ForumPosts : List<ForumPostAndUser>
    {
        public int Total { get; set; }
    }
}
