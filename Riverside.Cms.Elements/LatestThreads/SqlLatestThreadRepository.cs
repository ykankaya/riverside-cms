using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.LatestThreads
{
    public class SqlLatestThreadRepository : ILatestThreadRepository
    {
        private IDatabaseManagerFactory _databaseManagerFactory;
        private ISqlManager _sqlManager;

        public SqlLatestThreadRepository(IDatabaseManagerFactory databaseManagerFactory, ISqlManager sqlManager)
        {
            _databaseManagerFactory = databaseManagerFactory;
            _sqlManager = sqlManager;
        }

        public void Create(LatestThreadSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.CreateLatestThread.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@PageTenantId", FieldType.BigInt, settings.PageTenantId ?? (object)DBNull.Value);
                dbm.AddParameter("@PageId", FieldType.BigInt, settings.PageId ?? (object)DBNull.Value);
                dbm.AddParameter("@DisplayName", FieldType.NVarChar, 256, settings.DisplayName ?? (object)DBNull.Value);
                dbm.AddParameter("@Recursive", FieldType.Bit, settings.Recursive);
                dbm.AddParameter("@NoThreadsMessage", FieldType.NVarChar, -1, settings.NoThreadsMessage ?? (object)DBNull.Value);
                dbm.AddParameter("@Preamble", FieldType.NVarChar, -1, settings.Preamble ?? (object)DBNull.Value);
                dbm.AddParameter("@PageSize", FieldType.Int, settings.PageSize);
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
                dbm.SetSQL(_sqlManager.GetSql("Sql.CopyLatestThread.sql"));
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

        public void Read(LatestThreadSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.ReadLatestThread.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.ExecuteReader();
                dbm.Read();
                settings.PageTenantId = dbm.DataReaderValue("PageTenantId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("PageTenantId");
                settings.PageId = dbm.DataReaderValue("PageId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("PageId");
                settings.DisplayName = dbm.DataReaderValue("DisplayName") == DBNull.Value ? null : (string)dbm.DataReaderValue("DisplayName");
                settings.Recursive = (bool)dbm.DataReaderValue("Recursive");
                settings.NoThreadsMessage = dbm.DataReaderValue("NoThreadsMessage") == DBNull.Value ? null : (string)dbm.DataReaderValue("NoThreadsMessage");
                settings.Preamble = dbm.DataReaderValue("Preamble") == DBNull.Value ? null : (string)dbm.DataReaderValue("Preamble");
                settings.PageSize = (int)dbm.DataReaderValue("PageSize");
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public void Update(LatestThreadSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.UpdateLatestThread.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@PageId", FieldType.BigInt, settings.PageId ?? (object)DBNull.Value);
                dbm.AddParameter("@DisplayName", FieldType.NVarChar, 256, settings.DisplayName ?? (object)DBNull.Value);
                dbm.AddParameter("@Recursive", FieldType.Bit, settings.Recursive);
                dbm.AddParameter("@NoThreadsMessage", FieldType.NVarChar, -1, settings.NoThreadsMessage ?? (object)DBNull.Value);
                dbm.AddParameter("@Preamble", FieldType.NVarChar, -1, settings.Preamble ?? (object)DBNull.Value);
                dbm.AddParameter("@PageSize", FieldType.Int, settings.PageSize);
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
                dbm.SetSQL(_sqlManager.GetSql("Sql.DeleteLatestThread.sql"));
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