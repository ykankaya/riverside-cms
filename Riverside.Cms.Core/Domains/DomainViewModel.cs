using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Domains;
using Riverside.UI.Controls;

namespace Riverside.Cms.Core.Domains
{
    public class DomainViewModel
    {
        public Domain Domain { get; set; }
        public List<Button> Buttons { get; set; }
    }
}
