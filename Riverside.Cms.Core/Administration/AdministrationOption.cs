using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Administration
{
    public abstract class AdministrationOption : IAdministrationOption
    {
        public abstract AdministrationAction Action { get; }
        public string Name { get; set; }
    }
}
