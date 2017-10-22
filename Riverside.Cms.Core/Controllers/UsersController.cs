using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Uploads;
using Riverside.Cms.Core.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Riverside.Cms.Core.Controllers
{
    /// <summary>
    /// Controller for user functionality.
    /// </summary>
    public class UsersController : Controller
    {
        // Member variables
        private IAuthenticationService _authenticationService;
        private IPagePortalService _pagePortalService;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="authenticationService">Provides access to authentication related features.</param>
        /// <param name="pagePortalService">Portal related page functionality.</param>
        public UsersController(IAuthenticationService authenticationService, IPagePortalService pagePortalService)
        {
            _authenticationService = authenticationService;
            _pagePortalService = pagePortalService;
        }

        /// <summary>
        /// Displays the register user form.
        /// </summary>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View("~/Views/Pages/Read.cshtml", _pagePortalService.GetCreateUserPageViewModel(_authenticationService.TenantId));
        }

        /// <summary>
        /// Displays activate user form with set password functionality.
        /// </summary>
        /// <param name="activationKey">Activation key used to identify and validate account being activated.</param>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult ConfirmSetPassword()
        {
            return View("~/Views/Pages/Read.cshtml", _pagePortalService.GetConfirmUserSetPasswordPageViewModel(_authenticationService.TenantId));
        }

        /// <summary>
        /// Displays activate user form.
        /// </summary>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult Confirm()
        {
            return View("~/Views/Pages/Read.cshtml", _pagePortalService.GetConfirmUserPageViewModel(_authenticationService.TenantId));
        }

        /// <summary>
        /// Displays logon form.
        /// </summary>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult Logon()
        {
            return View("~/Views/Pages/Read.cshtml", _pagePortalService.GetLogonUserPageViewModel(_authenticationService.TenantId));
        }

        /// <summary>
        /// Displays logoff form.
        /// </summary>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult Logoff()
        {
            // Log user off
            _authenticationService.Logoff();

            // Redirect to home page
            return Redirect(Url.Content("~/"));
        }

        /// <summary>
        /// Displays change password form.
        /// </summary>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View("~/Views/Pages/Read.cshtml", _pagePortalService.GetChangePasswordPageViewModel(_authenticationService.TenantId));
        }

        /// <summary>
        /// Display page for updating a user's profile.
        /// </summary>
        [HttpGet]
        [Authorize]
        public IActionResult UpdateUser()
        {
            return View("~/Views/Pages/Read.cshtml", _pagePortalService.GetUpdateUserPageViewModel(_authenticationService.TenantId));
        }

        /// <summary>
        /// Display page for initiating a password reset.
        /// </summary>
        [HttpGet]
        public IActionResult ForgottenPassword()
        {
            return View("~/Views/Pages/Read.cshtml", _pagePortalService.GetForgottenPasswordPageViewModel(_authenticationService.TenantId));
        }

        /// <summary>
        /// Display page for reset password.
        /// </summary>
        /// <param name="resetPasswordKey">Reset password key used to identify and validate account whose password is being reset.</param>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View("~/Views/Pages/Read.cshtml", _pagePortalService.GetResetPasswordPageViewModel(_authenticationService.TenantId));
        }

        /// <summary>
        /// Gets a user's image.
        /// </summary>
        /// <param name="userId">The user whose image is returned.</param>
        /// <param name="format">The format that is retrieve "thumbnail" or "preview".</param>
        /// <returns>An ActionResult.</returns>
        [HttpGet]
        [ResponseCache(Duration = 86400)]
        public IActionResult ReadImage(long userId, string format)
        {
            long tenantId = _authenticationService.TenantId;
            Image image = null;
            switch (format)
            {
                case "thumbnail":
                    image = _authenticationService.ReadThumbnailImage(tenantId, userId);
                    break;

                case "preview":
                    image = _authenticationService.ReadPreviewImage(tenantId, userId);
                    break;
            }
            if (image != null)
                return new FileContentResult(image.Content, image.ContentType) { FileDownloadName = image.Name };
            return null;
        }
    }
}
