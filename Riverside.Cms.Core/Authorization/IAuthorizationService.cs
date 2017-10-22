using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Authorization
{
    public interface IAuthorizationService
    {
        bool UserInFunction(string function);
        IEnumerable<string> ListFunctionsForRoles(IEnumerable<string> roles, IUnitOfWork unitOfWork = null);
        void AuthorizeUserForFunction(string function);
    }
}
