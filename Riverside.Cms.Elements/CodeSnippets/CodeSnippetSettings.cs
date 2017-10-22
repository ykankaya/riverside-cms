using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;

namespace Riverside.Cms.Elements.CodeSnippets
{
    public class CodeSnippetSettings : ElementSettings
    {
        public string Code { get; set; }
        public Language Language { get; set; }
    }
}
