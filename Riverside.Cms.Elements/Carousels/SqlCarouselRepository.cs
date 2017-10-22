using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.Carousels
{
    public class SqlCarouselRepository : ICarouselRepository
    {
        private IDatabaseManagerFactory _databaseManagerFactory;
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private ISqlManager _sqlManager;

        public SqlCarouselRepository(IDatabaseManagerFactory databaseManagerFactory, IUnitOfWorkFactory unitOfWorkFactory, ISqlManager sqlManager)
        {
            _databaseManagerFactory = databaseManagerFactory;
            _unitOfWorkFactory = unitOfWorkFactory;
            _sqlManager = sqlManager;
        }

        public void Create(CarouselSettings settings, IUnitOfWork unitOfWork = null)
        {
        }

        public void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null)
        {
        }

        private CarouselSlide GetCarouselSlideFromDatabaseManager(IDatabaseManager dbm)
        {
            return new CarouselSlide
            {
                TenantId = (long)dbm.DataReaderValue("TenantId"),
                ElementId = (long)dbm.DataReaderValue("ElementId"),
                CarouselSlideId = (long)dbm.DataReaderValue("CarouselSlideId"),
                ImageTenantId = (long)dbm.DataReaderValue("ImageTenantId"),
                ThumbnailImageUploadId = (long)dbm.DataReaderValue("ThumbnailImageUploadId"),
                PreviewImageUploadId = (long)dbm.DataReaderValue("PreviewImageUploadId"),
                ImageUploadId = (long)dbm.DataReaderValue("ImageUploadId"),
                Name = dbm.DataReaderValue("Name") == DBNull.Value ? null : (string)dbm.DataReaderValue("Name"),
                Description = dbm.DataReaderValue("Description") == DBNull.Value ? null : (string)dbm.DataReaderValue("Description"),
                PageTenantId = dbm.DataReaderValue("PageTenantId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("PageTenantId"),
                PageId = dbm.DataReaderValue("PageId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("PageId"),
                PageText = dbm.DataReaderValue("PageText") == DBNull.Value ? null : (string)dbm.DataReaderValue("PageText"),
                SortOrder = (int)dbm.DataReaderValue("SortOrder")
            };
        }

        public void Read(CarouselSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.ReadCarousel.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.ExecuteReader();
                settings.Slides = new List<CarouselSlide>();
                while (dbm.Read())
                    settings.Slides.Add(GetCarouselSlideFromDatabaseManager(dbm));
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public CarouselSlide ReadSlide(long tenantId, long elementId, long carouselSlideId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                CarouselSlide slide = null;
                string sql = _sqlManager.GetSql("Sql.ReadCarouselSlide.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, elementId);
                dbm.AddParameter("@CarouselSlideId", FieldType.BigInt, carouselSlideId);
                dbm.ExecuteReader();
                if (dbm.Read())
                    slide = GetCarouselSlideFromDatabaseManager(dbm);
                return slide;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        private CarouselSlideCollection GetCarouselSlideCollection(CarouselSettings settings)
        {
            CarouselSlideCollection carouselSlideCollection = new CarouselSlideCollection();
            foreach (CarouselSlide slide in settings.Slides)
                carouselSlideCollection.Add(slide);
            return carouselSlideCollection;
        }

        public void Update(CarouselSettings settings, IUnitOfWork unitOfWork = null)
        {
            CarouselSlideCollection carouselSlideCollection = GetCarouselSlideCollection(settings);
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                string sql = _sqlManager.GetSql("Sql.UpdateCarousel.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddTypedParameter("@CarouselSlides", FieldType.Structured, carouselSlideCollection.Count == 0 ? null : carouselSlideCollection, "element.CarouselSlideTableType");
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
                string sql = _sqlManager.GetSql("Sql.DeleteCarousel.sql");
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
