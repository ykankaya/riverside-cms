using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Administration;
using Riverside.Cms.Core.MasterPages;
using Riverside.UI.Grids;
using Microsoft.AspNetCore.Mvc;

namespace Riverside.Cms.Core.ApiControllers
{
    public class MasterPagesController : Controller
    {
        private IAuthenticationService _authenticationService;
        private IMasterPagePortalService _masterPagePortalService;

        public MasterPagesController(IAuthenticationService authenticationService, IMasterPagePortalService masterPagePortalService)
        {
            _authenticationService = authenticationService;
            _masterPagePortalService = masterPagePortalService;
        }

        public AdminPageViewModel<Grid> Get(int? page, string search)
        {
            return _masterPagePortalService.GetSearchViewModel(_authenticationService.TenantId, page, search);
        }

        public Grid Get(bool grid, int? page, string search)
        {
            return _masterPagePortalService.GetSearchGrid(_authenticationService.TenantId, page, search);
        }

        public AdminPageViewModel<MasterPageViewModel> Get(long id)
        {
            return _masterPagePortalService.Read(_authenticationService.TenantId, id);
        }
    }
}
