using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;

namespace Riverside.Cms.Elements.Forums
{
    public class ForumContent : ElementContent, IPageLinkProvider
    {
        public ThreadsViewModel ThreadsViewModel { get; set; }
        public ThreadViewModel ThreadViewModel { get; set; }
        public ForumThreadAndUser ThreadAndUser { get; set; }
        public ForumThread Thread { get; set; }
        public ForumPostAndUser PostAndUser { get; set; }
        public string FormContext { get; set; }
        public Page Page { get; set; }
        public IList<IPageLink> PageLinks { get; set; }
    }
}
