using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Riverside.Cms.Core.Authentication
{
    public class CookieAuthenticationProviderService : IAuthenticationProviderService
    {
        private IHttpContextAccessor _httpContextAccessor;

        public CookieAuthenticationProviderService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public AuthenticatedUserInfo GetCurrentUser()
        {
            if (_httpContextAccessor.HttpContext.User == null || !_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                return null;

            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User;

            AuthenticatedUser user = new AuthenticatedUser();
            user.Alias = principal.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).FirstOrDefault();
            user.Email = principal.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault();
            user.TenantId = Convert.ToInt64(principal.Claims.Where(c => c.Type == "TenantId").Select(c => c.Value).FirstOrDefault());
            user.UserId = Convert.ToInt64(principal.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).FirstOrDefault());
            user.Roles = new List<string>();
            foreach (Claim claim in principal.Claims.Where(c => c.Type == ClaimTypes.Role))
                user.Roles.Add(claim.Value);
            bool rememberMe = Convert.ToBoolean(principal.Claims.Where(c => c.Type == "RememberMe").Select(c => c.Value).FirstOrDefault());

            AuthenticatedUserInfo userInfo = new AuthenticatedUserInfo
            {
                User = user,
                RememberMe = rememberMe
            };

            return userInfo;
        }

        public void Logoff()
        {
            _httpContextAccessor.HttpContext.SignOutAsync().Wait();
        }

        public void LogonAuthenticatedUser(AuthenticatedUserInfo authenticatedUserInfo)
        {
            AuthenticationProperties properties = null;

            if (authenticatedUserInfo.RememberMe)
                properties = new AuthenticationProperties { IsPersistent = true };
            else
                properties = new AuthenticationProperties { ExpiresUtc = DateTime.UtcNow.AddMinutes(30), IsPersistent = true };

            ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, authenticatedUserInfo.User.Alias));
            identity.AddClaim(new Claim(ClaimTypes.Email, authenticatedUserInfo.User.Email));
            identity.AddClaim(new Claim("TenantId", authenticatedUserInfo.User.TenantId.ToString()));
            identity.AddClaim(new Claim("UserId", authenticatedUserInfo.User.UserId.ToString()));
            identity.AddClaim(new Claim("RememberMe", authenticatedUserInfo.RememberMe.ToString()));
            identity.AddClaim(new Claim("Version", "1"));
            foreach (string role in authenticatedUserInfo.User.Roles)
                identity.AddClaim(new Claim(ClaimTypes.Role, role));

            _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties).Wait();
        }
    }
}
