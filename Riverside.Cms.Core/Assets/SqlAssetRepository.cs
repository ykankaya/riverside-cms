using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Assets;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Assets
{
    public class SqlAssetRepository : IAssetRepository
    {
        private IDatabaseManagerFactory _databaseManagerFactory;
        private ISqlManager _sqlManager;

        public SqlAssetRepository(IDatabaseManagerFactory databaseManagerFactory, ISqlManager sqlManager)
        {
            _databaseManagerFactory = databaseManagerFactory;
            _sqlManager = sqlManager;
        }

        public void RegisterDeployment(long tenantId, string hostname, DateTime deployed, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.RegisterAssetDeployment.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@Hostname", FieldType.NVarChar, 100, hostname);
                dbm.AddParameter("@Deployed", FieldType.DateTime, deployed);
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public AssetDeployment ReadDeployment(long tenantId, string hostname, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                AssetDeployment deployment = null;
                dbm.SetSQL(_sqlManager.GetSql("Sql.ReadAssetDeployment.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@Hostname", FieldType.NVarChar, 100, hostname);
                dbm.ExecuteReader();
                if (dbm.Read())
                {
                    deployment = new AssetDeployment
                    {
                        TenantId = (long)dbm.DataReaderValue("TenantId"),
                        Hostname = (string)dbm.DataReaderValue("Hostname"),
                        Deployed = (DateTime)dbm.DataReaderValue("Deployed")
                    };
                }
                return deployment;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public IEnumerable<Guid> ListAssetElementTypes(long tenantId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                List<Guid> guids = new List<Guid>();
                dbm.SetSQL(_sqlManager.GetSql("Sql.ListAssetElementTypes.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.ExecuteReader();
                while (dbm.Read())
                    guids.Add((Guid)dbm.DataReaderValue("ElementTypeId"));
                return guids;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }
    }
}
