using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Administration
{
    public class AdministrationOptionSection : IAdministrationOptionSection
    {
        public string Name { get; set; }
        public List<IAdministrationOption> Options { get; set; }
    }
}
