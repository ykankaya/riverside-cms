using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Administration
{
    public class UpdatePageOption : AdministrationOption
    {
        public override AdministrationAction Action { get { return AdministrationAction.UpdatePage; } }
        public long PageId { get; set; }
    }
}
