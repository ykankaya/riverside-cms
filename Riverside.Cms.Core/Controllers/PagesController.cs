using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Riverside.Cms.Core.Assets;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Domains;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Uploads;
using Riverside.Cms.Core.Webs;
using Riverside.UI.Web;

namespace Riverside.Cms.Core.Controllers
{
    /// <summary>
    /// Controller for page administration.
    /// </summary>
    public class PagesController : Controller
    {
        // Member variables
        private IAssetService _assetService;
        private IAuthenticationService _authenticationService;
        private IDomainService _domainService;
        private IPagePortalService _pagePortalService;
        private IPageService _pageService;
        private IUploadService _uploadService;
        private IWebHelperService _webHelperService;
        private IWebService _webService;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="assetService">Asset service.</param>
        /// <param name="authenticationService">Authentication service.</param>
        /// <param name="pagePortalService">Provides access to portal related page functionality.</param>
        /// <param name="pageService">Provides access to pages.</param>
        /// <param name="uploadService">Provides access to uploads.</param>
        /// <param name="webHelperService">Low level access to web components.</param>
        public PagesController(IAssetService assetService, IAuthenticationService authenticationService, IDomainService domainService, IPagePortalService pagePortalService, IPageService pageService, IUploadService uploadService, IWebHelperService webHelperService, IWebService webService)
        {
            _assetService = assetService;
            _authenticationService = authenticationService;
            _domainService = domainService;
            _pagePortalService = pagePortalService;
            _pageService = pageService;
            _uploadService = uploadService;
            _webHelperService = webHelperService;
            _webService = webService;
        }

        /// <summary>
        /// Displays searchable list of pages.
        /// </summary>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult Search()
        {
            ViewBag.AngularJsController = "SearchPagesController";
            return View();
        }

        /// <summary>
        /// Checks whether assets have been deployed and, if not, delpoys them.
        /// </summary>
        private void CheckAssetDeployment()
        {
            string hostname = _webHelperService.MachineName();
            AssetDeployment deployment = _assetService.ReadDeployment(_authenticationService.TenantId, hostname);
            if (deployment == null)
            {
                _assetService.Deploy(_authenticationService.TenantId);
                _assetService.RegisterDeployment(_authenticationService.TenantId, hostname);
            }
        }

        /// <summary>
        /// Displays a page.
        /// </summary>
        /// <param name="pageId">Identifies page whose details are displayed.</param>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult Read(long pageId)
        {
            CheckAssetDeployment();
            PageViewModel pageViewModel = _pagePortalService.GetReadPagePageViewModel(_authenticationService.TenantId, pageId);
            ViewBag.PageContext = pageViewModel.PageContext;
            return View("Read", pageViewModel);
        }

        /// <summary>
        /// Displays homepage.
        /// </summary>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult Home()
        {
            CheckAssetDeployment();
            PageViewModel pageViewModel = _pagePortalService.GetHomePagePageViewModel(_authenticationService.TenantId);
            ViewBag.PageContext = pageViewModel.PageContext;
            return View("Read", pageViewModel);
        }

        /// <summary>
        /// Displays administration page for updating fonts and colurs.
        /// </summary>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult Theme()
        {
            CheckAssetDeployment();
            PageViewModel pageViewModel = _pagePortalService.GetUpdateThemePageViewModel(_authenticationService.TenantId);
            ViewBag.PageContext = pageViewModel.PageContext;
            return View("Read", pageViewModel);
        }

        /// <summary>
        /// Displays administration page for creating a new page.
        /// </summary>
        /// <param name="masterPageId">Identifies master page that newly created page will be based upon.</param>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult Create(long masterPageId)
        {
            CheckAssetDeployment();
            PageViewModel pageViewModel = _pagePortalService.GetCreatePagePageViewModel(_authenticationService.TenantId, masterPageId);
            ViewBag.PageContext = pageViewModel.PageContext;
            return View("Read", pageViewModel);
        }

        /// <summary>
        /// Displays administration page for updating a new page.
        /// </summary>
        /// <param name="pageId">Identifies page that is being updated.</param>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult Update(long pageId)
        {
            CheckAssetDeployment();
            PageViewModel pageViewModel = _pagePortalService.GetUpdatePagePageViewModel(_authenticationService.TenantId, pageId);
            ViewBag.PageContext = pageViewModel.PageContext;
            return View("Read", pageViewModel);
        }

        /// <summary>
        /// Display create master page page.
        /// </summary>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult CreateMasterPage()
        {
            CheckAssetDeployment();
            PageViewModel pageViewModel = _pagePortalService.GetCreateMasterPagePageViewModel(_authenticationService.TenantId);
            ViewBag.PageContext = pageViewModel.PageContext;
            return View("Read", pageViewModel);
        }

        /// <summary>
        /// Display update master page page.
        /// </summary>
        /// <param name="masterPageId">The master page that is being updated.</param>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult UpdateMasterPage(long masterPageId)
        {
            CheckAssetDeployment();
            PageViewModel pageViewModel = _pagePortalService.GetUpdateMasterPagePageViewModel(_authenticationService.TenantId, masterPageId);
            ViewBag.PageContext = pageViewModel.PageContext;
            return View("Read", pageViewModel);
        }

        /// <summary>
        /// Display form for updating a specific master page zone.
        /// </summary>
        /// <param name="masterPageId">The master page whose zone is being updated.</param>
        /// <param name="masterPageZoneId">Identifies the master page zone being updated.</param>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult UpdateMasterPageZone(long masterPageId, long masterPageZoneId)
        {
            CheckAssetDeployment();
            PageViewModel pageViewModel = _pagePortalService.GetUpdateMasterPageZonePageViewModel(_authenticationService.TenantId, masterPageId, masterPageZoneId);
            ViewBag.PageContext = pageViewModel.PageContext;
            return View("Read", pageViewModel);
        }

        /// <summary>
        /// Display form for adding and removing master page zones.
        /// </summary>
        /// <param name="masterPageId">The master page whose zone are being updated.</param>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult UpdateMasterPageZones(long masterPageId)
        {
            CheckAssetDeployment();
            PageViewModel pageViewModel = _pagePortalService.GetUpdateMasterPageZonesPageViewModel(_authenticationService.TenantId, masterPageId);
            ViewBag.PageContext = pageViewModel.PageContext;
            return View("Read", pageViewModel);
        }

        /// <summary>
        /// Displays administration page for updating element that occurs on a page.
        /// </summary>
        /// <param name="pageId">The page that we were on when update element action was selected.</param>
        /// <param name="elementId">Identifies element that is being updated.</param>
        [HttpGet]
        public IActionResult UpdatePageElement(long pageId, long elementId)
        {
            CheckAssetDeployment();
            PageViewModel pageViewModel = _pagePortalService.GetUpdatePageElementPageViewModel(_authenticationService.TenantId, pageId, elementId);
            ViewBag.PageContext = pageViewModel.PageContext;
            return View("Read", pageViewModel);
        }

        /// <summary>
        /// Displays administration page for updating element that occurs on a master page.
        /// </summary>
        /// <param name="masterPageId">The master page that we were on when update element action was selected.</param>
        /// <param name="elementId">Identifies element that is being updated.</param>
        [HttpGet]
        public IActionResult UpdateMasterPageElement(long masterPageId, long elementId)
        {
            CheckAssetDeployment();
            PageViewModel pageViewModel = _pagePortalService.GetUpdateMasterPageElementPageViewModel(_authenticationService.TenantId, masterPageId, elementId);
            ViewBag.PageContext = pageViewModel.PageContext;
            return View("Read", pageViewModel);
        }

        /// <summary>
        /// Displays administration page for updating a page zone.
        /// </summary>
        /// <param name="pageId">The page that we were on when update page zone action was selected.</param>
        /// <param name="pageZoneId">Identifies page zone that is being updated.</param>
        [HttpGet]
        public IActionResult UpdatePageZone(long pageId, long pageZoneId)
        {
            CheckAssetDeployment();
            PageViewModel pageViewModel = _pagePortalService.GetUpdatePageZonePageViewModel(_authenticationService.TenantId, pageId, pageZoneId);
            ViewBag.PageContext = pageViewModel.PageContext;
            return View("Read", pageViewModel);
        }

        /// <summary>
        /// Gets a page's image.
        /// </summary>
        /// <param name="pageId">The page whose image is returned.</param>
        /// <param name="format">The format that is retrieve "thumbnail" or "preview".</param>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        [ResponseCache(Duration = 86400)]
        public IActionResult ReadImage(long pageId, string format)
        {
            long tenantId = _authenticationService.TenantId;
            Image image = null;
            switch (format)
            {
                case "thumbnail":
                    image = _pageService.ReadThumbnailImage(tenantId, pageId);
                    break;

                case "preview":
                    image = _pageService.ReadPreviewImage(tenantId, pageId);
                    break;
            }
            if (image != null)
                return new FileContentResult(image.Content, image.ContentType) { FileDownloadName = image.Name };
            return null;
        }
    }
}