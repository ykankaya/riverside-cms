using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Riverside.Cms.Core.Resources;
using Riverside.Cms.Core.Administration;
using Riverside.Cms.Core.Controls;
using Riverside.Utilities.Data;
using Riverside.Utilities.Validation;
using Riverside.UI.Controls;
using Riverside.UI.Extensions;
using Riverside.UI.Grids;
using Riverside.UI.Routes;
using Riverside.UI.Forms;
using Riverside.UI.Web;

namespace Riverside.Cms.Core.Webs
{
    /// <summary>
    /// Contains website related portal functionality.
    /// </summary>
    public class WebPortalService : IWebPortalService
    {
        // Member variables
        private IAdministrationPortalService _administrationPortalService;
        private IFormHelperService _formHelperService;
        private IGridService _gridService;
        private IModelConverter<Web, WebViewModel> _webConverter;
        private IWebService _webService;
        private IWebHelperService _webHelperService;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="administrationPortalService">Provides generic administration features.</param>
        /// <param name="formHelperService">Form helper service.</param>
        /// <param name="gridService">Used to construct grid view models.</param>
        /// <param name="webConverter">Converts between web business and web view models.</param>
        /// <param name="webService">Provides access to websites.</param>
        /// <param name="webHelperService">Access to low level web components.</param>
        public WebPortalService(IAdministrationPortalService administrationPortalService, IFormHelperService formHelperService, IGridService gridService, IModelConverter<Web, WebViewModel> webConverter, IWebService webService, IWebHelperService webHelperService)
        {
            _administrationPortalService = administrationPortalService;
            _formHelperService = formHelperService;
            _gridService = gridService;
            _webConverter = webConverter;
            _webService = webService;
            _webHelperService = webHelperService;
        }

        /// <summary>
        /// Gets search grid.
        /// </summary>
        /// <param name="page">1-based page index (null if not specified).</param>
        /// <param name="search">Search terms.</param>
        /// <returns>Grid view model.</returns>
        public Grid GetSearchGrid(int? page, string search)
        {
            // Get layouts that match search parameters
            ISearchParameters searchParameters = new SearchParameters { PageIndex = (page ?? 1) - 1, PageSize = 10, Search = search };
            ISearchResult<Web> searchResult = _webService.Search(searchParameters);

            // Construct grid
            List<string> properties = new List<string> { "Name" };
            UrlParameters urlParameters = new UrlParameters { ControllerName = "webs", ActionName = "read" };
            List<RoutePropertyPair> routePropertyPairs = new List<RoutePropertyPair> { new RoutePropertyPair { PropertyName = WebPropertyNames.Id, RouteValueName = WebPropertyNames.Id } };
            return _gridService.GetGrid<Web>(searchParameters, searchResult, properties, urlParameters, routePropertyPairs, WebResource.NoWebsMessage);
        }

        /// <summary>
        /// Returns actions available for the current page.
        /// </summary>
        /// <param name="action">Action being performed.</param>
        /// <param name="web">Website model (null if no website).</param>
        /// <returns>List of navigation items.</returns>
        private List<NavigationItemViewModel> GetActionItems(DataAction action, Web web)
        {
            List<NavigationItemViewModel> navigationItems = new List<NavigationItemViewModel>();
            switch (action)
            {
                case DataAction.Search:
                    navigationItems.Add(new NavigationItemViewModel { Name = WebResource.CreateWebActionLabel, Icon = Icon.Create, Url = _webHelperService.GetUrl(new UrlParameters { ControllerName = "webs", ActionName = "create" }) });
                    break;

                case DataAction.Read:
                case DataAction.Update:
                case DataAction.Delete:
                    object routeValues = new { tenantId = web.TenantId };
                    if (action != DataAction.Read)
                        navigationItems.Add(new NavigationItemViewModel { Name = string.Format(WebResource.ReadWebActionLabel, web.Name), Icon = Icon.Read, Url = _webHelperService.GetUrl(new UrlParameters { ControllerName = "masterpages", ActionName = "read", RouteValues = routeValues }) });
                    if (action != DataAction.Update)
                        navigationItems.Add(new NavigationItemViewModel { Name = string.Format(WebResource.UpdateWebActionLabel, web.Name), Icon = Icon.Update, Url = _webHelperService.GetUrl(new UrlParameters { ControllerName = "masterpages", ActionName = "update", RouteValues = routeValues }) });
                    if (action != DataAction.Delete)
                        navigationItems.Add(new NavigationItemViewModel { Name = string.Format(WebResource.DeleteWebActionLabel, web.Name), Icon = Icon.Delete, Url = _webHelperService.GetUrl(new UrlParameters { ControllerName = "masterpages", ActionName = "delete", RouteValues = routeValues }) });
                    break;
            }
            return navigationItems;
        }

        /// <summary>
        /// Gets view model for website search.
        /// </summary>
        /// <param name="page">1-based page index (null if not specified).</param>
        /// <param name="search">Search terms.</param>
        /// <returns>View model for website search.</returns>
        public AdminPageViewModel<Grid> GetSearchViewModel(int? page, string search)
        {
            // Get breadcrumbs
            List<NavigationItemViewModel> breadcrumbs = new List<NavigationItemViewModel>();
            breadcrumbs.Add(new NavigationItemViewModel
            {
                Name = WebResource.SearchWebsBreadcrumbLabel,
                Url = _webHelperService.GetUrl(new UrlParameters { ControllerName = "webs", ActionName = "search" })
            });

            // Construct UI navigation
            NavigationViewModel navigation = new NavigationViewModel
            {
                ActionItems = GetActionItems(DataAction.Search, null),
                BreadcrumbItems = breadcrumbs,
                HamburgerItems = _administrationPortalService.GetHamburgerItems(AdministrationArea.Web)
            };

            // Construct model to return
            AdminPageViewModel<Grid> viewModel = new AdminPageViewModel<Grid>
            {
                Model = GetSearchGrid(page, search),
                Navigation = navigation
            };

            // Return result
            return viewModel;
        }

        /// <summary>
        /// Gets view model for website create.
        /// </summary>
        /// <returns>Form model for website create.</returns>
        public AdminPageViewModel<WebViewModel> GetCreateViewModel()
        {
            // Get empty website
            Web web = new Web();

            // Get breadcrumbs
            List<NavigationItemViewModel> breadcrumbs = new List<NavigationItemViewModel>();
            breadcrumbs.Add(new NavigationItemViewModel
            {
                Name = WebResource.SearchWebsBreadcrumbLabel,
                Url = _webHelperService.GetUrl(new UrlParameters { ControllerName = "webs", ActionName = "search" })
            });
            breadcrumbs.Add(new NavigationItemViewModel
            {
                Name = WebResource.CreateWebBreadcrumbLabel,
                Url = _webHelperService.GetUrl(new UrlParameters { ControllerName = "webs", ActionName = "create" })
            });

            // Construct UI navigation
            NavigationViewModel navigation = new NavigationViewModel
            {
                ActionItems = GetActionItems(DataAction.Create, null),
                BreadcrumbItems = breadcrumbs,
                HamburgerItems = _administrationPortalService.GetHamburgerItems(AdministrationArea.Web)
            };

            // Construct model to return
            AdminPageViewModel<WebViewModel> viewModel = new AdminPageViewModel<WebViewModel>
            {
                Model = _webConverter.GetViewModel(web),
                Navigation = navigation
            };

            // Return result
            return viewModel;
        }

        /// <summary>
        /// Handles create website form submission.
        /// </summary>
        /// <param name="viewModel">View model containing new website details.</param>
        /// <returns>View model indicating whether or not action was successful.</returns>
        public WebViewModel PostCreateViewModel(WebViewModel viewModel)
        {
            // Get website details
            Web web = _webConverter.GetModel(viewModel);
            long templateTenantId = Convert.ToInt64(viewModel.Template.Value);

            // Try to do the create
            try
            {
                _webService.Create(templateTenantId, web);
            }
            catch (ValidationErrorException)
            {
            }

            // Return form state
            return viewModel;
        }

        /// <summary>
        /// Gets website action buttons. Button states are determined by the current action being performed.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <param name="action">Action being performed.</param>
        /// <returns>Collection of buttons.</returns>
        private List<Button> GetActionButtons(long tenantId, DataAction action)
        {
            Button readButton = new Button
            {
                Text = WebResource.ReadWebTabLabel,
                Icon = Icon.Read,
                UrlParameters = new UrlParameters { ControllerName = "webs", ActionName = "read", RouteValues = new { tenantId = tenantId } },
                State = action == DataAction.Read ? ButtonState.Active : ButtonState.Enabled
            };
            Button updateButton = new Button
            {
                Text = WebResource.UpdateWebTabLabel,
                Icon = Icon.Update,
                UrlParameters = new UrlParameters { ControllerName = "webs", ActionName = "update", RouteValues = new { tenantId = tenantId } },
                State = action == DataAction.Update ? ButtonState.Active : ButtonState.Enabled
            };
            Button deleteButton = new Button
            {
                Text = WebResource.DeleteWebTabLabel,
                Icon = Icon.Delete,
                UrlParameters = new UrlParameters { ControllerName = "webs", ActionName = "delete", RouteValues = new { tenantId = tenantId } },
                State = action == DataAction.Delete ? ButtonState.Active : ButtonState.Enabled
            };
            return new List<Button> { readButton, updateButton, deleteButton };
        }

        /// <summary>
        /// Get view model for website read, update or delete.
        /// </summary>
        /// <param name="tenantId">Identifies website whose details are returned.</param>
        /// <param name="action">The SCRUD action being performed.</param>
        /// <returns>View model for website read, update or delete.</returns>
        public WebViewModel ReadUpdateDelete(long tenantId, DataAction action)
        {
            return new WebViewModel
            {
                //Buttons = GetActionButtons(tenantId, action),
                //Web = _webService.Read(tenantId)
            };
        }
    }
}
