using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Administration
{
    public interface IAdministrationOptionGroup
    {
        AdministrationGroup Group { get; set; }
        List<IAdministrationOptionSection> Sections { get; set; }
    }
}
