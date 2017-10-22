using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.Footers
{
    public class SqlFooterRepository : IFooterRepository
    {
        private IDatabaseManagerFactory _databaseManagerFactory;
        private ISqlManager _sqlManager;

        public SqlFooterRepository(IDatabaseManagerFactory databaseManagerFactory, ISqlManager sqlManager)
        {
            _databaseManagerFactory = databaseManagerFactory;
            _sqlManager = sqlManager;
        }

        public void Create(FooterSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.CreateFooter.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@Message", FieldType.NVarChar, -1, settings.Message ?? (object)DBNull.Value);
                dbm.AddParameter("@ShowLoggedOnUserOptions", FieldType.Bit, settings.ShowLoggedOnUserOptions);
                dbm.AddParameter("@ShowLoggedOffUserOptions", FieldType.Bit, settings.ShowLoggedOffUserOptions);
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.CopyFooter.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@SourceTenantId", FieldType.BigInt, sourceTenantId);
                dbm.AddParameter("@SourceElementId", FieldType.BigInt, sourceElementId);
                dbm.AddParameter("@DestTenantId", FieldType.BigInt, destTenantId);
                dbm.AddParameter("@DestElementId", FieldType.BigInt, destElementId);
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public void Read(FooterSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.ReadFooter.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.ExecuteReader();
                dbm.Read();
                settings.Message = dbm.DataReaderValue("Message") == DBNull.Value ? null : (string)dbm.DataReaderValue("Message");
                settings.ShowLoggedOnUserOptions = (bool)dbm.DataReaderValue("ShowLoggedOnUserOptions");
                settings.ShowLoggedOffUserOptions = (bool)dbm.DataReaderValue("ShowLoggedOffUserOptions");
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public void Update(FooterSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.UpdateFooter.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@Message", FieldType.NVarChar, -1, settings.Message ?? (object)DBNull.Value);
                dbm.AddParameter("@ShowLoggedOnUserOptions", FieldType.Bit, settings.ShowLoggedOnUserOptions);
                dbm.AddParameter("@ShowLoggedOffUserOptions", FieldType.Bit, settings.ShowLoggedOffUserOptions);
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.DeleteFooter.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, elementId);
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }
    }
}
