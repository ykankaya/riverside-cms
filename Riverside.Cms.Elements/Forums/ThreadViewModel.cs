using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.UI.Controls;

namespace Riverside.Cms.Elements.Forums
{
    public class ThreadViewModel : ForumViewModel
    {
        public ThreadViewModel()
        {
            Action = ForumAction.Thread;
        }

        public bool DisplayThreadDetails { get; set; }
        public List<PostViewModel> PostViewModels { get; set; }
        public ForumThreadAndUser ThreadAndUser { get; set; }
        public Pager Pager { get; set; }
        public bool ShowUpdateThread { get; set; }
        public bool ShowDeleteThread { get; set; }
        public string ReplyThreadUrl { get; set; }
        public string QuoteThreadUrl { get; set; }
        public string UpdateThreadUrl { get; set; }
        public string DeleteThreadUrl { get; set; }
    }
}
