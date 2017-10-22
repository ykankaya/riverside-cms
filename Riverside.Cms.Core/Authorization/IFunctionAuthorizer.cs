using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Authorization
{
    /// <summary>
    /// Implement this interface to check whether a user can access a given function. If not, an authorization failure exception is thrown.
    /// </summary>
    public interface IFunctionAuthorizer
    {
        /// <summary>
        /// Checks that a user has the right to access a given function. If user does not have permission, then an exception is thrown.
        /// </summary>
        /// <param name="userFunction">Contains user and function whose access is tested.</param>
        void Authorize(UserFunction userFunction);
    }
}
