using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Elements.Forums;
using Riverside.Utilities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.UI.Routes;
using Riverside.UI.Controls;

namespace Riverside.Cms.Elements.PageList
{
    public class PageListService : IAdvancedElementService
    {
        // Member variables
        private IPageListRepository _pageListRepository;
        private IForumRepository _forumRepository;
        private IPageService _pageService;

        public PageListService(IForumRepository forumRepository, IPageListRepository pageListRepository, IPageService pageService)
        {
            _forumRepository = forumRepository;
            _pageListRepository = pageListRepository;
            _pageService = pageService;
        }

        public Guid ElementTypeId
        {
            get
            {
                return new Guid("61f55535-9f3e-4ef5-96a2-bc84d648842a");
            }
        }

        public IElementSettings New(long tenantId)
        {
            return new PageListSettings { TenantId = tenantId, ElementTypeId = ElementTypeId };
        }

        public IElementInfo NewInfo(IElementSettings settings, IElementContent content)
        {
            return new ElementInfo<PageListSettings, PageListContent> { Settings = settings, Content = content };
        }

        public void Create(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _pageListRepository.Create((PageListSettings)settings, unitOfWork);
        }

        public void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null)
        {
            _pageListRepository.Copy(sourceTenantId, sourceElementId, destTenantId, destElementId, unitOfWork);
        }

        public void Read(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _pageListRepository.Read((PageListSettings)settings, unitOfWork);
        }

        public void Update(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            // Get page list typed settings
            PageListSettings pageListSettings = (PageListSettings)settings;

            // Set non mandatory text fields null if they only contain white space
            if (string.IsNullOrWhiteSpace(pageListSettings.DisplayName))
                pageListSettings.DisplayName = null;
            if (string.IsNullOrWhiteSpace(pageListSettings.MoreMessage))
                pageListSettings.MoreMessage = null;
            if (string.IsNullOrWhiteSpace(pageListSettings.Preamble))
                pageListSettings.Preamble = null;
            if (string.IsNullOrWhiteSpace(pageListSettings.NoPagesMessage))
                pageListSettings.NoPagesMessage = null;

            // Do the update
            _pageListRepository.Update(pageListSettings, unitOfWork);
        }

        public void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null)
        {
            _pageListRepository.Delete(tenantId, elementId, unitOfWork);
        }

        /// <summary>
        /// Returns page whose pages are enumerated. Page returned determined by rules executed in the following order:
        ///  1. If page specified in page list settings, then use this page
        ///  2. If page not specified in page list settings and current page a folder, then use current page
        ///  3. If page not specified in page list settings and current page a document, then use current page parent
        /// Case 3 ensures that we list documents even at the leaves of the page hierarchy.
        /// </summary>
        /// <param name="pageListSettings">Page list settings.</param>
        /// <param name="pageContext">Page context.</param>
        /// <returns>The page whose child pages are enumerated.</returns>
        private long GetPageWhosePagesAreListed(PageListSettings pageListSettings, IPageContext pageContext)
        {
            // If page is specified in page list settings, then always use that page
            if (pageListSettings.PageId != null)
                return pageListSettings.PageId.Value;

            // If page not specified, then use current page if the current page is a folder
            if (pageContext.MasterPage.PageType == PageType.Folder)
                return pageContext.Page.PageId;

            // If page not specified and current page is document, then return document's parent folder
            Page page = pageContext.Hierarchy;
            Page documentParent = page;
            if (page.ChildPages.Count > 0)
            {
                while (page.ChildPages[0].ChildPages.Count > 0)
                {
                    page = page.ChildPages[0];
                    documentParent = page;
                }
            }
            return documentParent.PageId;
        }

        public IElementContent GetContent(IElementSettings settings, IPageContext pageContext, IUnitOfWork unitOfWork = null)
        {
            // Get 1-based page index
            int pageIndex = 1;
            if (pageContext.Parameters.ContainsKey("page"))
                Int32.TryParse(pageContext.Parameters["page"], out pageIndex);
            pageIndex = Math.Max(pageIndex, 1);

            // Retrieve list of pages
            PageListSettings pageListSettings = (PageListSettings)settings;
            ISearchParameters parameters = new SearchParameters { PageIndex = pageIndex - 1, PageSize = pageListSettings.PageSize };
            long tenantId = pageListSettings.PageTenantId ?? pageContext.Page.TenantId;
            long pageId = GetPageWhosePagesAreListed(pageListSettings, pageContext);
            ISearchResult<Page> result = null;
            if (pageContext.Tags.Count == 0)
                result = _pageService.List(tenantId, parameters, pageId, pageListSettings.SortBy, pageListSettings.SortAsc, pageListSettings.Recursive, pageListSettings.PageType, pageListSettings.ShowTags, unitOfWork);
            else
                result = _pageService.ListTagged(tenantId, parameters, pageContext.Tags, pageId, pageListSettings.SortBy, pageListSettings.SortAsc, pageListSettings.Recursive, pageListSettings.PageType, pageListSettings.ShowTags, unitOfWork);

            // If required, get parent page details whose child pages are displayed
            Page page = null;
            if (pageListSettings.MoreMessage != null)
            {
                if ((pageListSettings.PageId == null) || (pageListSettings.PageId.Value == pageContext.Page.PageId))
                    page = pageContext.Page;
                else
                    page = _pageService.Read(pageListSettings.PageTenantId.Value, pageListSettings.PageId.Value, unitOfWork);
            }

            // Get comment counts?
            IDictionary<long, int> commentCounts = null;
            if (pageListSettings.ShowComments)
                commentCounts = _forumRepository.ListPageForumCounts(tenantId, result.Items, unitOfWork);
            else
                commentCounts = new Dictionary<long, int>();

            // Setup pager
            UrlParameters urlParameters = new UrlParameters { RouteName = "ReadPage", RouteValues = new { pageid = pageContext.Page.PageId } };
            Pager pager = new Pager { PageSize = pageListSettings.PageSize, Total = result.Total, PageIndex = pageIndex, UrlParameters = urlParameters };

            // Get display name
            string displayName = pageListSettings.DisplayName;
            if (displayName != null && pageContext.Tags.Count > 0)
                displayName += " " + string.Join(" ", pageContext.Tags.Select(t => "#" + t.Name));

            // Return page list content
            return new PageListContent { Pages = result.Items.ToList(), Total = result.Total, PartialViewName = "PageList", Pager = pager, Page = page, DisplayName = displayName, CurrentPage = pageContext.Page, CommentCounts = commentCounts };
        }
    }
}
