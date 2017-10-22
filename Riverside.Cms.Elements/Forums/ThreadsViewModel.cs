using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.UI.Controls;

namespace Riverside.Cms.Elements.Forums
{
    public class ThreadsViewModel : ForumViewModel
    {
        public ThreadsViewModel()
        {
            Action = ForumAction.Threads;
        }

        public ForumThreads Threads { get; set; }
        public List<int> PageCounts { get; set; }
        public Pager Pager { get; set; }
        public bool ShowCreateThread { get; set; }
        public string CreateThreadUrl { get; set; }
        public string Search { get; set; }
    }
}
