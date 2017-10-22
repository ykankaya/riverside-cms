using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Uploads;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Uploads
{
    /// <summary>
    /// SQL implementation of upload repository.
    /// </summary>
    public class SqlUploadRepository : IUploadRepository
    {
        // Member variables
        private IDatabaseManagerFactory _databaseManagerFactory;
        private IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="databaseManagerFactory">Database manager factory.</param>
        /// <param name="unitOfWorkFactory">Unit of work factory.</param>
        public SqlUploadRepository(IDatabaseManagerFactory databaseManagerFactory, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _databaseManagerFactory = databaseManagerFactory;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Creates a new upload.
        /// </summary>
        /// <param name="upload">New upload details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Newly allocated upload identifier.</returns>
        public long CreateUpload(Upload upload, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetStoredProcedure("cms.CreateUpload");
                dbm.AddParameter("@TenantId", FieldType.BigInt, upload.TenantId);
                dbm.AddParameter("@UploadType", FieldType.Int, (int)upload.UploadType);
                dbm.AddParameter("@Name", FieldType.NVarChar, -1, upload.Name);
                dbm.AddParameter("@Size", FieldType.Int, upload.Size);
                dbm.AddParameter("@Committed", FieldType.Bit, upload.Committed);
                dbm.AddParameter("@Created", FieldType.DateTime, upload.Created);
                dbm.AddParameter("@Updated", FieldType.DateTime, upload.Updated);
                dbm.AddOutputParameter("@UploadId", FieldType.BigInt);
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                dbm.ExecuteNonQuery(outputValues);
                return (long)outputValues["@UploadId"];
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Creates a new image.
        /// </summary>
        /// <param name="image">New image details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Newly allocated upload identifier.</returns>
        public long CreateImage(Image image, IUnitOfWork unitOfWork = null)
        {
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                dbm.SetStoredProcedure("cms.CreateImage");
                dbm.AddParameter("@TenantId", FieldType.BigInt, image.TenantId);
                dbm.AddParameter("@UploadType", FieldType.Int, (int)image.UploadType);
                dbm.AddParameter("@Name", FieldType.NVarChar, -1, image.Name);
                dbm.AddParameter("@Size", FieldType.Int, image.Size);
                dbm.AddParameter("@Committed", FieldType.Bit, image.Committed);
                dbm.AddParameter("@Created", FieldType.DateTime, image.Created);
                dbm.AddParameter("@Updated", FieldType.DateTime, image.Updated);
                dbm.AddParameter("@Width", FieldType.Int, image.Width);
                dbm.AddParameter("@Height", FieldType.Int, image.Height);
                dbm.AddOutputParameter("@UploadId", FieldType.BigInt);
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                dbm.ExecuteNonQuery(outputValues);
                if (localUnitOfWork != null)
                    localUnitOfWork.Commit();
                return (long)outputValues["@UploadId"];
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

        /// <summary>
        /// Gets upload from database manager data reader.
        /// </summary>
        /// <param name="dbm">Database manager.</param>
        /// <returns>A populated Upload object.</returns>
        private Upload GetUploadFromDatabaseManager(IDatabaseManager dbm)
        {
            return new Upload
            {
                TenantId = (long)dbm.DataReaderValue("TenantId"),
                UploadId = (long)dbm.DataReaderValue("UploadId"),
                UploadType = UploadType.Upload,
                Name = (string)dbm.DataReaderValue("Name"),
                Size = (int)dbm.DataReaderValue("Size"),
                Created = (DateTime)dbm.DataReaderValue("Created")
            };
        }

        /// <summary>
        /// Gets image from database manager data reader.
        /// </summary>
        /// <param name="dbm">Database manager.</param>
        /// <returns>A populated Image object.</returns>
        private Image GetImageFromDatabaseManager(IDatabaseManager dbm)
        {
            return new Image
            {
                TenantId = (long)dbm.DataReaderValue("TenantId"),
                UploadId = (long)dbm.DataReaderValue("UploadId"),
                UploadType = UploadType.Image,
                Name = (string)dbm.DataReaderValue("Name"),
                Size = (int)dbm.DataReaderValue("Size"),
                Created = (DateTime)dbm.DataReaderValue("Created"),
                Width = (int)dbm.DataReaderValue("Width"),
                Height = (int)dbm.DataReaderValue("Height")
            };
        }

        /// <summary>
        /// Gets upload details.
        /// </summary>
        /// <param name="tenantId">Identifies tenant whose upload is returned.</param>
        /// <param name="uploadId">Identifies the upload to return.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Upload details (or null if upload not found).</returns>
        public Upload Read(long tenantId, long uploadId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                Upload upload = null;
                dbm.SetStoredProcedure("cms.ReadUpload");
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@UploadId", FieldType.BigInt, uploadId);
                dbm.ExecuteReader();
                if (dbm.Read())
                {
                    UploadType uploadType = (UploadType)(int)dbm.DataReaderValue("UploadType");
                    switch (uploadType)
                    {
                        case UploadType.Image:
                            upload = GetImageFromDatabaseManager(dbm);
                            break;

                        case UploadType.Upload:
                            upload = GetUploadFromDatabaseManager(dbm);
                            break;
                    }
                }
                return upload;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Update upload details.
        /// </summary>
        /// <param name="upload">Updated upload details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void UpdateUpload(Upload upload, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetStoredProcedure("cms.UpdateUpload");
                dbm.AddParameter("@TenantId", FieldType.BigInt, upload.TenantId);
                dbm.AddParameter("@UploadId", FieldType.BigInt, upload.UploadId);
                dbm.AddParameter("@UploadType", FieldType.Int, (int)upload.UploadType);
                dbm.AddParameter("@Name", FieldType.NVarChar, -1, upload.Name);
                dbm.AddParameter("@Size", FieldType.Int, upload.Size);
                dbm.AddParameter("@Committed", FieldType.Bit, upload.Committed);
                dbm.AddParameter("@Created", FieldType.DateTime, upload.Created);
                dbm.AddParameter("@Updated", FieldType.DateTime, upload.Updated);
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Update image details.
        /// </summary>
        /// <param name="upload">Updated image details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void UpdateImage(Image image, IUnitOfWork unitOfWork = null)
        {
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                dbm.SetStoredProcedure("cms.UpdateImage");
                dbm.AddParameter("@TenantId", FieldType.BigInt, image.TenantId);
                dbm.AddParameter("@UploadId", FieldType.BigInt, image.UploadId);
                dbm.AddParameter("@UploadType", FieldType.Int, (int)image.UploadType);
                dbm.AddParameter("@Name", FieldType.NVarChar, -1, image.Name);
                dbm.AddParameter("@Size", FieldType.Int, image.Size);
                dbm.AddParameter("@Committed", FieldType.Bit, image.Committed);
                dbm.AddParameter("@Created", FieldType.DateTime, image.Created);
                dbm.AddParameter("@Updated", FieldType.DateTime, image.Updated);
                dbm.AddParameter("@Width", FieldType.Int, image.Width);
                dbm.AddParameter("@Height", FieldType.Int, image.Height);
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

        /// <summary>
        /// Deletes an upload.
        /// </summary>
        /// <param name="tenantId">Identifies tenant whose upload is to be deleted.</param>
        /// <param name="uploadId">Identifies the upload to delete.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Delete(long tenantId, long uploadId, IUnitOfWork unitOfWork = null)
        {
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                dbm.SetStoredProcedure("cms.DeleteUpload");
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@UploadId", FieldType.BigInt, uploadId);
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
    }
}