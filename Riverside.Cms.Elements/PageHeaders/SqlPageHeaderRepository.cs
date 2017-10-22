using Riverside.Utilities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.PageHeaders
{
    public class SqlPageHeaderRepository : IPageHeaderRepository
    {
        private IDatabaseManagerFactory _databaseManagerFactory;
        private ISqlManager _sqlManager;

        public SqlPageHeaderRepository(IDatabaseManagerFactory databaseManagerFactory, ISqlManager sqlManager)
        {
            _databaseManagerFactory = databaseManagerFactory;
            _sqlManager = sqlManager;
        }

        public void Create(PageHeaderSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.CreatePageHeader.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@PageTenantId", FieldType.BigInt, settings.PageTenantId ?? (object)DBNull.Value);
                dbm.AddParameter("@PageId", FieldType.BigInt, settings.PageId ?? (object)DBNull.Value);
                dbm.AddParameter("@ShowName", FieldType.Bit, settings.ShowName);
                dbm.AddParameter("@ShowDescription", FieldType.Bit, settings.ShowDescription);
                dbm.AddParameter("@ShowImage", FieldType.Bit, settings.ShowImage);
                dbm.AddParameter("@ShowCreated", FieldType.Bit, settings.ShowCreated);
                dbm.AddParameter("@ShowUpdated", FieldType.Bit, settings.ShowUpdated);
                dbm.AddParameter("@ShowOccurred", FieldType.Bit, settings.ShowOccurred);
                dbm.AddParameter("@ShowBreadcrumbs", FieldType.Bit, settings.ShowBreadcrumbs);
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
                string sql = _sqlManager.GetSql("Sql.CopyPageHeader.sql");
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

        public void Read(PageHeaderSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.ReadPageHeader.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.ExecuteReader();
                dbm.Read();
                settings.PageTenantId = dbm.DataReaderValue("PageTenantId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("PageTenantId");
                settings.PageId = dbm.DataReaderValue("PageId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("PageId");
                settings.ShowName = (bool)dbm.DataReaderValue("ShowName");
                settings.ShowDescription = (bool)dbm.DataReaderValue("ShowDescription");
                settings.ShowImage = (bool)dbm.DataReaderValue("ShowImage");
                settings.ShowCreated = (bool)dbm.DataReaderValue("ShowCreated");
                settings.ShowUpdated = (bool)dbm.DataReaderValue("ShowUpdated");
                settings.ShowOccurred = (bool)dbm.DataReaderValue("ShowOccurred");
                settings.ShowBreadcrumbs = (bool)dbm.DataReaderValue("ShowBreadcrumbs");
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public void Update(PageHeaderSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.UpdatePageHeader.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@PageTenantId", FieldType.BigInt, settings.PageTenantId ?? (object)DBNull.Value);
                dbm.AddParameter("@PageId", FieldType.BigInt, settings.PageId ?? (object)DBNull.Value);
                dbm.AddParameter("@ShowName", FieldType.Bit, settings.ShowName);
                dbm.AddParameter("@ShowDescription", FieldType.Bit, settings.ShowDescription);
                dbm.AddParameter("@ShowImage", FieldType.Bit, settings.ShowImage);
                dbm.AddParameter("@ShowCreated", FieldType.Bit, settings.ShowCreated);
                dbm.AddParameter("@ShowUpdated", FieldType.Bit, settings.ShowUpdated);
                dbm.AddParameter("@ShowOccurred", FieldType.Bit, settings.ShowOccurred);
                dbm.AddParameter("@ShowBreadcrumbs", FieldType.Bit, settings.ShowBreadcrumbs);
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
                string sql = _sqlManager.GetSql("Sql.DeletePageHeader.sql");
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
