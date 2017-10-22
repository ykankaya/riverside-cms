using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Authorization
{
    public interface IAuthorizationRepository
    {
        IEnumerable<Role> ListRoles(IUnitOfWork unitOfWork = null);
    }
}
