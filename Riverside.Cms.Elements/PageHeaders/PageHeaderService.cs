using System;
using System.Collections.Generic;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Elements.Resources;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.PageHeaders
{
    public class PageHeaderService : IAdvancedElementService
    {
        private IPageHeaderRepository _pageHeaderRepository;
        private IPageService _pageService;

        public PageHeaderService(IPageHeaderRepository pageHeaderRepository, IPageService pageService)
        {
            _pageHeaderRepository = pageHeaderRepository;
            _pageService = pageService;
        }

        public Guid ElementTypeId
        {
            get
            {
                return new Guid("1cbac30c-5deb-404e-8ea8-aabc20c82aa8");
            }
        }

        public IElementSettings New(long tenantId)
        {
            return new PageHeaderSettings { TenantId = tenantId, ElementTypeId = ElementTypeId };
        }

        public IElementInfo NewInfo(IElementSettings settings, IElementContent content)
        {
            return new ElementInfo<PageHeaderSettings, PageHeaderContent> { Settings = settings, Content = content };
        }

        public void Create(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _pageHeaderRepository.Create((PageHeaderSettings)settings, unitOfWork);
        }

        public void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null)
        {
            _pageHeaderRepository.Copy(sourceTenantId, sourceElementId, destTenantId, destElementId, unitOfWork);
        }

        public void Read(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _pageHeaderRepository.Read((PageHeaderSettings)settings, unitOfWork);
        }

        public void Update(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _pageHeaderRepository.Update((PageHeaderSettings)settings, unitOfWork);
        }

        public void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null)
        {
            _pageHeaderRepository.Delete(tenantId, elementId, unitOfWork);
        }

        private IList<IPageLink> GetPageHierarchyPageLinks(Page page)
        {
            bool firstPage = true;
            IList<IPageLink> pageLinks = new List<IPageLink>();
            while (page != null)
            {
                PageLinkViewModel pageLink = new PageLinkViewModel
                {
                    Description = page.Name,
                    LinkText = firstPage ? ElementResource.PageHeaderHomeBreadcrumbLabel : page.Name,
                    Page = page
                };
                pageLinks.Add(pageLink);
                page = page.ChildPages.Count == 1 ? page.ChildPages[0] : null;
                firstPage = false;
            }
            return pageLinks;
        }

        public IElementContent GetContent(IElementSettings settings, IPageContext pageContext, IUnitOfWork unitOfWork = null)
        {
            // Construct element content
            PageHeaderContent pageHeaderContent = new PageHeaderContent();
            pageHeaderContent.PartialViewName = "PageHeader";

            // Populate element content page according to element settings
            PageHeaderSettings pageHeaderSettings = (PageHeaderSettings)settings;
            if ((pageHeaderSettings.PageId == null) || (pageHeaderSettings.PageId.Value == pageContext.Page.PageId))
                pageHeaderContent.Page = pageContext.Page;
            else
                pageHeaderContent.Page = _pageService.Read(pageHeaderSettings.PageTenantId.Value, pageHeaderSettings.PageId.Value, unitOfWork);

            // Populate element content breadcrumb navigation according to element settings
            if (pageHeaderSettings.ShowBreadcrumbs)
            {
                Page pageHierarchy = null;
                if ((pageHeaderSettings.PageId == null) || (pageHeaderSettings.PageId.Value == pageContext.Page.PageId))
                    pageHierarchy = pageContext.Hierarchy;
                else
                    pageHierarchy = _pageService.ReadHierarchy(pageHeaderSettings.PageTenantId.Value, pageHeaderSettings.PageId.Value, unitOfWork);
                pageHeaderContent.PageHierarchyPageLinks = GetPageHierarchyPageLinks(pageHierarchy);
            }

            // Return resulting element content
            return pageHeaderContent;
        }
    }
}
