using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Administration;
using Riverside.Cms.Core.Webs;
using Riverside.UI.Grids;
using Microsoft.AspNetCore.Mvc;

namespace Riverside.Cms.Core.ApiControllers
{
    public class WebsController : Controller
    {
        private IAuthenticationService _authenticationService;
        private IWebPortalService _webPortalService;

        public WebsController(IAuthenticationService authenticationService, IWebPortalService webPortalService)
        {
            _authenticationService = authenticationService;
            _webPortalService = webPortalService;
        }

        public AdminPageViewModel<Grid> Get(int? page, string search)
        {
            return _webPortalService.GetSearchViewModel(page, search);
        }

        public Grid Get(bool grid, int? page, string search)
        {
            return _webPortalService.GetSearchGrid(page, search);
        }

        public AdminPageViewModel<WebViewModel> Get(bool create)
        {
            return _webPortalService.GetCreateViewModel();
        }

        public WebViewModel Post([FromBody]WebViewModel viewModel)
        {
            return _webPortalService.PostCreateViewModel(viewModel);
        }
    }
}
