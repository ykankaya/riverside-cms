using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;
using Riverside.Utilities.Security;

namespace Riverside.Cms.Core.Authentication
{
    public interface IUserRepository
    {
        User ReadUser(long tenantId, long userId, IUnitOfWork unitOfWork = null);
        User ReadUserByAlias(long tenantId, string alias, IUnitOfWork unitOfWork = null);
        User ReadUserByEmail(long tenantId, string email, IUnitOfWork unitOfWork = null);
        User ReadUserByConfirmToken(long tenantId, Token confirmToken, IUnitOfWork unitOfWork = null);
        User ReadUserByResetPasswordToken(long tenantId, Token resetPasswordToken, IUnitOfWork unitOfWork = null);
        long CreateUser(long tenantId, string email, string alias, List<string> roles, Token confirmToken, IUnitOfWork unitOfWork = null);
        void UpdateUser(User user, IUnitOfWork unitOfWork = null);
    }
}
