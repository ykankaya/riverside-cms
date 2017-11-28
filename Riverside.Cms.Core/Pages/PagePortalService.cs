using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Riverside.Cms.Core.Administration;
using Riverside.Cms.Core.Assets;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.MasterPages;
using Riverside.Cms.Core.Resources;
using Riverside.UI.Controls;
using Riverside.UI.Extensions;
using Riverside.UI.Grids;
using Riverside.UI.Routes;
using Riverside.UI.Web;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Pages
{
    /// <summary>
    /// A service that implements page related portal functionality.
    /// </summary>
    public class PagePortalService : IPagePortalService
    {
        // Member variables
        private IAdministrationService _administrationService;
        private IAdministrationPortalService _administrationPortalService;
        private IAssetService _assetService;
        private IAuthenticationService _authenticationService;
        private IElementService _elementService;
        private IGridService _gridService;
        private IMasterPageService _masterPageService;
        private IPageService _pageService;
        private IWebHelperService _webHelperService;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="administrationService">Administration service.</param>
        /// <param name="administrationPortalService">Administration portal service.</param>
        /// <param name="assetService">Assets service.</param>
        /// <param name="authenticationService">Provces access to authentication functions.</param>
        /// <param name="elementService">Provides access to element information.</param>
        /// <param name="gridService">Used to construct grid view models.</param>
        /// <param name="masterPageService">Used for the retrieval of master pages.</param>
        /// <param name="pageService">Provides access to page management functionality.</param>
        /// <param name="webHelperService">Provides low level access to web components.</param>
        public PagePortalService(IAdministrationService administrationService, IAdministrationPortalService administrationPortalService, IAssetService assetService, IAuthenticationService authenticationService, IElementService elementService, IGridService gridService, IMasterPageService masterPageService, IPageService pageService, IWebHelperService webHelperService)
        {
            _administrationService = administrationService;
            _administrationPortalService = administrationPortalService;
            _assetService = assetService;
            _authenticationService = authenticationService;
            _elementService = elementService;
            _gridService = gridService;
            _masterPageService = masterPageService;
            _pageService = pageService;
            _webHelperService = webHelperService;
        }

        /// <summary>
        /// Gets search grid.
        /// </summary>
        /// <param name="tenantId">Identifies website whose pages are searched.</param>
        /// <param name="page">1-based page index (null if not specified).</param>
        /// <param name="search">Search terms.</param>
        /// <returns>Grid view model.</returns>
        public Grid GetSearchGrid(long tenantId, int? page, string search)
        {
            // Get layouts that match search parameters
            ISearchParameters searchParameters = new SearchParameters { PageIndex = (page ?? 1) - 1, PageSize = 10, Search = search };
            ISearchResult<Page> searchResult = _pageService.Search(tenantId, searchParameters);

            // Construct grid
            List<string> properties = new List<string> { "Name" };
            UrlParameters urlParameters = new UrlParameters { ControllerName = "pages", ActionName = "read" };
            List<RoutePropertyPair> routePropertyPairs = new List<RoutePropertyPair> { new RoutePropertyPair { PropertyName = PagePropertyNames.Id, RouteValueName = PagePropertyNames.Id } };
            return _gridService.GetGrid<Page>(searchParameters, searchResult, properties, urlParameters, routePropertyPairs, PageResource.NoPagesMessage);
        }

        /// <summary>
        /// Returns actions available for the current page.
        /// </summary>
        /// <param name="action">Action being performed.</param>
        /// <param name="page">Page model (null if no page).</param>
        /// <returns>List of navigation items.</returns>
        private List<NavigationItemViewModel> GetActionItems(DataAction action, Page page)
        {
            List<NavigationItemViewModel> navigationItems = new List<NavigationItemViewModel>();
            return navigationItems;
        }

        /// <summary>
        /// Gets view model for page search.
        /// </summary>
        /// <param name="tenantId">Identifies website whose pages are searched.</param>
        /// <param name="page">1-based page index (null if not specified).</param>
        /// <param name="search">Search terms.</param>
        /// <returns>View model for page search.</returns>
        public AdminPageViewModel<Grid> GetSearchViewModel(long tenantId, int? page, string search)
        {
            // Get breadcrumbs
            List<NavigationItemViewModel> breadcrumbs = new List<NavigationItemViewModel>();
            breadcrumbs.Add(new NavigationItemViewModel
            {
                Name = PageResource.SearchPagesBreadcrumbLabel,
                Url = _webHelperService.GetUrl(new UrlParameters { ControllerName = "pages", ActionName = "search" })
            });

            // Construct UI navigation
            NavigationViewModel navigation = new NavigationViewModel
            {
                ActionItems = GetActionItems(DataAction.Search, null),
                BreadcrumbItems = breadcrumbs,
                HamburgerItems = _administrationPortalService.GetHamburgerItems(AdministrationArea.Page)
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
        /// Retrieves elements on a page whose content implements IAdministrationContent.
        /// </summary>
        /// <typeparam name="T">Element returned if element content is of this type.</typeparam>
        /// <param name="pageViewModel">The page view model that is searched.</param>
        /// <returns>List of elements that can display administration options.</returns>
        private List<IElementInfo> GetElementsWithContent<T>(PageViewModel pageViewModel)
        {
            List<IElementInfo> elements = new List<IElementInfo>();
            foreach (PageZoneViewModel pageZoneViewModel in pageViewModel.PageZoneViewModels)
            {
                foreach (PageZoneElementViewModel pageZoneElementViewModel in pageZoneViewModel.PageZoneElementViewModels)
                {
                    if (pageZoneElementViewModel.ElementInfo.Content is T)
                        elements.Add(pageZoneElementViewModel.ElementInfo);
                }
            }
            return elements;
        }

        /// <summary>
        /// Populate the content of elements that consume page links with provided page links.
        /// </summary>
        /// <param name="pageViewModel">Page view model.</param>
        private void SetConsumerPageLinks(PageViewModel pageViewModel)
        {
            // Get page link provider elements
            List<IElementInfo> pageLinkProviderElements = GetElementsWithContent<IPageLinkProvider>(pageViewModel);

            // Construct list of all page links provided
            List<IPageLink> pageLinks = new List<IPageLink>();
            foreach (IElementInfo pageLinkProviderElement in pageLinkProviderElements)
            {
                IList<IPageLink> providerPageLinks = ((IPageLinkProvider)pageLinkProviderElement.Content).PageLinks;
                if (providerPageLinks != null)
                    pageLinks.AddRange(providerPageLinks);
            }

            // Get page link consumer elements
            List<IElementInfo> pageLinkConsumerElements = GetElementsWithContent<IPageLinkConsumer>(pageViewModel);
            foreach (IElementInfo pageLinkConsumerElement in pageLinkConsumerElements)
                ((IPageLinkConsumer)pageLinkConsumerElement.Content).PageLinks = pageLinks;
        }

        /// <summary>
        /// Gets page title from page context.
        /// </summary>
        /// <param name="pageContext">Page context.</param>
        /// <returns>Page title.</returns>
        private string GetPageTitle(IPageContext pageContext)
        {
            // When we are on the home page, return the website name as page title
            Page page = pageContext.Hierarchy;
            if (page == null || page.ChildPages == null || page.ChildPages.Count == 0)
                return pageContext.Web.Name;

            // Otherwise construct page title from page hierarchy
            StringBuilder sb = new StringBuilder();
            while (page != null)
            {
                sb.Append(page.Name);
                if (page.ChildPages != null && page.ChildPages.Count > 0)
                {
                    sb.Append(" | ");
                    page = page.ChildPages[0];
                }
                else
                {
                    page = null;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Makes keyword substitutions on layout text.
        /// </summary>
        /// <param name="page">Page information.</param>
        /// <param name="text">Text.</param>
        /// <returns>Text with keywords replaced with dynamic content.</returns>
        private string FormatLayoutHtml(Page page, string text)
        {
            if (page.PreviewImageUploadId.HasValue && text != null && text.Contains("class=\"jumbotron-outer\""))
            {
                string url = _webHelperService.RouteUrl("ReadPageImage", new { pageid = page.PageId, format = "preview", description = _webHelperService.UrlFriendly(page.Name), t = page.PreviewImageUploadId });
                string replaceText = string.Format("class=\"jumbotron-outer\" style=\"background-image: url({0});\"", url);
                text = text.Replace("class=\"jumbotron-outer\"", replaceText);
            }
            return text;
        }

        /// <summary>
        /// Returns keywords that will be used to populate the content attribute of this page's keywords metadata element.
        /// </summary>
        /// <param name="page">Contains page details.</param>
        /// <returns>Metadata keywords (or null if no keywords).</returns>
        private string GetMetaKeywords(Page page)
        {
            if (page.Tags == null || page.Tags.Count == 0)
                return null;
            return string.Join(", ", page.Tags.Select(t => t.Name));
        }

        /// <summary>
        /// Returns description that will be used to populate the content attribute of this page's description metadata element.
        /// </summary>
        /// <param name="page">Contains page details.</param>
        /// <returns>Metadata description (or null if no description).</returns>
        private string GetMetaDescription(Page page)
        {
            return page.Description;
        }

        private string GetForumAction()
        {
            return _webHelperService.Query()["forumaction"];
        }

        /// <summary>
        /// Retrieves page for display.
        /// </summary>
        /// <param name="tenantId">Website whose page is read.</param>
        /// <param name="pageContext">Page context.</param>
        /// <returns>View model required to render a page.</returns>
        public PageViewModel GetPageViewModelFromPageContext(IPageContext pageContext)
        {
            // Construct page view model
            PageViewModel pageViewModel = new PageViewModel
            {
                Administration = pageContext.MasterPage.Administration,
                PageContext = pageContext,
                BeginRender = FormatLayoutHtml(pageContext.Page, pageContext.MasterPage.BeginRender),
                EndRender = FormatLayoutHtml(pageContext.Page, pageContext.MasterPage.EndRender),
                PageZoneViewModels = new List<PageZoneViewModel>(),
                Title = GetPageTitle(pageContext),
                MetaKeywords = GetMetaKeywords(pageContext.Page),
                MetaDescription = GetMetaDescription(pageContext.Page),
                AssetStyleSheetPath = _assetService.GetAssetStyleSheetPath(pageContext.Web.TenantId),
                FontOptionStyleSheetPath = _assetService.GetFontOptionStyleSheetPath(pageContext.Web.TenantId, pageContext.Web.FontOption),
                ColourOptionStyleSheetPath = _assetService.GetColourOptionStyleSheetPath(pageContext.Web.TenantId, pageContext.Web.ColourOption),
                ForumAction = GetForumAction(),
                AssetElementTypes = _assetService.GetAssetElementTypes(pageContext.Web.TenantId),
                StockElementViewPath = "~/Views/Elements",
                AssetElementViewPath = _assetService.GetElementViewPath(pageContext.Web.TenantId)
            };

            // Maintain lists of page elements, master elements and configurable page zones
            List<IElementInfo> pageElements = new List<IElementInfo>();
            List<IElementInfo> masterElements = new List<IElementInfo>();
            List<PageZone> configurablePageZones = new List<PageZone>();

            // Get page zones by master page zone ID
            Dictionary<long, PageZone> pageZonesByMasterPageZoneId = new Dictionary<long, PageZone>();
            foreach (PageZone pageZone in pageContext.Page.PageZones)
                pageZonesByMasterPageZoneId.Add(pageZone.MasterPageZoneId, pageZone);

            // Populate page zone view models
            foreach (MasterPageZone masterPageZone in pageContext.MasterPage.MasterPageZones)
            {
                // Construct page zone view model
                PageZoneViewModel pageZoneViewModel = new PageZoneViewModel
                {
                    BeginRender = FormatLayoutHtml(pageContext.Page, masterPageZone.BeginRender),
                    EndRender = FormatLayoutHtml(pageContext.Page, masterPageZone.EndRender),
                    ContentType = masterPageZone.ContentType,
                    PageZoneElementViewModels = new List<PageZoneElementViewModel>()
                };

                // Get page zone element view models
                PageZone pageZone;
                if (pageZonesByMasterPageZoneId.TryGetValue(masterPageZone.MasterPageZoneId, out pageZone))
                {
                    // Associate master page zone with page zone
                    pageZone.MasterPageZone = masterPageZone;

                    // If master page zone admin type is Editable, sort order is NULL at PageZoneElement level and we must use sort order stored in MasterPageZoneElement
                    Dictionary<long, MasterPageZoneElement> masterPageZoneElementsById = null;
                    if (masterPageZone.AdminType == MasterPageZoneAdminType.Editable)
                    {
                        // If admin type is Editable, then defer to MasterPageZoneElement level for ordering
                        masterPageZoneElementsById = new Dictionary<long, MasterPageZoneElement>();
                        foreach (MasterPageZoneElement masterPageZoneElement in masterPageZone.MasterPageZoneElements)
                            masterPageZoneElementsById.Add(masterPageZoneElement.MasterPageZoneElementId, masterPageZoneElement);
                        foreach (PageZoneElement pageZoneElement in pageZone.PageZoneElements)
                        {
                            MasterPageZoneElement masterPageZoneElement = masterPageZoneElementsById[pageZoneElement.MasterPageZoneElementId.Value];
                            pageZoneElement.SortOrder = masterPageZoneElement.SortOrder;
                        }
                        pageZone.PageZoneElements.Sort(new PageZoneElementComparer());
                    }

                    // Get page zone element view models from page
                    if (masterPageZone.AdminType == MasterPageZoneAdminType.Configurable)
                        configurablePageZones.Add(pageZone);
                    foreach (PageZoneElement pageZoneElement in pageZone.PageZoneElements)
                    {
                        IElementSettings settings = _elementService.Read(pageContext.Web.TenantId, pageZoneElement.ElementTypeId, pageZoneElement.ElementId);
                        IElementContent content = _elementService.GetContent(settings, pageContext);
                        IElementInfo info = _elementService.NewInfo(settings, content);
                        pageElements.Add(info);
                        PageZoneElementViewModel pageZoneElementViewModel = new PageZoneElementViewModel { ElementInfo = info };
                        if (masterPageZone.AdminType == MasterPageZoneAdminType.Editable)
                        {
                            pageZoneElementViewModel.BeginRender = masterPageZoneElementsById[pageZoneElement.MasterPageZoneElementId.Value].BeginRender;
                            pageZoneElementViewModel.EndRender = masterPageZoneElementsById[pageZoneElement.MasterPageZoneElementId.Value].EndRender;
                        }
                        pageZoneViewModel.PageZoneElementViewModels.Add(pageZoneElementViewModel);
                    }
                }
                else
                {
                    // Get page zone element view models from master page
                    foreach (MasterPageZoneElement masterPageZoneElement in masterPageZone.MasterPageZoneElements)
                    {
                        IElementSettings settings = _elementService.Read(pageContext.Web.TenantId, masterPageZoneElement.Element.ElementTypeId, masterPageZoneElement.ElementId);
                        IElementContent content = _elementService.GetContent(settings, pageContext);
                        IElementInfo info = _elementService.NewInfo(settings, content);
                        masterElements.Add(info);
                        pageZoneViewModel.PageZoneElementViewModels.Add(new PageZoneElementViewModel { ElementInfo = info, BeginRender = masterPageZoneElement.BeginRender, EndRender = masterPageZoneElement.EndRender });
                    }
                }

                // Register page zone view model
                pageViewModel.PageZoneViewModels.Add(pageZoneViewModel);
            }

            // Pass administration options to elements that can display them
            IAdministrationOptions options = _administrationService.GetAdministrationOptions(pageContext, pageElements, masterElements, configurablePageZones);
            List<IElementInfo> administrationElements = GetElementsWithContent<IAdministrationContent>(pageViewModel);
            foreach (IElementInfo elementInfo in administrationElements)
                ((IAdministrationContent)elementInfo.Content).Options = options;
            SetConsumerPageLinks(pageViewModel);
            pageViewModel.Options = options;

            // Return the resulting page view model
            return pageViewModel;
        }

        /// <summary>
        /// Gets query string parameters.
        /// </summary>
        /// <returns>A dictionary of string to string, representing query string parameters.</returns>
        private IDictionary<string, string> GetQueryStringParameters()
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            foreach (KeyValuePair<string, StringValues> kvp in _webHelperService.Query())
                parameters.Add(kvp.Key, kvp.Value[0]);
            return parameters;
        }

        /// <summary>
        /// Gets route values.
        /// </summary>
        /// <returns>Route value dictionary.</returns>
        private IDictionary<string, object> GetRouteValues()
        {
            IDictionary<string, object> routeValues = new Dictionary<string, object>();
            RouteValueDictionary rvd = _webHelperService.GetRouteValueDictionary();
            foreach (KeyValuePair<string, object> kvp in rvd)
                routeValues.Add(kvp.Key, kvp.Value);
            return routeValues;
        }

        /// <summary>
        /// Retrieves page for display.
        /// </summary>
        /// <param name="tenantId">Website whose page is read.</param>
        /// <param name="pageId">Unique page identifier.</param>
        /// <returns>View model required to render a page.</returns>
        public PageViewModel GetReadPagePageViewModel(long tenantId, long pageId)
        {
            // Retrieve page context, master page and layout
            IList<Tag> tags = GetTags();
            IDictionary<string, string> parameters = GetQueryStringParameters();
            IPageContext pageContext = _administrationService.GetPageContext(tenantId, pageId, parameters, tags);
            pageContext.RouteValues = GetRouteValues();

            // Get the page view model
            PageViewModel pageViewModel = GetPageViewModelFromPageContext(pageContext);

            // Return result
            return pageViewModel;
        }

        /// <summary>
        /// Retrieves home page for display.
        /// </summary>
        /// <param name="tenantId">Website whose page is read.</param>
        /// <returns>View model required to render a page.</returns>
        public PageViewModel GetHomePagePageViewModel(long tenantId)
        {
            // Get home page identifier
            long pageId = _pageService.ReadHome(tenantId).PageId;

            // Return page view model for home page
            return GetReadPagePageViewModel(tenantId, pageId);
        }

        /// <summary>
        /// Retrieves page for update theme administration action. Administration content dynamically inserted.
        /// </summary>
        /// <param name="tenantId">Website whose admin page is displayed.</param>
        /// <returns>View model required to render the admin page.</returns>
        public PageViewModel GetUpdateThemePageViewModel(long tenantId)
        {
            // Retrieve page context, master page and layout
            IList<Tag> tags = GetTags();
            IDictionary<string, string> parameters = GetQueryStringParameters();
            IPageContext pageContext = _administrationService.GetUpdateThemeContext(tenantId, parameters, tags);
            pageContext.RouteValues = GetRouteValues();

            // Get the page view model
            PageViewModel pageViewModel = GetPageViewModelFromPageContext(pageContext);

            // Return result
            return pageViewModel;
        }

        /// <summary>
        /// Retrieves page for create page administration action. Administration content dynamically inserted.
        /// </summary>
        /// <param name="tenantId">Website whose admin page is displayed.</param>
        /// <param name="masterPageId">The type of page that is to be created.</param>
        /// <returns>View model required to render the admin page.</returns>
        public PageViewModel GetCreatePagePageViewModel(long tenantId, long masterPageId)
        {
            // Retrieve page context, master page and layout
            IList<Tag> tags = GetTags();
            IDictionary<string, string> parameters = GetQueryStringParameters();
            IPageContext pageContext = _administrationService.GetCreatePageContext(tenantId, masterPageId, parameters, tags);
            pageContext.RouteValues = GetRouteValues();

            // Get the page view model
            PageViewModel pageViewModel = GetPageViewModelFromPageContext(pageContext);

            // Return result
            return pageViewModel;
        }

        /// <summary>
        /// Retrieves page for update page administration action. Administration content dynamically inserted.
        /// </summary>
        /// <param name="tenantId">Website whose admin page is displayed.</param>
        /// <param name="pageId">The page that is being updated.</param>
        /// <returns>View model required to render the admin page.</returns>
        public PageViewModel GetUpdatePagePageViewModel(long tenantId, long pageId)
        {
            // Retrieve page context, master page and layout
            IList<Tag> tags = GetTags();
            IDictionary<string, string> parameters = GetQueryStringParameters();
            IPageContext pageContext = _administrationService.GetUpdatePageContext(tenantId, pageId, parameters, tags);
            pageContext.RouteValues = GetRouteValues();

            // Get the page view model
            PageViewModel pageViewModel = GetPageViewModelFromPageContext(pageContext);

            // Return result
            return pageViewModel;
        }

        /// <summary>
        /// Gets page view model for creating master pages.
        /// </summary>
        /// <param name="tenantId">Website whose admin page is displayed.</param>
        /// <returns>View model required to render the admin page.</returns>
        public PageViewModel GetCreateMasterPagePageViewModel(long tenantId)
        {
            // Retrieve page context, master page and layout
            IList<Tag> tags = GetTags();
            IDictionary<string, string> parameters = GetQueryStringParameters();
            IPageContext pageContext = _administrationService.GetCreateMasterPageContext(tenantId, parameters, tags);
            pageContext.RouteValues = GetRouteValues();

            // Get the page view model
            PageViewModel pageViewModel = GetPageViewModelFromPageContext(pageContext);

            // Return result
            return pageViewModel;
        }

        /// <summary>
        /// Gets page view model for update of master page.
        /// </summary>
        /// <param name="tenantId">Website whose admin page is displayed.</param>
        /// <param name="masterPageId">The master page that is being updated.</param>
        /// <returns>View model required to render the admin page.</returns>
        public PageViewModel GetUpdateMasterPagePageViewModel(long tenantId, long masterPageId)
        {
            // Retrieve page context, master page and layout
            IList<Tag> tags = GetTags();
            IDictionary<string, string> parameters = GetQueryStringParameters();
            IPageContext pageContext = _administrationService.GetUpdateMasterPageContext(tenantId, masterPageId, parameters, tags);
            pageContext.RouteValues = GetRouteValues();

            // Get the page view model
            PageViewModel pageViewModel = GetPageViewModelFromPageContext(pageContext);

            // Return result
            return pageViewModel;
        }

        /// <summary>
        /// Gets page view model for update of master page zone.
        /// </summary>
        /// <param name="tenantId">Website whose admin page is displayed.</param>
        /// <param name="masterPageId">The master page whose zone is being updated.</param>
        /// <param name="masterPageZoneId">The master page zone that is being updated.</param>
        /// <returns>View model required to render the admin page.</returns>
        public PageViewModel GetUpdateMasterPageZonePageViewModel(long tenantId, long masterPageId, long masterPageZoneId)
        {
            // Retrieve page context, master page and layout
            IList<Tag> tags = GetTags();
            IDictionary<string, string> parameters = GetQueryStringParameters();
            IPageContext pageContext = _administrationService.GetUpdateMasterPageZoneContext(tenantId, masterPageId, masterPageZoneId, parameters, tags);
            pageContext.RouteValues = GetRouteValues();

            // Get the page view model
            PageViewModel pageViewModel = GetPageViewModelFromPageContext(pageContext);

            // Return result
            return pageViewModel;
        }

        /// <summary>
        /// Gets page view model for add and remove of master page zones.
        /// </summary>
        /// <param name="tenantId">Website whose admin page is displayed.</param>
        /// <param name="masterPageId">The master page whose zones are being updated.</param>
        /// <returns>View model required to render the admin page.</returns>
        public PageViewModel GetUpdateMasterPageZonesPageViewModel(long tenantId, long masterPageId)
        {
            // Retrieve page context, master page and layout
            IList<Tag> tags = GetTags();
            IDictionary<string, string> parameters = GetQueryStringParameters();
            IPageContext pageContext = _administrationService.GetUpdateMasterPageZonesContext(tenantId, masterPageId, parameters, tags);
            pageContext.RouteValues = GetRouteValues();

            // Get the page view model
            PageViewModel pageViewModel = GetPageViewModelFromPageContext(pageContext);

            // Return result
            return pageViewModel;
        }

        /// <summary>
        /// Retrieves page for update page element administration action. Administration content dynamically inserted.
        /// </summary>
        /// <param name="tenantId">The website whose element is being updated.</param>
        /// <param name="pageId">The page that we were on when update element action was selected.</param>
        /// <param name="elementId">Identifies element being updated.</param>
        /// <returns>View model required to render the admin page.</returns>
        public PageViewModel GetUpdatePageElementPageViewModel(long tenantId, long pageId, long elementId)
        {
            // Retrieve page context, master page and layout
            IList<Tag> tags = GetTags();
            IDictionary<string, string> parameters = GetQueryStringParameters();
            parameters.Add("masterpageid", string.Empty);
            parameters.Add("pageid", pageId.ToString());
            IPageContext pageContext = _administrationService.GetUpdatePageElementContext(tenantId, pageId, elementId, parameters, tags);
            pageContext.RouteValues = GetRouteValues();

            // Get the page view model
            PageViewModel pageViewModel = GetPageViewModelFromPageContext(pageContext);

            // Return result
            return pageViewModel;
        }

        /// <summary>
        /// Retrieves page for update master page element administration action. Administration content dynamically inserted.
        /// </summary>
        /// <param name="tenantId">The website whose element is being updated.</param>
        /// <param name="masterPageId">The master page that we were on when update element action was selected.</param>
        /// <param name="elementId">Identifies element being updated.</param>
        /// <returns>View model required to render the admin page.</returns>
        public PageViewModel GetUpdateMasterPageElementPageViewModel(long tenantId, long masterPageId, long elementId)
        {
            // Retrieve page context, master page and layout
            IList<Tag> tags = GetTags();
            IDictionary<string, string> parameters = GetQueryStringParameters();
            parameters.Add("masterpageid", masterPageId.ToString());
            parameters.Add("pageid", string.Empty);
            IPageContext pageContext = _administrationService.GetUpdateMasterPageElementContext(tenantId, masterPageId, elementId, parameters, tags);
            pageContext.RouteValues = GetRouteValues();

            // Get the page view model
            PageViewModel pageViewModel = GetPageViewModelFromPageContext(pageContext);

            // Return result
            return pageViewModel;
        }

        /// <summary>
        /// Retrieves page for update page zone administration action. Administration content dynamically inserted.
        /// </summary>
        /// <param name="tenantId">The website whose page zone is being updated.</param>
        /// <param name="pageId">The page that we were on when update page zone action was selected.</param>
        /// <param name="pageZoneId">Identifies page zone being updated.</param>
        /// <returns>View model required to render the admin page.</returns>
        public PageViewModel GetUpdatePageZonePageViewModel(long tenantId, long pageId, long pageZoneId)
        {
            // Retrieve page context, master page and layout
            IList<Tag> tags = GetTags();
            IDictionary<string, string> parameters = GetQueryStringParameters();
            IPageContext pageContext = _administrationService.GetUpdatePageZoneContext(tenantId, pageId, pageZoneId, parameters, tags);
            pageContext.RouteValues = GetRouteValues();

            // Get the page view model
            PageViewModel pageViewModel = GetPageViewModelFromPageContext(pageContext);

            // Return result
            return pageViewModel;
        }

        /// <summary>
        /// Gets a page view model for the register user page.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <returns>View model required to render page.</returns>
        public PageViewModel GetCreateUserPageViewModel(long tenantId)
        {
            // Retrieve page context, master page and layout
            IList<Tag> tags = GetTags();
            IDictionary<string, string> parameters = GetQueryStringParameters();
            IPageContext pageContext = _administrationService.GetCreateUserContext(tenantId, parameters, tags);
            pageContext.RouteValues = GetRouteValues();

            // Get the page view model
            PageViewModel pageViewModel = GetPageViewModelFromPageContext(pageContext);

            // Return result
            return pageViewModel;
        }

        /// <summary>
        /// Gets a page view model for the confirm user set password page.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <returns>View model required to render page.</returns>
        public PageViewModel GetConfirmUserSetPasswordPageViewModel(long tenantId)
        {
            // Retrieve page context, master page and layout
            IList<Tag> tags = GetTags();
            IDictionary<string, string> parameters = GetQueryStringParameters();
            IPageContext pageContext = _administrationService.GetConfirmUserSetPasswordContext(tenantId, parameters, tags);
            pageContext.RouteValues = GetRouteValues();

            // Get the page view model
            PageViewModel pageViewModel = GetPageViewModelFromPageContext(pageContext);

            // Return result
            return pageViewModel;
        }

        /// <summary>
        /// Gets a page view model for the confirm user page.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <returns>View model required to render page.</returns>
        public PageViewModel GetConfirmUserPageViewModel(long tenantId)
        {
            // Retrieve page context, master page and layout
            IList<Tag> tags = GetTags();
            IDictionary<string, string> parameters = GetQueryStringParameters();
            IPageContext pageContext = _administrationService.GetConfirmUserContext(tenantId, parameters, tags);
            pageContext.RouteValues = GetRouteValues();

            // Get the page view model
            PageViewModel pageViewModel = GetPageViewModelFromPageContext(pageContext);

            // Return result
            return pageViewModel;
        }

        /// <summary>
        /// Gets a page view model for the logon page.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <returns>View model required to render page.</returns>
        public PageViewModel GetLogonUserPageViewModel(long tenantId)
        {
            // Retrieve page context, master page and layout
            IList<Tag> tags = GetTags();
            IDictionary<string, string> parameters = GetQueryStringParameters();
            IPageContext pageContext = _administrationService.GetLogonUserContext(tenantId, parameters, tags);
            pageContext.RouteValues = GetRouteValues();

            // Get the page view model
            PageViewModel pageViewModel = GetPageViewModelFromPageContext(pageContext);

            // Return result
            return pageViewModel;
        }

        /// <summary>
        /// Gets a page view model for the change password page.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <returns>View model required to render page.</returns>
        public PageViewModel GetChangePasswordPageViewModel(long tenantId)
        {
            // Retrieve page context, master page and layout
            IDictionary<string, string> parameters = GetQueryStringParameters();
            IList<Tag> tags = GetTags();
            IPageContext pageContext = _administrationService.GetChangePasswordContext(tenantId, parameters, tags);
            pageContext.RouteValues = GetRouteValues();

            // Get the page view model
            PageViewModel pageViewModel = GetPageViewModelFromPageContext(pageContext);

            // Return result
            return pageViewModel;
        }

        /// <summary>
        /// Gets a page view model for the updating a user profile.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <returns>View model required to render page.</returns>
        public PageViewModel GetUpdateUserPageViewModel(long tenantId)
        {
            // Retrieve page context, master page and layout
            IList<Tag> tags = GetTags();
            IDictionary<string, string> parameters = GetQueryStringParameters();
            IPageContext pageContext = _administrationService.GetUpdateUserContext(tenantId, parameters, tags);
            pageContext.RouteValues = GetRouteValues();

            // Get the page view model
            PageViewModel pageViewModel = GetPageViewModelFromPageContext(pageContext);

            // Return result
            return pageViewModel;
        }

        /// <summary>
        /// Gets a page view model for initiating a password reset.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <returns>View model required to render page.</returns>
        public PageViewModel GetForgottenPasswordPageViewModel(long tenantId)
        {
            // Retrieve page context, master page and layout
            IList<Tag> tags = GetTags();
            IDictionary<string, string> parameters = GetQueryStringParameters();
            IPageContext pageContext = _administrationService.GetForgottenPasswordContext(tenantId, parameters, tags);
            pageContext.RouteValues = GetRouteValues();

            // Get the page view model
            PageViewModel pageViewModel = GetPageViewModelFromPageContext(pageContext);

            // Return result
            return pageViewModel;
        }

        /// <summary>
        /// Gets a page view model for reseting a user's password.
        /// </summary>
        /// <param name="tenantId">Website identifier.</param>
        /// <returns>View model required to render page.</returns>
        public PageViewModel GetResetPasswordPageViewModel(long tenantId)
        {
            // Retrieve page context, master page and layout
            IList<Tag> tags = GetTags();
            IDictionary<string, string> parameters = GetQueryStringParameters();
            IPageContext pageContext = _administrationService.GetResetPasswordContext(tenantId, parameters, tags);
            pageContext.RouteValues = GetRouteValues();

            // Get the page view model
            PageViewModel pageViewModel = GetPageViewModelFromPageContext(pageContext);

            // Return result
            return pageViewModel;
        }

        /// <summary>
        /// Looks at URL to get tags. Returns list of tags that are valid.
        /// </summary>
        /// <returns>List of tags.</returns>
        private IList<Tag> GetTags()
        {
            // Get list of unverified tags from tags text on URL
            IQueryCollection query = _webHelperService.Query();
            string tags = null;
            if (query.ContainsKey("tags"))
                tags = query["tags"][0];
            IList<Tag> unverifiedTags = GetTagsFromTextString(new string[] { TagVariables.Separator }, tags);

            // Get tags whose names match actual tags in the database
            long tenantId = _authenticationService.TenantId;
            return _pageService.ListNamedTags(tenantId, unverifiedTags.Select(t => t.Name).ToList());
        }

        /// <summary>
        /// Converts tags in a test string into a list of strongly typed tags.
        /// Credit: http://stackoverflow.com/questions/1547476/easiest-way-to-split-a-string-on-newlines-in-net (see Guffa's answer).
        /// </summary>
        /// <param name="separators">Separators used to split text string of tags.</param>
        /// <param name="text">Text string containing tags.</param>
        /// <returns>List of tags.</returns>
        public IList<Tag> GetTagsFromTextString(string[] separators, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new List<Tag>();
            List<Tag> tags = text.Split(separators, StringSplitOptions.None).Select(t => t.Trim().Replace(TagVariables.Separator, string.Empty)).Distinct().Where(t => t != string.Empty).Select(t => new Tag { Name = t }).ToList();
            return tags;
        }

        /// <summary>
        /// Converts list of tags into a space separated string of tag names.
        /// </summary>
        /// <param name="separator">Separator used to split tags.</param>
        /// <param name="tags">List of tags.</param>
        /// <returns>Space separated list of tag names.</returns>
        public string GetTagsAsTextString(string separator, IList<Tag> tags)
        {
            if (tags == null || tags.Count == 0)
                return null;
            return string.Join(separator, tags.Select(t => t.Name));
        }

        /// <summary>
        /// Gets URL for page.
        /// </summary>
        /// <param name="pageId">The page whose URL is returned.</param>
        /// <param name="name">Page name.</param>
        /// <returns>URL for page.</returns>
        public string GetPageUrl(long pageId, string name)
        {
            string description = _webHelperService.UrlFriendly(name);
            return _webHelperService.RouteUrl("ReadPage", new { pageid = pageId, description = description });
        }

        /// <summary>
        /// Gets URL for home page.
        /// </summary>
        /// <returns>URL for page.</returns>
        public string GetHomeUrl()
        {
            return _webHelperService.RouteUrl("HomePage");
        }
    }
}
