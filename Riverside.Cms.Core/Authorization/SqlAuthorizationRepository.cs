using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authorization;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Authorization
{
    public class SqlAuthorizationRepository : IAuthorizationRepository
    {
        private IDatabaseManagerFactory _databaseManagerFactory;
        private ISqlManager _sqlManager;

        public SqlAuthorizationRepository(IDatabaseManagerFactory databaseManagerFactory, ISqlManager sqlManager)
        {
            _databaseManagerFactory = databaseManagerFactory;
            _sqlManager = sqlManager;
        }

        private Role GetRoleFromDatabaseManager(IDatabaseManager dbm)
        {
            Role role = new Role {
                RoleId = (long)dbm.DataReaderValue("RoleId"),
                Name = (string)dbm.DataReaderValue("Name"),
                Functions = new List<Function>()
            };
            return role;
        }

        private Function GetFunctionFromDatabaseManager(IDatabaseManager dbm)
        {
            Function function = new Function {
                FunctionId = (long)dbm.DataReaderValue("FunctionId"),
                Name = (string)dbm.DataReaderValue("Name")
            };
            return function;
        }

        public IEnumerable<Role> ListRoles(IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                // The list that will be returned
                List<Role> roles = new List<Role>();
                Dictionary<long, Role> rolesById = new Dictionary<long, Role>();

                // Get roles
                dbm.SetSQL(_sqlManager.GetSql("Sql.ListRoles.sql"));
                dbm.ExecuteReader();
                while (dbm.Read())
                {
                    Role role = GetRoleFromDatabaseManager(dbm);
                    roles.Add(role);
                    rolesById.Add(role.RoleId, role);
                }

                // Get functions
                while (dbm.Read())
                {
                    long roleId = (long)dbm.DataReaderValue("RoleId");
                    Role role = rolesById[roleId];
                    Function function = GetFunctionFromDatabaseManager(dbm);
                    role.Functions.Add(function);
                }

                // Return list of roles
                return roles;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }
    }
}
