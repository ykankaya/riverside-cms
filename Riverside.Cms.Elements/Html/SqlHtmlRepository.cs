using Riverside.Utilities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Html
{
    public class SqlHtmlRepository : IHtmlRepository
    {
        private IDatabaseManagerFactory _databaseManagerFactory;
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private ISqlManager _sqlManager;

        public SqlHtmlRepository(IDatabaseManagerFactory databaseManagerFactory, IUnitOfWorkFactory unitOfWorkFactory, ISqlManager sqlManager)
        {
            _databaseManagerFactory = databaseManagerFactory;
            _unitOfWorkFactory = unitOfWorkFactory;
            _sqlManager = sqlManager;
        }

        public void Create(HtmlSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.CreateHtml.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@Html", FieldType.NVarChar, -1, settings.Html);
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public long CreateUpload(HtmlUpload upload, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.CreateHtmlUpload.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, upload.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, upload.ElementId);
                dbm.AddParameter("@ImageTenantId", FieldType.BigInt, upload.ImageTenantId ?? (object)DBNull.Value);
                dbm.AddParameter("@ThumbnailImageUploadId", FieldType.BigInt, upload.ThumbnailImageUploadId ?? (object)DBNull.Value);
                dbm.AddParameter("@PreviewImageUploadId", FieldType.BigInt, upload.PreviewImageUploadId ?? (object)DBNull.Value);
                dbm.AddParameter("@ImageUploadId", FieldType.BigInt, upload.ImageUploadId ?? (object)DBNull.Value);
                dbm.AddParameter("@UploadTenantId", FieldType.BigInt, upload.UploadTenantId ?? (object)DBNull.Value);
                dbm.AddParameter("@UploadId", FieldType.BigInt, upload.UploadId ?? (object)DBNull.Value);
                dbm.AddOutputParameter("@HtmlUploadId", FieldType.BigInt);
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                dbm.ExecuteNonQuery(outputValues);
                return (long)outputValues["@HtmlUploadId"];
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
                string sql = _sqlManager.GetSql("Sql.CopyHtml.sql");
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

        public void Read(HtmlSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.ReadHtml.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.ExecuteReader();
                dbm.Read();
                settings.Html = (string)dbm.DataReaderValue("Html");
                settings.Uploads = new List<HtmlUpload>();
                dbm.Read();
                while (dbm.Read())
                    settings.Uploads.Add(GetHtmlUploadFromDatabaseManager(dbm));
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        private HtmlUpload GetHtmlUploadFromDatabaseManager(IDatabaseManager dbm)
        {
            return new HtmlUpload
            {
                TenantId = (long)dbm.DataReaderValue("TenantId"),
                ElementId = (long)dbm.DataReaderValue("ElementId"),
                HtmlUploadId = (long)dbm.DataReaderValue("HtmlUploadId"),
                ImageTenantId = dbm.DataReaderValue("ImageTenantId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("ImageTenantId"),
                ThumbnailImageUploadId = dbm.DataReaderValue("ThumbnailImageUploadId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("ThumbnailImageUploadId"),
                PreviewImageUploadId = dbm.DataReaderValue("PreviewImageUploadId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("PreviewImageUploadId"),
                ImageUploadId = dbm.DataReaderValue("ImageUploadId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("ImageUploadId"),
                UploadTenantId = dbm.DataReaderValue("UploadTenantId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("UploadTenantId"),
                UploadId = dbm.DataReaderValue("UploadId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("UploadId")
            };
        }

        public HtmlUpload ReadUpload(long tenantId, long elementId, long htmlUploadId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                HtmlUpload htmlUpload = null;
                string sql = _sqlManager.GetSql("Sql.ReadHtmlUpload.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, elementId);
                dbm.AddParameter("@HtmlUploadId", FieldType.BigInt, htmlUploadId);
                dbm.ExecuteReader();
                if (dbm.Read())
                    htmlUpload = GetHtmlUploadFromDatabaseManager(dbm);
                return htmlUpload;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        private HtmlUploadCollection GetHtmlUploadCollection(HtmlSettings settings)
        {
            HtmlUploadCollection htmlUploadCollection = new HtmlUploadCollection();
            foreach (HtmlUpload htmlUpload in settings.Uploads)
                htmlUploadCollection.Add(htmlUpload);
            return htmlUploadCollection;
        }

        public void Update(HtmlSettings settings, IUnitOfWork unitOfWork = null)
        {
            HtmlUploadCollection htmlUploadCollection = GetHtmlUploadCollection(settings);
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                string sql = _sqlManager.GetSql("Sql.UpdateHtml.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@Html", FieldType.NVarChar, -1, settings.Html);
                dbm.AddTypedParameter("@HtmlUploads", FieldType.Structured, htmlUploadCollection.Count == 0 ? null : htmlUploadCollection, "element.HtmlUploadTableType");
                dbm.ExecuteNonQuery();
                if (localUnitOfWork != null)
                    localUnitOfWork.Commit();
            }
            catch
            {
                if (localUnitOfWork != null)
                    localUnitOfWork.Rollback();
                throw;
            }
            finally
            {
                if (localUnitOfWork != null)
                    localUnitOfWork.Dispose();
            }
        }

        public void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.DeleteHtml.sql");
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
