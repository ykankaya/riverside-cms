using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Elements
{
    public class ElementInfo : IElementInfo
    {
        public IElementSettings Settings { get; set; }
        public IElementContent Content { get; set; }
    }

    public class ElementInfo<TSettings, TContent> : ElementInfo, IElementInfo<TSettings, TContent> where TSettings : IElementSettings where TContent : IElementContent
    {
        TSettings IElementInfo<TSettings, TContent>.Settings
        {
            get
            {
                return (TSettings)base.Settings;
            }
            set
            {
                base.Settings = value;
            }
        }

        TContent IElementInfo<TSettings, TContent>.Content
        {
            get
            {
                return (TContent)base.Content;
            }
            set
            {
                base.Content = value;
            }
        }
    }
}
