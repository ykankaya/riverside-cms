using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Utilities.Authorization;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private static Dictionary<string, Role> _rolesByName;

        private IAuthenticationService _authenticationService;
        private IAuthorizationRepository _authorizationRepository;

        public AuthorizationService(IAuthenticationService authenticationService, IAuthorizationRepository authorizationRepository)
        {
            _authenticationService = authenticationService;
            _authorizationRepository = authorizationRepository;
        }

        public IEnumerable<string> ListFunctionsForRoles(IEnumerable<string> roles, IUnitOfWork unitOfWork = null)
        {
            // Get local copy of reference to roles dictionary
            Dictionary<string, Role> rolesByName = _rolesByName;

            // If reference does not exist, create the dictionary 
            if (rolesByName == null)
            {
                IEnumerable<Role> rolesObjects = _authorizationRepository.ListRoles(unitOfWork);
                rolesByName = rolesObjects.GroupBy(r => r.Name).ToDictionary(g => g.Key, g => g.First());
                _rolesByName = rolesByName;
            }

            // Get long list of all functions in roles
            List<string> functions = new List<string>();
            foreach (string role in roles)
            {
                Role roleObject = rolesByName[role];
                functions.AddRange(roleObject.Functions.Select(f => f.Name));
            }

            // Return list of functions with no duplicates
            return functions.Distinct();
        }

        public bool UserInFunction(string function)
        {
            AuthenticatedUserInfo userInfo = _authenticationService.GetCurrentUser();
            if (userInfo == null)
                return false;
            IEnumerable<string> functions = ListFunctionsForRoles(userInfo.User.Roles);
            return functions.Where(f => f == function).Count() > 0;
        }

        public void AuthorizeUserForFunction(string function)
        {
            if (!UserInFunction(function))
                throw new AuthorizationException();
        }
    }
}
