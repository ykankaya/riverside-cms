using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Authentication
{
    /// <summary>
    /// Authentication providers (for example forms authentication), should implement this provider service.
    /// </summary>
    public interface IAuthenticationProviderService
    {
        /// <summary>
        /// Log user on using underlying authentication provider.
        /// </summary>
        /// <param name="user">Contains information about user that is being logged on.</param>
        void LogonAuthenticatedUser(AuthenticatedUserInfo authenticatedUserInfo);

        /// <summary>
        /// Gets logged on user or returns null if no user logged on.
        /// </summary>
        /// <returns>Authenticated user details.</returns>
        AuthenticatedUserInfo GetCurrentUser();

        /// <summary>
        /// Logs off the currently authenticated user.
        /// </summary>
        void Logoff();
    }
}
