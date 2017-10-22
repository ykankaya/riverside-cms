using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.Albums
{
    public class SqlAlbumRepository : IAlbumRepository
    {
        private IDatabaseManagerFactory _databaseManagerFactory;
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private ISqlManager _sqlManager;

        public SqlAlbumRepository(IDatabaseManagerFactory databaseManagerFactory, IUnitOfWorkFactory unitOfWorkFactory, ISqlManager sqlManager)
        {
            _databaseManagerFactory = databaseManagerFactory;
            _unitOfWorkFactory = unitOfWorkFactory;
            _sqlManager = sqlManager;
        }

        public void Create(AlbumSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.CreateAlbum.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@DisplayName", FieldType.NVarChar, 256, settings.DisplayName ?? (object)DBNull.Value);
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
                dbm.SetSQL(_sqlManager.GetSql("Sql.CopyAlbum.sql"));
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

        private AlbumPhoto GetAlbumPhotoFromDatabaseManager(IDatabaseManager dbm)
        {
            return new AlbumPhoto
            {
                TenantId = (long)dbm.DataReaderValue("TenantId"),
                ElementId = (long)dbm.DataReaderValue("ElementId"),
                AlbumPhotoId = (long)dbm.DataReaderValue("AlbumPhotoId"),
                ImageTenantId = (long)dbm.DataReaderValue("ImageTenantId"),
                ThumbnailImageUploadId = (long)dbm.DataReaderValue("ThumbnailImageUploadId"),
                PreviewImageUploadId = (long)dbm.DataReaderValue("PreviewImageUploadId"),
                ImageUploadId = (long)dbm.DataReaderValue("ImageUploadId"),
                Name = dbm.DataReaderValue("Name") == DBNull.Value ? null : (string)dbm.DataReaderValue("Name"),
                Description = dbm.DataReaderValue("Description") == DBNull.Value ? null : (string)dbm.DataReaderValue("Description"),
                SortOrder = (int)dbm.DataReaderValue("SortOrder")
            };
        }

        public void Read(AlbumSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.ReadAlbum.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.ExecuteReader();
                dbm.Read();
                settings.DisplayName = dbm.DataReaderValue("DisplayName") == DBNull.Value ? null : (string)dbm.DataReaderValue("DisplayName");
                dbm.Read();
                settings.Photos = new List<AlbumPhoto>();
                while (dbm.Read())
                    settings.Photos.Add(GetAlbumPhotoFromDatabaseManager(dbm));
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public AlbumPhoto ReadPhoto(long tenantId, long elementId, long albumPhotoId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                AlbumPhoto photo = null;
                dbm.SetSQL(_sqlManager.GetSql("Sql.ReadAlbumPhoto.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, elementId);
                dbm.AddParameter("@AlbumPhotoId", FieldType.BigInt, albumPhotoId);
                dbm.ExecuteReader();
                if (dbm.Read())
                    photo = GetAlbumPhotoFromDatabaseManager(dbm);
                return photo;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        private AlbumPhotoCollection GetAlbumPhotoCollection(AlbumSettings settings)
        {
            AlbumPhotoCollection albumPhotoCollection = new AlbumPhotoCollection();
            foreach (AlbumPhoto photo in settings.Photos)
                albumPhotoCollection.Add(photo);
            return albumPhotoCollection;
        }

        public void Update(AlbumSettings settings, IUnitOfWork unitOfWork = null)
        {
            AlbumPhotoCollection albumPhotoCollection = GetAlbumPhotoCollection(settings);
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                dbm.SetSQL(_sqlManager.GetSql("Sql.UpdateAlbum.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@DisplayName", FieldType.NVarChar, 256, settings.DisplayName ?? (object)DBNull.Value);
                dbm.AddTypedParameter("@AlbumPhotos", FieldType.Structured, albumPhotoCollection.Count == 0 ? null : albumPhotoCollection, "element.AlbumPhotoTableType");
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
                dbm.SetSQL(_sqlManager.GetSql("Sql.DeleteAlbum.sql"));
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
