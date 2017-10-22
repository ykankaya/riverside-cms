using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Administration
{
    public class UpdateMasterPageElementOption : UpdateElementOption
    {
        public override AdministrationAction Action { get { return AdministrationAction.UpdateMasterPageElement; } }
        public long MasterPageId { get; set; }
    }
}
