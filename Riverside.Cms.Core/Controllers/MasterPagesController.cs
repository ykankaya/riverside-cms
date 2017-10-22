using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Riverside.Cms.Core.Controllers
{
    /// <summary>
    /// Controller for master page administration.
    /// </summary>
    public class MasterPagesController : Controller
    {
        /// <summary>
        /// Displays searchable list of master pages.
        /// </summary>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult Search()
        {
            ViewBag.AngularJsController = "SearchMasterPagesController";
            return View();
        }

        /// <summary>
        /// Displays master page details.
        /// </summary>
        /// <param name="masterPageId">Identifies master page whose details are displayed.</param>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult Read(long masterPageId)
        {
            ViewBag.AngularJsController = "ReadMasterPageController";
            return View();
        }
    }
}
