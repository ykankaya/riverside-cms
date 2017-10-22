using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Webs;
using Riverside.Utilities.Data;
using Riverside.Utilities.Validation;
using Riverside.UI.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Riverside.Cms.Core.Controllers
{
    /// <summary>
    /// Controller for website administration.
    /// </summary>
    public class WebsController : Controller
    {
        // Member variables
        private IValidationService _validationService;
        private IWebPortalService _webPortalService;
        private IWebService _webService;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="validationService">Registers errors in model state.</param>
        /// <param name="webPortalService">Portal related functionality.</param>
        /// <param name="webService">Provides access to web management functionality.</param>
        public WebsController(IValidationService validationService, IWebPortalService webPortalService, IWebService webService)
        {
            _validationService = validationService;
            _webPortalService = webPortalService;
            _webService = webService;
        }

        /// <summary>
        /// Displays searchable list of websites.
        /// </summary>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult Search()
        {
            ViewBag.AngularJsController = "SearchWebsController";
            return View();
        }

        /// <summary>
        /// Displays create website page.
        /// </summary>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.AngularJsController = "CreateWebController";
            return View();
        }

        /// <summary>
        /// Displays website details.
        /// </summary>
        /// <param name="tenantId">Identifies website whose details are displayed.</param>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult Read(long tenantId)
        {
            return View(_webPortalService.ReadUpdateDelete(tenantId, DataAction.Read));
        }

        /// <summary>
        /// Displays website update page.
        /// </summary>
        /// <param name="tenantId">Identifies website that is being updated.</param>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult Update(long tenantId)
        {
            return View(_webPortalService.ReadUpdateDelete(tenantId, DataAction.Update));
        }

        /// <summary>
        /// Update website form postback.
        /// </summary>
        /// <param name="tenantId">Identifies website that is being updated.</param>
        /// <param name="web">Contains new website details.</param>
        /// <returns>An ActionResult.</returns>
        [HttpPost]
        public IActionResult Update(long tenantId, Web web)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    web.TenantId = tenantId;
                    _webService.Update(web);
                    return RedirectToAction("search");
                }
                catch (ValidationErrorException ex)
                {
                    _validationService.RegisterValidationErrors(ModelState, ex.Errors, "Web");
                }
            }
            return Update(tenantId);
        }

        /// <summary>
        /// Display delete website form.
        /// </summary>
        /// <param name="tenantId">Identifies website to be deleted.</param>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult Delete(long tenantId)
        {
            return View(_webPortalService.ReadUpdateDelete(tenantId, DataAction.Delete));
        }

        /// <summary>
        /// Deletes website.
        /// </summary>
        /// <param name="tenantId">Identifies website to be deleted.</param>
        /// <param name="formCollection">Used to differentiate this function from the HttpGet version.</param>
        /// <returns>An ActionResult.</returns>
        [HttpPost]
        public IActionResult Delete(long tenantId, FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _webService.Delete(tenantId);
                    return RedirectToAction("search");
                }
                catch (ValidationErrorException ex)
                {
                    _validationService.RegisterValidationErrors(ModelState, ex.Errors, "Web");
                }
            }
            return Delete(tenantId);
        }
    }
}
