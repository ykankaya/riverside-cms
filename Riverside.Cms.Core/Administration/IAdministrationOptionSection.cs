using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Administration
{
    public interface IAdministrationOptionSection
    {
        string Name { get; set; }
        List<IAdministrationOption> Options { get; set; }
    }
}
