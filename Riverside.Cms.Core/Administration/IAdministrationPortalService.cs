using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Riverside.Cms.Core.Administration
{
    public interface IAdministrationPortalService
    {
        List<HamburgerItem> GetHamburgerItems(AdministrationArea area);
    }
}
