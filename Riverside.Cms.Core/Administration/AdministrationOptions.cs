using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authentication;

namespace Riverside.Cms.Core.Administration
{
    public class AdministrationOptions : IAdministrationOptions
    {
        public List<IAdministrationOptionGroup> Groups { get; set; }
        public AuthenticatedUserInfo LoggedOnUser { get; set; }
    }
}
