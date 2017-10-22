using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Routing;
using Riverside.Cms.Core.Resources;
using Riverside.UI.Extensions;
using Riverside.UI.Routes;
using Riverside.UI.Web;

namespace Riverside.Cms.Core.Administration
{
    public class AdministrationPortalService : IAdministrationPortalService
    {
        private IWebHelperService _webHelperService;

        public AdministrationPortalService(IWebHelperService webHelperService)
        {
            _webHelperService = webHelperService;
        }

        public List<HamburgerItem> GetHamburgerItems(AdministrationArea area)
        {
            List<HamburgerItem> items = new List<HamburgerItem>();
            items.Add(new HamburgerItem { Name = AdministrationResource.HamburgerWebsitesLabel, Icon = "fa-tachometer", Url = _webHelperService.GetUrl(new UrlParameters { ActionName = "search", ControllerName = "webs" }), Active = area == AdministrationArea.Web });
            items.Add(new HamburgerItem { Name = AdministrationResource.HamburgerDomainsLabel, Icon = "fa-globe", Url = _webHelperService.GetUrl(new UrlParameters { ActionName = "search", ControllerName = "domains" }), Active = area == AdministrationArea.Domain });
            items.Add(new HamburgerItem { Name = AdministrationResource.HamburgerZonesLabel, Icon = "fa-th", Url = _webHelperService.GetUrl(new UrlParameters { ActionName = "search", ControllerName = "zones" }), Active = area == AdministrationArea.Zone });
            items.Add(new HamburgerItem { Name = AdministrationResource.HamburgerLayoutsLabel, Icon = "fa-object-group", Url = _webHelperService.GetUrl(new UrlParameters { ActionName = "search", ControllerName = "layouts" }), Active = area == AdministrationArea.Layout });
            items.Add(new HamburgerItem { Name = AdministrationResource.HamburgerPagesLabel, Icon = "fa-files-o", Url = _webHelperService.GetUrl(new UrlParameters { ActionName = "search", ControllerName = "pages" }), Active = area == AdministrationArea.Page });
            items.Add(new HamburgerItem { Name = AdministrationResource.HamburgerUsersLabel, Icon = "fa-users", Url = _webHelperService.GetUrl(new UrlParameters { ActionName = "search", ControllerName = "users" }), Active = area == AdministrationArea.User });
            items.Add(new HamburgerItem { Name = AdministrationResource.HamburgerAccountLabel, Icon = "fa-user", Url = _webHelperService.GetUrl(new UrlParameters { ActionName = "search", ControllerName = "accounts" }), Active = area == AdministrationArea.Account });
            return items;
        }
    }
}
