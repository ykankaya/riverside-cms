using Riverside.Cms.Core.Pages;
using Riverside.Utilities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.PageList
{
    public class SqlPageListRepository : IPageListRepository
    {
        private IDatabaseManagerFactory _databaseManagerFactory;
        private ISqlManager _sqlManager;

        public SqlPageListRepository(IDatabaseManagerFactory databaseManagerFactory, ISqlManager sqlManager)
        {
            _databaseManagerFactory = databaseManagerFactory;
            _sqlManager = sqlManager;
        }

        public void Create(PageListSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.CreatePageList.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@PageTenantId", FieldType.BigInt, settings.PageTenantId ?? (object)DBNull.Value);
                dbm.AddParameter("@PageId", FieldType.BigInt, settings.PageId ?? (object)DBNull.Value);
                dbm.AddParameter("@DisplayName", FieldType.NVarChar, 256, settings.DisplayName ?? (object)DBNull.Value);
                dbm.AddParameter("@SortBy", FieldType.Int, (int)settings.SortBy);
                dbm.AddParameter("@SortAsc", FieldType.Bit, settings.SortAsc);
                dbm.AddParameter("@ShowRelated", FieldType.Bit, settings.ShowRelated);
                dbm.AddParameter("@ShowDescription", FieldType.Bit, settings.ShowDescription);
                dbm.AddParameter("@ShowImage", FieldType.Bit, settings.ShowImage);
                dbm.AddParameter("@ShowBackgroundImage", FieldType.Bit, settings.ShowBackgroundImage);
                dbm.AddParameter("@ShowCreated", FieldType.Bit, settings.ShowCreated);
                dbm.AddParameter("@ShowUpdated", FieldType.Bit, settings.ShowUpdated);
                dbm.AddParameter("@ShowOccurred", FieldType.Bit, settings.ShowOccurred);
                dbm.AddParameter("@ShowComments", FieldType.Bit, settings.ShowComments);
                dbm.AddParameter("@ShowTags", FieldType.Bit, settings.ShowTags);
                dbm.AddParameter("@ShowPager", FieldType.Bit, settings.ShowPager);
                dbm.AddParameter("@MoreMessage", FieldType.NVarChar, 256, settings.MoreMessage ?? (object)DBNull.Value);
                dbm.AddParameter("@Recursive", FieldType.Bit, settings.Recursive);
                dbm.AddParameter("@PageType", FieldType.Int, (int)settings.PageType);
                dbm.AddParameter("@PageSize", FieldType.Int, settings.PageSize);
                dbm.AddParameter("@NoPagesMessage", FieldType.NVarChar, -1, settings.NoPagesMessage ?? (object)DBNull.Value);
                dbm.AddParameter("@Preamble", FieldType.NVarChar, -1, settings.Preamble ?? (object)DBNull.Value);
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
                dbm.SetSQL(_sqlManager.GetSql("Sql.CopyPageList.sql"));
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

        public void Read(PageListSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.ReadPageList.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.ExecuteReader();
                dbm.Read();
                settings.PageTenantId = dbm.DataReaderValue("PageTenantId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("PageTenantId");
                settings.PageId = dbm.DataReaderValue("PageId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("PageId");
                settings.DisplayName = dbm.DataReaderValue("DisplayName") == DBNull.Value ? null : (string)dbm.DataReaderValue("DisplayName");
                settings.SortBy = (PageSortBy)(int)dbm.DataReaderValue("SortBy");
                settings.SortAsc = (bool)dbm.DataReaderValue("SortAsc");
                settings.ShowRelated = (bool)dbm.DataReaderValue("ShowRelated");
                settings.ShowDescription = (bool)dbm.DataReaderValue("ShowDescription");
                settings.ShowImage = (bool)dbm.DataReaderValue("ShowImage");
                settings.ShowBackgroundImage = (bool)dbm.DataReaderValue("ShowBackgroundImage");
                settings.ShowCreated = (bool)dbm.DataReaderValue("ShowCreated");
                settings.ShowUpdated = (bool)dbm.DataReaderValue("ShowUpdated");
                settings.ShowOccurred = (bool)dbm.DataReaderValue("ShowOccurred");
                settings.ShowComments = (bool)dbm.DataReaderValue("ShowComments");
                settings.ShowTags = (bool)dbm.DataReaderValue("ShowTags");
                settings.ShowPager = (bool)dbm.DataReaderValue("ShowPager");
                settings.MoreMessage = dbm.DataReaderValue("MoreMessage") == DBNull.Value ? null : (string)dbm.DataReaderValue("MoreMessage");
                settings.Recursive = (bool)dbm.DataReaderValue("Recursive");
                settings.PageType = (PageType)(int)dbm.DataReaderValue("PageType");
                settings.PageSize = (int)dbm.DataReaderValue("PageSize");
                settings.NoPagesMessage = dbm.DataReaderValue("NoPagesMessage") == DBNull.Value ? null : (string)dbm.DataReaderValue("NoPagesMessage");
                settings.Preamble = dbm.DataReaderValue("Preamble") == DBNull.Value ? null : (string)dbm.DataReaderValue("Preamble");
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public void Update(PageListSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.UpdatePageList.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@PageTenantId", FieldType.BigInt, settings.PageTenantId ?? (object)DBNull.Value);
                dbm.AddParameter("@PageId", FieldType.BigInt, settings.PageId ?? (object)DBNull.Value);
                dbm.AddParameter("@DisplayName", FieldType.NVarChar, 256, settings.DisplayName ?? (object)DBNull.Value);
                dbm.AddParameter("@SortBy", FieldType.Int, (int)settings.SortBy);
                dbm.AddParameter("@SortAsc", FieldType.Bit, settings.SortAsc);
                dbm.AddParameter("@ShowRelated", FieldType.Bit, settings.ShowRelated);
                dbm.AddParameter("@ShowDescription", FieldType.Bit, settings.ShowDescription);
                dbm.AddParameter("@ShowImage", FieldType.Bit, settings.ShowImage);
                dbm.AddParameter("@ShowBackgroundImage", FieldType.Bit, settings.ShowBackgroundImage);
                dbm.AddParameter("@ShowCreated", FieldType.Bit, settings.ShowCreated);
                dbm.AddParameter("@ShowUpdated", FieldType.Bit, settings.ShowUpdated);
                dbm.AddParameter("@ShowOccurred", FieldType.Bit, settings.ShowOccurred);
                dbm.AddParameter("@ShowComments", FieldType.Bit, settings.ShowComments);
                dbm.AddParameter("@ShowTags", FieldType.Bit, settings.ShowTags);
                dbm.AddParameter("@ShowPager", FieldType.Bit, settings.ShowPager);
                dbm.AddParameter("@MoreMessage", FieldType.NVarChar, 256, settings.MoreMessage ?? (object)DBNull.Value);
                dbm.AddParameter("@Recursive", FieldType.Bit, settings.Recursive);
                dbm.AddParameter("@PageType", FieldType.Int, (int)settings.PageType);
                dbm.AddParameter("@PageSize", FieldType.Int, settings.PageSize);
                dbm.AddParameter("@NoPagesMessage", FieldType.NVarChar, -1, settings.NoPagesMessage ?? (object)DBNull.Value);
                dbm.AddParameter("@Preamble", FieldType.NVarChar, -1, settings.Preamble ?? (object)DBNull.Value);
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
                dbm.SetSQL(_sqlManager.GetSql("Sql.DeletePageList.sql"));
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
