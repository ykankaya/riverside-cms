using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using Riverside.Utilities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.NavBars
{
    public class NavBarService : IAdvancedElementService, INavigationElementService
    {
        private INavBarRepository _navBarRepository;
        private IPageService _pageService;

        public NavBarService(INavBarRepository navBarRepository, IPageService pageService)
        {
            _navBarRepository = navBarRepository;
            _pageService = pageService;
        }

        public Guid ElementTypeId
        {
            get
            {
                return new Guid("a94c34c0-1a4c-4c91-a669-2f830cf1ea5f");
            }
        }

        public IElementSettings New(long tenantId)
        {
            return new NavBarSettings { TenantId = tenantId, ElementTypeId = ElementTypeId, Tabs = new List<NavBarTab>() };
        }

        public IElementInfo NewInfo(IElementSettings settings, IElementContent content)
        {
            return new ElementInfo<NavBarSettings, NavBarContent> { Settings = settings, Content = content };
        }

        public void Create(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _navBarRepository.Create((NavBarSettings)settings, unitOfWork);
        }

        public void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null)
        {
            _navBarRepository.Copy(sourceTenantId, sourceElementId, destTenantId, destElementId, unitOfWork);
        }

        public void Read(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _navBarRepository.Read((NavBarSettings)settings, unitOfWork);
        }

        public void Update(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _navBarRepository.Update((NavBarSettings)settings, unitOfWork);
        }

        public void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null)
        {
            _navBarRepository.Delete(tenantId, elementId, unitOfWork);
        }

        public IElementContent GetContent(IElementSettings settings, IPageContext pageContext, IUnitOfWork unitOfWork = null)
        {
            // Construct nav bar content
            NavBarContent navBarContent = new NavBarContent { PartialViewName = "NavBar", PageTabs = new List<NavBarPageTab>() };

            // Get full page information for pages identified in nav bar tabs
            NavBarSettings navBarSettings = (NavBarSettings)settings;
            foreach (NavBarTab tab in navBarSettings.Tabs)
            {
                Page page = _pageService.Read(tab.TenantId, tab.PageId, unitOfWork);
                if (page != null)
                {
                    Page pageHierarchy = pageContext.Hierarchy;
                    bool active = false;
                    while (!active && pageHierarchy != null)
                    {
                        active = pageHierarchy.PageId == page.PageId;
                        pageHierarchy = pageHierarchy.ChildPages.Count == 0 ? null : pageHierarchy.ChildPages[0];
                    }
                    navBarContent.PageTabs.Add(new NavBarPageTab { Page = page, Name = tab.Name, Active = active });
                }
            }

            // Return nav bar content
            return navBarContent;
        }

        public void AddNavigationPages(long tenantId, long elementId, List<Page> navigationPages, IUnitOfWork unitOfWork = null)
        {
            // Get nav bar settings
            NavBarSettings navBarSettings = (NavBarSettings)New(tenantId);
            navBarSettings.ElementId = elementId;

            // Populate nav bar settings
            _navBarRepository.Read(navBarSettings, unitOfWork);

            // Add navigation pages
            int sortOrder = navBarSettings.Tabs.Count;
            foreach (Page page in navigationPages)
            {
                sortOrder++;
                navBarSettings.Tabs.Add(new NavBarTab
                {
                    ElementId = elementId,
                    Name = page.Name,
                    NavBarTabId = 0,
                    PageId = page.PageId,
                    TenantId = page.TenantId,
                    SortOrder = sortOrder
                });
            }

            // Update nav bar settings
            _navBarRepository.Update(navBarSettings, unitOfWork);
        }
    }
}
