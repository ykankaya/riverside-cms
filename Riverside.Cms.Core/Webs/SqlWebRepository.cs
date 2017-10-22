using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Webs;
using Riverside.Utilities.Data;
using Riverside.Utilities.Drawing;

namespace Riverside.Cms.Core.Webs
{
    /// <summary>
    /// SQL implementation of web repository.
    /// </summary>
    public class SqlWebRepository : IWebRepository
    {
        // Member variables
        private IDatabaseManagerFactory _databaseManagerFactory;
        private ISqlManager _sqlManager;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="databaseManagerFactory">Database manager factory.</param>
        /// <param name="sqlManager">For the retrieveal of SQL scripts.</param>
        public SqlWebRepository(IDatabaseManagerFactory databaseManagerFactory, ISqlManager sqlManager)
        {
            _databaseManagerFactory = databaseManagerFactory;
            _sqlManager = sqlManager;
        }

        /// <summary>
        /// Gets web from database manager data reader.
        /// </summary>
        /// <param name="dbm">Database manager.</param>
        /// <returns>A populated Web object.</returns>
        private Web GetWebFromDatabaseManager(IDatabaseManager dbm)
        {
            return new Web
            {
                TenantId = (long)dbm.DataReaderValue("TenantId"),
                Name = (string)dbm.DataReaderValue("Name"),
                CreateUserEnabled = (bool)dbm.DataReaderValue("CreateUserEnabled"),
                UserHasImage = (bool)dbm.DataReaderValue("UserHasImage"),
                UserThumbnailImageWidth = dbm.DataReaderValue("UserThumbnailImageWidth") == DBNull.Value ? null : (int?)dbm.DataReaderValue("UserThumbnailImageWidth"),
                UserThumbnailImageHeight = dbm.DataReaderValue("UserThumbnailImageHeight") == DBNull.Value ? null : (int?)dbm.DataReaderValue("UserThumbnailImageHeight"),
                UserThumbnailImageResizeMode = dbm.DataReaderValue("UserThumbnailImageResizeMode") == DBNull.Value ? null : (ResizeMode?)(int)dbm.DataReaderValue("UserThumbnailImageResizeMode"),
                UserPreviewImageWidth = dbm.DataReaderValue("UserPreviewImageWidth") == DBNull.Value ? null : (int?)dbm.DataReaderValue("UserPreviewImageWidth"),
                UserPreviewImageHeight = dbm.DataReaderValue("UserPreviewImageHeight") == DBNull.Value ? null : (int?)dbm.DataReaderValue("UserPreviewImageHeight"),
                UserPreviewImageResizeMode = dbm.DataReaderValue("UserPreviewImageResizeMode") == DBNull.Value ? null : (ResizeMode?)(int)dbm.DataReaderValue("UserPreviewImageResizeMode"),
                UserImageMinWidth = dbm.DataReaderValue("UserImageMinWidth") == DBNull.Value ? null : (int?)dbm.DataReaderValue("UserImageMinWidth"),
                UserImageMinHeight = dbm.DataReaderValue("UserImageMinHeight") == DBNull.Value ? null : (int?)dbm.DataReaderValue("UserImageMinHeight"),
                FontOption = dbm.DataReaderValue("FontOption") == DBNull.Value ? null : (string)dbm.DataReaderValue("FontOption"),
                ColourOption = dbm.DataReaderValue("ColourOption") == DBNull.Value ? null : (string)dbm.DataReaderValue("ColourOption")
            };
        }

        /// <summary>
        /// Searches websites.
        /// </summary>
        /// <param name="parameters">Search and paging parameters.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Search result.</returns>
        public ISearchResult<Web> Search(ISearchParameters parameters, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                List<Web> webs = new List<Web>();
                dbm.SetSQL(_sqlManager.GetSql("Sql.SearchWebs.sql"));
                dbm.AddParameter("@PageIndex", FieldType.Int, parameters.PageIndex);
                dbm.AddParameter("@PageSize", FieldType.Int, parameters.PageSize);
                dbm.AddParameter("@Search", FieldType.NVarChar, 50, parameters.Search);
                dbm.ExecuteReader();
                while (dbm.Read())
                    webs.Add(GetWebFromDatabaseManager(dbm));
                dbm.Read();
                int total = (int)dbm.DataReaderValue("Total");
                return new SearchResult<Web> { Items = webs, Total = total };
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Creates a new website.
        /// </summary>
        /// <param name="web">New website details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Create(Web web, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.CreateWeb.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, web.TenantId);
                dbm.AddParameter("@Name", FieldType.NVarChar, 256, web.Name);
                dbm.AddParameter("@CreateUserEnabled", FieldType.Bit, web.CreateUserEnabled);
                dbm.AddParameter("@UserHasImage", FieldType.Bit, web.UserHasImage);
                dbm.AddParameter("@UserThumbnailImageWidth", FieldType.Int, web.UserThumbnailImageWidth ?? (object)DBNull.Value);
                dbm.AddParameter("@UserThumbnailImageHeight", FieldType.Int, web.UserThumbnailImageHeight ?? (object)DBNull.Value);
                dbm.AddParameter("@UserThumbnailImageResizeMode", FieldType.Int, web.UserThumbnailImageResizeMode ?? (object)DBNull.Value);
                dbm.AddParameter("@UserPreviewImageWidth", FieldType.Int, web.UserPreviewImageWidth ?? (object)DBNull.Value);
                dbm.AddParameter("@UserPreviewImageHeight", FieldType.Int, web.UserPreviewImageHeight ?? (object)DBNull.Value);
                dbm.AddParameter("@UserPreviewImageResizeMode", FieldType.Int, web.UserPreviewImageResizeMode ?? (object)DBNull.Value);
                dbm.AddParameter("@UserImageMinWidth", FieldType.Int, web.UserImageMinWidth ?? (object)DBNull.Value);
                dbm.AddParameter("@UserImageMinHeight", FieldType.Int, web.UserImageMinHeight ?? (object)DBNull.Value);
                dbm.AddParameter("@FontOption", FieldType.NVarChar, 100, web.FontOption ?? (object)DBNull.Value);
                dbm.AddParameter("@ColourOption", FieldType.NVarChar, 100, web.ColourOption ?? (object)DBNull.Value);
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Gets website details.
        /// </summary>
        /// <param name="tenantId">Identifies website whose details are returned.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Website details (or null if website not found).</returns>
        public Web Read(long tenantId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                Web web = null;
                dbm.SetSQL(_sqlManager.GetSql("Sql.ReadWeb.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.ExecuteReader();
                if (dbm.Read())
                    web = GetWebFromDatabaseManager(dbm);
                return web;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Gets website details.
        /// </summary>
        /// <param name="name">Name of website whose details are returned.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Website details (or null if website not found).</returns>
        public Web ReadByName(string name, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                Web web = null;
                dbm.SetSQL(_sqlManager.GetSql("Sql.ReadWebByName.sql"));
                dbm.AddParameter("@Name", FieldType.NVarChar, 256, name);
                dbm.ExecuteReader();
                if (dbm.Read())
                    web = GetWebFromDatabaseManager(dbm);
                return web;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Updates a website's details.
        /// </summary>
        /// <param name="web">Updated website details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Update(Web web, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.UpdateWeb.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, web.TenantId);
                dbm.AddParameter("@Name", FieldType.NVarChar, 256, web.Name);
                dbm.AddParameter("@CreateUserEnabled", FieldType.Bit, web.CreateUserEnabled);
                dbm.AddParameter("@UserHasImage", FieldType.Bit, web.UserHasImage);
                dbm.AddParameter("@UserThumbnailImageWidth", FieldType.Int, web.UserThumbnailImageWidth ?? (object)DBNull.Value);
                dbm.AddParameter("@UserThumbnailImageHeight", FieldType.Int, web.UserThumbnailImageHeight ?? (object)DBNull.Value);
                dbm.AddParameter("@UserThumbnailImageResizeMode", FieldType.Int, web.UserThumbnailImageResizeMode ?? (object)DBNull.Value);
                dbm.AddParameter("@UserPreviewImageWidth", FieldType.Int, web.UserPreviewImageWidth ?? (object)DBNull.Value);
                dbm.AddParameter("@UserPreviewImageHeight", FieldType.Int, web.UserPreviewImageHeight ?? (object)DBNull.Value);
                dbm.AddParameter("@UserPreviewImageResizeMode", FieldType.Int, web.UserPreviewImageResizeMode ?? (object)DBNull.Value);
                dbm.AddParameter("@UserImageMinWidth", FieldType.Int, web.UserImageMinWidth ?? (object)DBNull.Value);
                dbm.AddParameter("@UserImageMinHeight", FieldType.Int, web.UserImageMinHeight ?? (object)DBNull.Value);
                dbm.AddParameter("@FontOption", FieldType.NVarChar, 100, web.FontOption ?? (object)DBNull.Value);
                dbm.AddParameter("@ColourOption", FieldType.NVarChar, 100, web.ColourOption ?? (object)DBNull.Value);
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Update theme options.
        /// </summary>
        /// <param name="tenantId">Identifies website whose theme options are updated.</param>
        /// <param name="fontOption">Font option.</param>
        /// <param name="colourOption">Colour option.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void UpdateThemeOptions(long tenantId, string fontOption, string colourOption, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.UpdateThemeOptions.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@FontOption", FieldType.NVarChar, 100, fontOption ?? (object)DBNull.Value);
                dbm.AddParameter("@ColourOption", FieldType.NVarChar, 100, colourOption ?? (object)DBNull.Value);
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Deletes a website.
        /// </summary>
        /// <param name="tenantId">Identifies website to delete.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Delete(long tenantId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.DeleteWeb.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
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
