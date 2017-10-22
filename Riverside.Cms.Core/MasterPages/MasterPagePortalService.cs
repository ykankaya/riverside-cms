using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Riverside.Cms.Core.Resources;
using Riverside.Cms.Core.Administration;
using Riverside.Cms.Core.Controls;
using Riverside.UI.Controls;
using Riverside.UI.Extensions;
using Riverside.UI.Forms;
using Riverside.UI.Grids;
using Riverside.UI.Routes;
using Riverside.Utilities.Data;
using Riverside.UI.Web;

namespace Riverside.Cms.Core.MasterPages
{
    /// <summary>
    /// Implements master page related portal functionality.
    /// </summary>
    public class MasterPagePortalService : IMasterPagePortalService
    {
        // Member
        private IAdministrationPortalService _administrationPortalService;
        private IGridService _gridService;
        private IMasterPageService _masterPageService;
        private IModelConverter<MasterPage, MasterPageViewModel> _masterPageConverter;
        private IWebHelperService _webHelperService;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="administrationPortalService">Provides generic administration features.</param>
        /// <param name="gridService">Used to construct grid view models.</param>
        /// <param name="masterPageService">Provides access to master pages.</param>
        /// <param name="masterPageConverter">Converts between master page business and view models.</param>
        /// <param name="webHelperService">Provides access to low level web components.</param>
        public MasterPagePortalService(IAdministrationPortalService administrationPortalService, IGridService gridService, IMasterPageService masterPageService, IModelConverter<MasterPage, MasterPageViewModel> masterPageConverter, IWebHelperService webHelperService)
        {
            _administrationPortalService = administrationPortalService;
            _gridService = gridService;
            _masterPageService = masterPageService;
            _masterPageConverter = masterPageConverter;
            _webHelperService = webHelperService;
        }

        /// <summary>
        /// Returns navigation items for actions available for the current page.
        /// </summary>
        /// <param name="action">Action being performed.</param>
        /// <param name="masterPage">Master page model (null if no master page).</param>
        /// <returns>List of navigation items.</returns>
        private List<NavigationItemViewModel> GetActionItems(DataAction action, MasterPage masterPage)
        {
            List<NavigationItemViewModel> navigationItems = new List<NavigationItemViewModel>();
            switch (action)
            {
                case DataAction.Search:
                    navigationItems.Add(new NavigationItemViewModel { Name = MasterPageResource.CreateMasterPageActionLabel, Icon = Icon.Create, Url = _webHelperService.GetUrl(new UrlParameters { ControllerName = "masterpages", ActionName = "create" }) });
                    break;

                case DataAction.Read:
                case DataAction.Update:
                case DataAction.Delete:
                    object routeValues = new { masterpageid = masterPage.MasterPageId };
                    if (action != DataAction.Read)
                        navigationItems.Add(new NavigationItemViewModel { Name = string.Format(MasterPageResource.ReadMasterPageActionLabel, masterPage.Name), Icon = Icon.Read, Url = _webHelperService.GetUrl(new UrlParameters { ControllerName = "masterpages", ActionName = "read", RouteValues = routeValues }) });
                    if (action != DataAction.Update)
                        navigationItems.Add(new NavigationItemViewModel { Name = string.Format(MasterPageResource.UpdateMasterPageActionLabel, masterPage.Name), Icon = Icon.Update, Url = _webHelperService.GetUrl(new UrlParameters { ControllerName = "masterpages", ActionName = "update", RouteValues = routeValues }) });
                    if (action != DataAction.Delete)
                        navigationItems.Add(new NavigationItemViewModel { Name = string.Format(MasterPageResource.DeleteMasterPageActionLabel, masterPage.Name), Icon = Icon.Delete, Url = _webHelperService.GetUrl(new UrlParameters { ControllerName = "masterpages", ActionName = "delete", RouteValues = routeValues }) });
                    break;
            }
            return navigationItems;
        }

        /// <summary>
        /// Gets view model for master page search.
        /// </summary>
        /// <param name="tenantId">Identifies website that master pages belong to.</param>
        /// <param name="page">1-based page index (null if not specified).</param>
        /// <param name="search">Search terms.</param>
        /// <returns>View model for master page search.</returns>
        public AdminPageViewModel<Grid> GetSearchViewModel(long tenantId, int? page, string search)
        {
            // Get breadcrumbs
            List<NavigationItemViewModel> breadcrumbs = new List<NavigationItemViewModel>();
            breadcrumbs.Add(new NavigationItemViewModel
            {
                Name = MasterPageResource.SearchMasterPagesBreadcrumbLabel,
                Url = _webHelperService.GetUrl(new UrlParameters { ControllerName = "masterpages", ActionName = "search" })
            });

            // Construct UI navigation
            NavigationViewModel navigation = new NavigationViewModel
            {
                ActionItems = GetActionItems(DataAction.Search, null),
                BreadcrumbItems = breadcrumbs,
                HamburgerItems = _administrationPortalService.GetHamburgerItems(AdministrationArea.MasterPage)
            };

            // Construct model to return
            AdminPageViewModel<Grid> viewModel = new AdminPageViewModel<Grid>
            {
                Model = GetSearchGrid(tenantId, page, search),
                Navigation = navigation
            };

            // Return result
            return viewModel;
        }

        /// <summary>
        /// Gets search grid.
        /// </summary>
        /// <param name="tenantId">Identifies website that master pages belong to.</param>
        /// <param name="page">1-based page index (null if not specified).</param>
        /// <param name="search">Search terms.</param>
        /// <returns>Grid view model.</returns>
        public Grid GetSearchGrid(long tenantId, int? page, string search)
        {
            // Get layouts that match search parameters
            ISearchParameters searchParameters = new SearchParameters { PageIndex = (page ?? 1) - 1, PageSize = 10, Search = search };
            ISearchResult<MasterPage> searchResult = _masterPageService.Search(tenantId, searchParameters);

            // Construct grid
            List<string> properties = new List<string> { "Name" };
            UrlParameters urlParameters = new UrlParameters { ControllerName = "masterpages", ActionName = "read" };
            List<RoutePropertyPair> routePropertyPairs = new List<RoutePropertyPair> { new RoutePropertyPair { PropertyName = MasterPagePropertyNames.Id, RouteValueName = MasterPagePropertyNames.Id } };
            return _gridService.GetGrid<MasterPage>(searchParameters, searchResult, properties, urlParameters, routePropertyPairs, MasterPageResource.NoMasterPagesMessage);
        }

        /// <summary>
        /// Gets view model for master page read.
        /// </summary>
        /// <param name="tenantId">Identifies website that master pages belong to.</param>
        /// <param name="masterPageId">Identifies master page whose details are returned.</param>
        /// <returns>View model for master page read.</returns>
        public AdminPageViewModel<MasterPageViewModel> Read(long tenantId, long masterPageId)
        {
            // Get master page
            MasterPage masterPage = _masterPageService.Read(tenantId, masterPageId);

            // Get breadcrumbs
            List<NavigationItemViewModel> breadcrumbs = new List<NavigationItemViewModel>();
            breadcrumbs.Add(new NavigationItemViewModel
            {
                Name = MasterPageResource.SearchMasterPagesBreadcrumbLabel,
                Url = _webHelperService.GetUrl(new UrlParameters { ControllerName = "masterpages", ActionName = "search" })
            });
            breadcrumbs.Add(new NavigationItemViewModel
            {
                Name = string.Format(MasterPageResource.ReadMasterPageBreadcrumbLabel, masterPage.Name),
                Url = _webHelperService.GetUrl(new UrlParameters { ControllerName = "masterpages", ActionName = "read", RouteValues = new { masterpageid = masterPage.MasterPageId } })
            });

            // Construct UI navigation
            NavigationViewModel navigation = new NavigationViewModel
            {
                ActionItems = GetActionItems(DataAction.Read, masterPage),
                BreadcrumbItems = breadcrumbs,
                HamburgerItems = _administrationPortalService.GetHamburgerItems(AdministrationArea.MasterPage)
            };

            // Construct model to return
            AdminPageViewModel<MasterPageViewModel> viewModel = new AdminPageViewModel<MasterPageViewModel>
            {
                Model = _masterPageConverter.GetViewModel(masterPage),
                Navigation = navigation
            };

            // Return result
            return viewModel;
        }
    }
}
