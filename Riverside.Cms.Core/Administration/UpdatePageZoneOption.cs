using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Administration
{
    public class UpdatePageZoneOption : AdministrationOption
    {
        public override AdministrationAction Action { get { return AdministrationAction.UpdatePageZone; } }
        public long PageId { get; set; }
        public long PageZoneId { get; set; }
    }
}
