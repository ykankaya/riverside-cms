using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Administration;
using Riverside.Cms.Core.Pages;
using Riverside.UI.Grids;
using Microsoft.AspNetCore.Mvc;

namespace Riverside.Cms.Core.ApiControllers
{
    public class PagesController : Controller
    {
        private IAuthenticationService _authenticationService;
        private IPagePortalService _pagePortalService;

        public PagesController(IAuthenticationService authenticationService, IPagePortalService pagePortalService)
        {
            _authenticationService = authenticationService;
            _pagePortalService = pagePortalService;
        }

        public AdminPageViewModel<Grid> Get(int? page, string search)
        {
            return _pagePortalService.GetSearchViewModel(_authenticationService.TenantId, page, search);
        }

        public Grid Get(bool grid, int? page, string search)
        {
            return _pagePortalService.GetSearchGrid(_authenticationService.TenantId, page, search);
        }
    }
}
