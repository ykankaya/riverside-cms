using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.TagCloud
{
    public class TagCloudContent : ElementContent
    {
        public List<TagCount> TagList { get; set; }
        public IList<TagTagged> TaggedList { get; set; }
        public List<TagCount> RelatedTagList { get; set; }
        public Page Page { get; set; }
        public string Tags { get; set; }
    }
}
