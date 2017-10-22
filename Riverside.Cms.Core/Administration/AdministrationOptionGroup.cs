using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Administration
{
    public class AdministrationOptionGroup : IAdministrationOptionGroup
    {
        public AdministrationGroup Group { get; set; }
        public List<IAdministrationOptionSection> Sections { get; set; }
    }
}
