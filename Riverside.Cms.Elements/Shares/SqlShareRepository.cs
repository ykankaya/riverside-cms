using Riverside.Utilities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Shares
{
    public class SqlShareRepository : IShareRepository
    {
        private IDatabaseManagerFactory _databaseManagerFactory;
        private ISqlManager _sqlManager;

        public SqlShareRepository(IDatabaseManagerFactory databaseManagerFactory, ISqlManager sqlManager)
        {
            _databaseManagerFactory = databaseManagerFactory;
            _sqlManager = sqlManager;
        }

        public void Create(ShareSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.CreateShare.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@DisplayName", FieldType.NVarChar, 256, settings.DisplayName ?? (object)DBNull.Value);
                dbm.AddParameter("@ShareOnDigg", FieldType.Bit, settings.ShareOnDigg);
                dbm.AddParameter("@ShareOnFacebook", FieldType.Bit, settings.ShareOnFacebook);
                dbm.AddParameter("@ShareOnGoogle", FieldType.Bit, settings.ShareOnGoogle);
                dbm.AddParameter("@ShareOnLinkedIn", FieldType.Bit, settings.ShareOnLinkedIn);
                dbm.AddParameter("@ShareOnPinterest", FieldType.Bit, settings.ShareOnPinterest);
                dbm.AddParameter("@ShareOnReddit", FieldType.Bit, settings.ShareOnReddit);
                dbm.AddParameter("@ShareOnStumbleUpon", FieldType.Bit, settings.ShareOnStumbleUpon);
                dbm.AddParameter("@ShareOnTumblr", FieldType.Bit, settings.ShareOnTumblr);
                dbm.AddParameter("@ShareOnTwitter", FieldType.Bit, settings.ShareOnTwitter);
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
                string sql = _sqlManager.GetSql("Sql.CopyShare.sql");
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

        public void Read(ShareSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.ReadShare.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.ExecuteReader();
                dbm.Read();
                settings.DisplayName = dbm.DataReaderValue("DisplayName") == DBNull.Value ? null : (string)dbm.DataReaderValue("DisplayName");
                settings.ShareOnDigg = (bool)dbm.DataReaderValue("ShareOnDigg");
                settings.ShareOnFacebook = (bool)dbm.DataReaderValue("ShareOnFacebook");
                settings.ShareOnGoogle = (bool)dbm.DataReaderValue("ShareOnGoogle");
                settings.ShareOnLinkedIn = (bool)dbm.DataReaderValue("ShareOnLinkedIn");
                settings.ShareOnPinterest = (bool)dbm.DataReaderValue("ShareOnPinterest");
                settings.ShareOnReddit = (bool)dbm.DataReaderValue("ShareOnReddit");
                settings.ShareOnStumbleUpon = (bool)dbm.DataReaderValue("ShareOnStumbleUpon");
                settings.ShareOnTumblr = (bool)dbm.DataReaderValue("ShareOnTumblr");
                settings.ShareOnTwitter = (bool)dbm.DataReaderValue("ShareOnTwitter");
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public void Update(ShareSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.UpdateShare.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@DisplayName", FieldType.NVarChar, 256, settings.DisplayName ?? (object)DBNull.Value);
                dbm.AddParameter("@ShareOnDigg", FieldType.Bit, settings.ShareOnDigg);
                dbm.AddParameter("@ShareOnFacebook", FieldType.Bit, settings.ShareOnFacebook);
                dbm.AddParameter("@ShareOnGoogle", FieldType.Bit, settings.ShareOnGoogle);
                dbm.AddParameter("@ShareOnLinkedIn", FieldType.Bit, settings.ShareOnLinkedIn);
                dbm.AddParameter("@ShareOnPinterest", FieldType.Bit, settings.ShareOnPinterest);
                dbm.AddParameter("@ShareOnReddit", FieldType.Bit, settings.ShareOnReddit);
                dbm.AddParameter("@ShareOnStumbleUpon", FieldType.Bit, settings.ShareOnStumbleUpon);
                dbm.AddParameter("@ShareOnTumblr", FieldType.Bit, settings.ShareOnTumblr);
                dbm.AddParameter("@ShareOnTwitter", FieldType.Bit, settings.ShareOnTwitter);
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
                string sql = _sqlManager.GetSql("Sql.DeleteShare.sql");
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
