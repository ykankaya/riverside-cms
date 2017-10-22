using Riverside.Utilities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.TagCloud
{
    public class SqlTagCloudRepository : ITagCloudRepository
    {
        private IDatabaseManagerFactory _databaseManagerFactory;
        private ISqlManager _sqlManager;

        public SqlTagCloudRepository(IDatabaseManagerFactory databaseManagerFactory, ISqlManager sqlManager)
        {
            _databaseManagerFactory = databaseManagerFactory;
            _sqlManager = sqlManager;
        }

        public void Create(TagCloudSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.CreateTagCloud.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@PageTenantId", FieldType.BigInt, settings.PageTenantId ?? (object)DBNull.Value);
                dbm.AddParameter("@PageId", FieldType.BigInt, settings.PageId ?? (object)DBNull.Value);
                dbm.AddParameter("@DisplayName", FieldType.NVarChar, 256, settings.DisplayName ?? (object)DBNull.Value);
                dbm.AddParameter("@Recursive", FieldType.Bit, settings.Recursive);
                dbm.AddParameter("@NoTagsMessage", FieldType.NVarChar, -1, settings.NoTagsMessage);
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
                string sql = _sqlManager.GetSql("Sql.CopyTagCloud.sql");
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

        public void Read(TagCloudSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.ReadTagCloud.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.ExecuteReader();
                dbm.Read();
                settings.PageTenantId = dbm.DataReaderValue("PageTenantId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("PageTenantId");
                settings.PageId = dbm.DataReaderValue("PageId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("PageId");
                settings.DisplayName = dbm.DataReaderValue("DisplayName") == DBNull.Value ? null : (string)dbm.DataReaderValue("DisplayName");
                settings.Recursive = (bool)dbm.DataReaderValue("Recursive");
                settings.NoTagsMessage = (string)dbm.DataReaderValue("NoTagsMessage");
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public void Update(TagCloudSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.UpdateTagCloud.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@PageTenantId", FieldType.BigInt, settings.PageTenantId ?? (object)DBNull.Value);
                dbm.AddParameter("@PageId", FieldType.BigInt, settings.PageId ?? (object)DBNull.Value);
                dbm.AddParameter("@DisplayName", FieldType.NVarChar, 256, settings.DisplayName ?? (object)DBNull.Value);
                dbm.AddParameter("@Recursive", FieldType.Bit, settings.Recursive);
                dbm.AddParameter("@NoTagsMessage", FieldType.NVarChar, -1, settings.NoTagsMessage);
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
                string sql = _sqlManager.GetSql("Sql.DeleteTagCloud.sql");
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
