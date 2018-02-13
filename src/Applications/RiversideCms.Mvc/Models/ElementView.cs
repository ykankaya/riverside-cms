using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiversideCms.Mvc.Models
{
    public class ElementView
    {
        public object Settings { get; set; }
        public object Content { get; set; }
    }

    public class ElementView<TSettings> : ElementView
    {
        public new TSettings Settings
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
    }

    public class ElementView<TSettings, TContent> : ElementView
    {
        public new TSettings Settings
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

        public new TContent Content
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
