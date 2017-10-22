using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;

namespace Riverside.Cms.Elements.Forms
{
    public class FormSettings : ElementSettings
    {
        public string RecipientEmail { get; set; }
        public string SubmitButtonLabel { get; set; }
        public string SubmittedMessage { get; set; }
        public List<FormElementField> Fields { get; set; }
    }
}
