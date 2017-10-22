using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.UI.Controls;

namespace Riverside.Cms.Elements.PageList
{
    public class PageListContent : ElementContent
    {
        public List<Page> Pages { get; set; }
        public int Total { get; set; }
        public Pager Pager { get; set; }
        public Page Page { get; set; }
        public string DisplayName { get; set; }
        public Page CurrentPage { get; set; }
        public IDictionary<long, int> CommentCounts { get; set; }
    }
}
