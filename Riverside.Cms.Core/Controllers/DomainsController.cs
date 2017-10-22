using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Domains;
using Riverside.Utilities.Data;
using Riverside.Utilities.Validation;
using Riverside.UI.Controls;
using Riverside.UI.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Riverside.Cms.Core.Controllers
{
    /// <summary>
    /// Controller for domain administration.
    /// </summary>
    public class DomainsController : Controller
    {
        // Member variables
        private IAuthenticationService _authenticationService;
        private IValidationService _validationService;
        private IDomainPortalService _domainPortalService;
        private IDomainService _domainService;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="authenticationService">Authentication service.</param>
        /// <param name="domainPortalService">Portal related domain administration.</param>
        /// <param name="domainService">Core domain administration.</param>
        /// <param name="validationService">Registers errors in model state.</param>
        public DomainsController(IAuthenticationService authenticationService, IDomainPortalService domainPortalService, IDomainService domainService, IValidationService validationService)
        {
            _authenticationService = authenticationService;
            _domainPortalService = domainPortalService;
            _domainService = domainService;
            _validationService = validationService;
        }

        /// <summary>
        /// Displays searchable list of domains for current website.
        /// </summary>
        /// <param name="page">The grid page index.</param>
        /// <param name="grid">Grid parameters.</param>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult Search(int? page, Grid<Domain> grid)
        {
            return View(_domainPortalService.Search(_authenticationService.TenantId, page, grid.Search, ModelState.IsValid));
        }

        /// <summary>
        /// Displays searchable list of domains for current website. Called when grid search form posted back.
        /// </summary>
        /// <param name="page">The grid page index.</param>
        /// <param name="grid">Grid parameters.</param>
        /// <param name="formCollection">Used to differentiate this function from the HttpGet version.</param>
        /// <returns>An ActionResult.</returns>
        [HttpPost]
        public IActionResult Search(int? page, Grid<Domain> grid, FormCollection formCollection)
        {
            if (ModelState.IsValid)
                return RedirectToAction("search", new { search = grid.Search });
            return Search(page, grid);
        }

        /// <summary>
        /// Displays create domain page.
        /// </summary>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View(_domainPortalService.Create());
        }

        /// <summary>
        /// Create domain form postback.
        /// </summary>
        /// <param name="domain">Contains new domain details.</param>
        /// <returns>An ActionResult.</returns>
        [HttpPost]
        public IActionResult Create(Domain domain)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    domain.TenantId = _authenticationService.TenantId;
                    _domainService.Create(domain);
                    return RedirectToAction("search");
                }
                catch (ValidationErrorException ex)
                {
                    _validationService.RegisterValidationErrors(ModelState, ex.Errors, "Domain");
                }
            }
            return Create();
        }

        /// <summary>
        /// Displays domain details.
        /// </summary>
        /// <param name="domainId">Identifies domain whose details are displayed.</param>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult Read(long domainId)
        {
            return View(_domainPortalService.ReadUpdateDelete(_authenticationService.TenantId, domainId, DataAction.Read));
        }

        /// <summary>
        /// Displays domain update page.
        /// </summary>
        /// <param name="domainId">Identifies domain that is being updated.</param>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult Update(long domainId)
        {
            return View(_domainPortalService.ReadUpdateDelete(_authenticationService.TenantId, domainId, DataAction.Update));
        }

        /// <summary>
        /// Update domain form postback.
        /// </summary>
        /// <param name="domainId">Identifies domain that is being updated.</param>
        /// <param name="domain">Contains new domain details.</param>
        /// <returns>An ActionResult.</returns>
        [HttpPost]
        public IActionResult Update(long domainId, Domain domain)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    domain.TenantId = _authenticationService.TenantId;
                    domain.DomainId = domainId;
                    _domainService.Update(domain);
                    return RedirectToAction("search");
                }
                catch (ValidationErrorException ex)
                {
                    _validationService.RegisterValidationErrors(ModelState, ex.Errors, "Domain");
                }
            }
            return Update(domainId);
        }

        /// <summary>
        /// Display delete domain form.
        /// </summary>
        /// <param name="domainId">Identifies domain to be deleted.</param>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult Delete(long domainId)
        {
            return View(_domainPortalService.ReadUpdateDelete(_authenticationService.TenantId, domainId, DataAction.Delete));
        }

        /// <summary>
        /// Deletes domain.
        /// </summary>
        /// <param name="domainId">Identifies domain to be deleted.</param>
        /// <param name="formCollection">Used to differentiate this function from the HttpGet version.</param>
        /// <returns>An ActionResult.</returns>
        [HttpPost]
        public IActionResult Delete(long domainId, FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _domainService.Delete(_authenticationService.TenantId, domainId);
                    return RedirectToAction("search");
                }
                catch (ValidationErrorException ex)
                {
                    _validationService.RegisterValidationErrors(ModelState, ex.Errors, "Domain");
                }
            }
            return Delete(domainId);
        }
    }
}
