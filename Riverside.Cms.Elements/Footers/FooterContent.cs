using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Administration;
using Riverside.Cms.Core.Elements;

namespace Riverside.Cms.Elements.Footers
{
    public class FooterContent : ElementContent, IAdministrationContent
    {
        public IAdministrationOptions Options { get; set; }
        public string FormattedMessage { get; set; }
    }
}
