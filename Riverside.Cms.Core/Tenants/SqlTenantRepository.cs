using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Tenants;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Tenants
{
    /// <summary>
    /// SQL implementation of tenant repository.
    /// </summary>
    public class SqlTenantRepository : ITenantRepository
    {
        // Member variables
        private IDatabaseManagerFactory _databaseManagerFactory;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="databaseManagerFactory">Database manager factory.</param>
        public SqlTenantRepository(IDatabaseManagerFactory databaseManagerFactory)
        {
            _databaseManagerFactory = databaseManagerFactory;
        }

        /// <summary>
        /// Creates a new tenant.
        /// </summary>
        /// <param name="tenant">New tenant details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Newly allocated tenant identifier.</returns>
        public long Create(Tenant tenant, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetStoredProcedure("cms.CreateTenant");
                dbm.AddParameter("@Created", FieldType.DateTime, tenant.Created);
                dbm.AddParameter("@Updated", FieldType.DateTime, tenant.Updated);
                dbm.AddOutputParameter("@TenantId", FieldType.BigInt);
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                dbm.ExecuteNonQuery(outputValues);
                return (long)outputValues["@TenantId"];
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }
    }
}
