using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.MasterPages;
using Riverside.Cms.Core.Pages;
using Riverside.Utilities.Data;
using Riverside.Utilities.Drawing;

namespace Riverside.Cms.Core.MasterPages
{
    /// <summary>
    /// SQL implementation of master page repository.
    /// </summary>
    public class SqlMasterPageRepository : IMasterPageRepository
    {
        // Member variables
        private IDatabaseManagerFactory _databaseManagerFactory;
        private ISqlManager             _sqlManager;
        private IUnitOfWorkFactory      _unitOfWorkFactory;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="databaseManagerFactory">For the creation of database managers.</param>
        /// <param name="sqlManager">For the retrieval of SQL files.</param>
        /// <param name="unitOfWorkFactory">For the creation of units of work.</param>
        public SqlMasterPageRepository(IDatabaseManagerFactory databaseManagerFactory, ISqlManager sqlManager, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _databaseManagerFactory = databaseManagerFactory;
            _sqlManager             = sqlManager;
            _unitOfWorkFactory      = unitOfWorkFactory;
        }

        /// <summary>
        /// Gets master page from database manager data reader.
        /// </summary>
        /// <param name="dbm">Database manager.</param>
        /// <returns>A populated master page object.</returns>
        private MasterPage GetMasterPageFromDatabaseManager(IDatabaseManager dbm)
        {
            return new MasterPage {
                TenantId                 = (long)dbm.DataReaderValue("TenantId"),
                MasterPageId             = (long)dbm.DataReaderValue("MasterPageId"),
                PageName                 = (string)dbm.DataReaderValue("PageName"),
                PageDescription          = dbm.DataReaderValue("PageDescription")   == DBNull.Value ? null : (string)dbm.DataReaderValue("PageDescription"),
                AncestorPageId           = dbm.DataReaderValue("AncestorPageId")    == DBNull.Value ? null : (long?)dbm.DataReaderValue("AncestorPageId"),
                AncestorPageLevel        = dbm.DataReaderValue("AncestorPageLevel") == DBNull.Value ? null : (PageLevel?)(int)dbm.DataReaderValue("AncestorPageLevel"),
                PageType                 = (PageType)(int)dbm.DataReaderValue("PageType"),
                HasOccurred              = (bool)dbm.DataReaderValue("HasOccurred"),
                HasImage                 = (bool)dbm.DataReaderValue("HasImage"),
                ThumbnailImageWidth      = dbm.DataReaderValue("ThumbnailImageWidth") == DBNull.Value ? null : (int?)dbm.DataReaderValue("ThumbnailImageWidth"),
                ThumbnailImageHeight     = dbm.DataReaderValue("ThumbnailImageHeight") == DBNull.Value ? null : (int?)dbm.DataReaderValue("ThumbnailImageHeight"),
                ThumbnailImageResizeMode = dbm.DataReaderValue("ThumbnailImageResizeMode") == DBNull.Value ? null : (ResizeMode?)(int)dbm.DataReaderValue("ThumbnailImageResizeMode"),
                PreviewImageWidth        = dbm.DataReaderValue("PreviewImageWidth") == DBNull.Value ? null : (int?)dbm.DataReaderValue("PreviewImageWidth"),
                PreviewImageHeight       = dbm.DataReaderValue("PreviewImageHeight") == DBNull.Value ? null : (int?)dbm.DataReaderValue("PreviewImageHeight"),
                PreviewImageResizeMode   = dbm.DataReaderValue("PreviewImageResizeMode") == DBNull.Value ? null : (ResizeMode?)(int)dbm.DataReaderValue("PreviewImageResizeMode"),
                ImageMinWidth            = dbm.DataReaderValue("ImageMinWidth") == DBNull.Value ? null : (int?)dbm.DataReaderValue("ImageMinWidth"),
                ImageMinHeight           = dbm.DataReaderValue("ImageMinHeight") == DBNull.Value ? null : (int?)dbm.DataReaderValue("ImageMinHeight"),
                Name                     = (string)dbm.DataReaderValue("Name"),
                Creatable                = (bool)dbm.DataReaderValue("Creatable"),
                Deletable                = (bool)dbm.DataReaderValue("Deletable"),
                Taggable                 = (bool)dbm.DataReaderValue("Taggable"),
                Administration           = (bool)dbm.DataReaderValue("Administration"),
                BeginRender              = dbm.DataReaderValue("BeginRender") == DBNull.Value ? null : (string)dbm.DataReaderValue("BeginRender"),
                EndRender                = dbm.DataReaderValue("EndRender") == DBNull.Value ? null : (string)dbm.DataReaderValue("EndRender"),
                MasterPageZones          = new List<MasterPageZone>()
            };
        }

        /// <summary>
        /// Gets master page zone from database manager data reader.
        /// </summary>
        /// <param name="dbm">Database manager.</param>
        /// <returns>A populated master page zone object.</returns>
        private MasterPageZone GetMasterPageZoneFromDatabaseManager(IDatabaseManager dbm)
        {
            MasterPageZone masterPageZone = new MasterPageZone {
                TenantId                   = (long)dbm.DataReaderValue("TenantId"),
                MasterPageId               = (long)dbm.DataReaderValue("MasterPageId"),
                MasterPageZoneId           = (long)dbm.DataReaderValue("MasterPageZoneId"),
                Name                       = (string)dbm.DataReaderValue("Name"),
                SortOrder                  = (int)dbm.DataReaderValue("SortOrder"),
                AdminType                  = (MasterPageZoneAdminType)(int)dbm.DataReaderValue("AdminType"),
                ContentType                = (MasterPageZoneContentType)(int)dbm.DataReaderValue("ContentType"),
                BeginRender                = dbm.DataReaderValue("BeginRender") == DBNull.Value ? null : (string)dbm.DataReaderValue("BeginRender"),
                EndRender                  = dbm.DataReaderValue("EndRender") == DBNull.Value ? null : (string)dbm.DataReaderValue("EndRender"),
                MasterPageZoneElementTypes = new List<MasterPageZoneElementType>(),
                MasterPageZoneElements     = new List<MasterPageZoneElement>()
            };
            return masterPageZone;
        }

        /// <summary>
        /// Gets master page zone element type from database manager data reader.
        /// </summary>
        /// <param name="dbm">Database manager.</param>
        /// <returns>A populated master page zone element type object.</returns>
        private MasterPageZoneElementType GetMasterPageZoneElementTypeFromDatabaseManager(IDatabaseManager dbm)
        {
            ElementType elementType = new ElementType
            {
                ElementTypeId = (Guid)dbm.DataReaderValue("ElementTypeId"),
                Name = (string)dbm.DataReaderValue("Name")
            };
            MasterPageZoneElementType masterPageZoneElementType = new MasterPageZoneElementType {
                TenantId = (long)dbm.DataReaderValue("TenantId"),
                MasterPageId = (long)dbm.DataReaderValue("MasterPageId"),
                MasterPageZoneId = (long)dbm.DataReaderValue("MasterPageZoneId"),
                ElementTypeId = elementType.ElementTypeId,
                ElementType = elementType
            };
            return masterPageZoneElementType;
        }

        /// <summary>
        /// Gets master page zone element from database manager data reader.
        /// </summary>
        /// <param name="dbm">Database manager.</param>
        /// <returns>A populated master page zone element object.</returns>
        private MasterPageZoneElement GetMasterPageZoneElementFromDatabaseManager(IDatabaseManager dbm)
        {
            ElementSettings element = new ElementSettings
            {
                TenantId      = (long)dbm.DataReaderValue("TenantId"),
                ElementId     = (long)dbm.DataReaderValue("ElementId"),
                ElementTypeId = (Guid)dbm.DataReaderValue("ElementTypeId"),
                Name          = (string)dbm.DataReaderValue("Name")
            };
            return new MasterPageZoneElement {
                TenantId                = element.TenantId,
                MasterPageId            = (long)dbm.DataReaderValue("MasterPageId"),
                MasterPageZoneId        = (long)dbm.DataReaderValue("MasterPageZoneId"),
                MasterPageZoneElementId = (long)dbm.DataReaderValue("MasterPageZoneElementId"),
                SortOrder               = (int)dbm.DataReaderValue("SortOrder"),
                ElementId               = element.ElementId,
                Element                 = element,
                BeginRender             = dbm.DataReaderValue("BeginRender") == DBNull.Value ? null : (string)dbm.DataReaderValue("BeginRender"),
                EndRender               = dbm.DataReaderValue("EndRender") == DBNull.Value ? null : (string)dbm.DataReaderValue("EndRender")
            };
        }

        /// <summary>
        /// Gets master page from database manager result.
        /// </summary>
        /// <param name="dbm">Database manager.</param>
        /// <param name="loadZonesAndElements">Indicates depth of data loaded.</param>
        /// <returns>Master page (or null if master page not found).</returns>
        private MasterPage GetMasterPage(IDatabaseManager dbm, bool loadZonesAndElements)
        {
            // Get master page
            if (loadZonesAndElements && !dbm.Read())
                return null;
            MasterPage masterPage = GetMasterPageFromDatabaseManager(dbm);

            // If not read, we can return result now
            if (!loadZonesAndElements)
                return masterPage;

            // Get master page zones
            dbm.Read();
            Dictionary<long, MasterPageZone> masterPageZonesById = new Dictionary<long, MasterPageZone>();
            while (dbm.Read())
            {
                MasterPageZone masterPageZone = GetMasterPageZoneFromDatabaseManager(dbm);
                masterPage.MasterPageZones.Add(masterPageZone);
                masterPageZonesById.Add(masterPageZone.MasterPageZoneId, masterPageZone);
            }

            // Get the element types that can exist in a master page zone when admin type is configurable
            while (dbm.Read())
            {
                MasterPageZoneElementType masterPageZoneElementType = GetMasterPageZoneElementTypeFromDatabaseManager(dbm);
                masterPageZonesById[masterPageZoneElementType.MasterPageZoneId].MasterPageZoneElementTypes.Add(masterPageZoneElementType);
            }

            // Get master page zone elements
            while (dbm.Read())
            {
                MasterPageZoneElement masterPageZoneElement = GetMasterPageZoneElementFromDatabaseManager(dbm);
                masterPageZonesById[masterPageZoneElement.MasterPageZoneId].MasterPageZoneElements.Add(masterPageZoneElement);
            }

            // Return the result
            return masterPage;
        }

        /// <summary>
        /// Searches master pages.
        /// </summary>
        /// <param name="tenantId">Identifies website that master pages belong to.</param>
        /// <param name="parameters">Search and paging parameters.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Search result.</returns>
        public ISearchResult<MasterPage> Search(long tenantId, ISearchParameters parameters, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                List<MasterPage> masterPages = new List<MasterPage>();
                dbm.SetSQL(_sqlManager.GetSql("Sql.SearchMasterPages.sql"));
                dbm.AddParameter("@TenantId",  FieldType.BigInt, tenantId);
                dbm.AddParameter("@PageIndex", FieldType.Int, parameters.PageIndex);
                dbm.AddParameter("@PageSize",  FieldType.Int, parameters.PageSize);
                dbm.AddParameter("@Search",    FieldType.NVarChar, 50, parameters.Search);
                dbm.ExecuteReader();
                while (dbm.Read())
                    masterPages.Add(GetMasterPage(dbm, false));
                dbm.Read();
                int total = (int)dbm.DataReaderValue("Total");
                return new SearchResult<MasterPage> { Items = masterPages, Total = total };
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Lists a website's master pages that can be created.
        /// </summary>
        /// <param name="tenantId">Identifies website that master pages belong to.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Collection of master pages that can be created.</returns>
        public IEnumerable<MasterPage> ListCreatable(long tenantId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                List<MasterPage> masterPages = new List<MasterPage>();
                dbm.SetSQL(_sqlManager.GetSql("Sql.ListCreatableMasterPages.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.ExecuteReader();
                while (dbm.Read())
                    masterPages.Add(GetMasterPage(dbm, false));
                return masterPages;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Populates zone element and zone element type collections for table types.
        /// </summary>
        /// <param name="masterPageZone">Master page zone.</param>
        /// <param name="masterPageZoneElementTypeCollection">Master page zone element type collection.</param>
        /// <param name="masterPageZoneElementCollection">Master page zone element collection.</param>
        private void PopulateMasterPageZoneCollections(MasterPageZone masterPageZone, MasterPageZoneElementTypeCollection masterPageZoneElementTypeCollection, MasterPageZoneElementCollection masterPageZoneElementCollection)
        {
            foreach (MasterPageZoneElementType masterPageZoneElementType in masterPageZone.MasterPageZoneElementTypes)
            {
                masterPageZoneElementType.MasterPageZoneSortOrder = masterPageZone.SortOrder;
                masterPageZoneElementTypeCollection.Add(masterPageZoneElementType);
            }
            foreach (MasterPageZoneElement masterPageZoneElement in masterPageZone.MasterPageZoneElements)
            {
                masterPageZoneElement.MasterPageZoneSortOrder = masterPageZone.SortOrder;
                masterPageZoneElementCollection.Add(masterPageZoneElement);
            }
        }

        /// <summary>
        /// Get zone, zone element and zone element type collections for table types.
        /// </summary>
        /// <param name="masterPage">Master page.</param>
        /// <param name="masterPageZoneCollection">Master page zone collection.</param>
        /// <param name="masterPageZoneElementTypeCollection">Master page zone element type collection.</param>
        /// <param name="masterPageZoneElementCollection">Master page zone element collection.</param>
        private void PopulateMasterPageCollections(MasterPage masterPage, MasterPageZoneCollection masterPageZoneCollection, MasterPageZoneElementTypeCollection masterPageZoneElementTypeCollection, MasterPageZoneElementCollection masterPageZoneElementCollection)
        {
            foreach (MasterPageZone masterPageZone in masterPage.MasterPageZones)
            {
                masterPageZoneCollection.Add(masterPageZone);
                PopulateMasterPageZoneCollections(masterPageZone, masterPageZoneElementTypeCollection, masterPageZoneElementCollection);
            }
        }

        /// <summary>
        /// Adds parameters to database manager that are to do with master page details.
        /// </summary>
        /// <param name="masterPage">Master page.</param>
        /// <param name="dbm">The database manager where parameters are added.</param>
        private void AddMasterPageDetailsParameters(MasterPage masterPage, IDatabaseManager dbm)
        {
            dbm.AddParameter("@Name", FieldType.NVarChar, MasterPageLengths.NameMaxLength, masterPage.Name);
            dbm.AddParameter("@PageName", FieldType.NVarChar, MasterPageLengths.PageNameMaxLength, masterPage.PageName);
            dbm.AddParameter("@PageDescription", FieldType.NVarChar, -1,  masterPage.PageDescription ?? (object)DBNull.Value);
            dbm.AddParameter("@AncestorPageId", FieldType.BigInt, masterPage.AncestorPageId ?? (object)DBNull.Value);
            dbm.AddParameter("@AncestorPageLevel", FieldType.Int, masterPage.AncestorPageLevel ?? (object)DBNull.Value);
            dbm.AddParameter("@PageType", FieldType.Int, (int)masterPage.PageType);
            dbm.AddParameter("@HasOccurred", FieldType.Bit, masterPage.HasOccurred);
            dbm.AddParameter("@HasImage", FieldType.Bit, masterPage.HasImage);
            dbm.AddParameter("@ThumbnailImageWidth", FieldType.Int, masterPage.ThumbnailImageWidth ?? (object)DBNull.Value);
            dbm.AddParameter("@ThumbnailImageHeight", FieldType.Int, masterPage.ThumbnailImageHeight ?? (object)DBNull.Value);
            dbm.AddParameter("@ThumbnailImageResizeMode", FieldType.Int, masterPage.ThumbnailImageResizeMode ?? (object)DBNull.Value);
            dbm.AddParameter("@PreviewImageWidth", FieldType.Int, masterPage.PreviewImageWidth ?? (object)DBNull.Value);
            dbm.AddParameter("@PreviewImageHeight", FieldType.Int, masterPage.PreviewImageHeight ?? (object)DBNull.Value);
            dbm.AddParameter("@PreviewImageResizeMode", FieldType.Int, masterPage.PreviewImageResizeMode ?? (object)DBNull.Value);
            dbm.AddParameter("@ImageMinWidth", FieldType.Int, masterPage.ImageMinWidth ?? (object)DBNull.Value);
            dbm.AddParameter("@ImageMinHeight", FieldType.Int, masterPage.ImageMinHeight ?? (object)DBNull.Value);
            dbm.AddParameter("@Creatable", FieldType.Bit, masterPage.Creatable);
            dbm.AddParameter("@Deletable", FieldType.Bit, masterPage.Deletable);
            dbm.AddParameter("@Taggable", FieldType.Bit, masterPage.Taggable);
            dbm.AddParameter("@Administration", FieldType.Bit, masterPage.Administration);
            dbm.AddParameter("@BeginRender", FieldType.NVarChar, -1, masterPage.BeginRender ?? (object)DBNull.Value);
            dbm.AddParameter("@EndRender", FieldType.NVarChar, -1, masterPage.EndRender ?? (object)DBNull.Value);
        }

        /// <summary>
        /// Add master page SQL parameters.
        /// </summary>
        /// <param name="masterPage">Master page.</param>
        /// <param name="dbm">The database manager where parameters are added.</param>
        private void AddMasterPageParameters(MasterPage masterPage, IDatabaseManager dbm)
        {
            MasterPageZoneCollection masterPageZoneCollection = new MasterPageZoneCollection();
            MasterPageZoneElementTypeCollection masterPageZoneElementTypeCollection = new MasterPageZoneElementTypeCollection();
            MasterPageZoneElementCollection masterPageZoneElementCollection = new MasterPageZoneElementCollection();
            PopulateMasterPageCollections(masterPage, masterPageZoneCollection, masterPageZoneElementTypeCollection, masterPageZoneElementCollection);
            AddMasterPageDetailsParameters(masterPage, dbm);
            dbm.AddTypedParameter("@MasterPageZones", FieldType.Structured, masterPageZoneCollection.Count == 0 ? null : masterPageZoneCollection, "cms.MasterPageZoneTableType");
            dbm.AddTypedParameter("@MasterPageZoneElementTypes", FieldType.Structured, masterPageZoneElementTypeCollection.Count == 0 ? null : masterPageZoneElementTypeCollection, "cms.MasterPageZoneElementTypeTableType");
            dbm.AddTypedParameter("@MasterPageZoneElements", FieldType.Structured, masterPageZoneElementCollection.Count == 0 ? null : masterPageZoneElementCollection, "cms.MasterPageZoneElementTableType");
        }

        /// <summary>
        /// Adds master page zone SQL parameters.
        /// </summary>
        /// <param name="masterPageZone">Master page zone.</param>
        /// <param name="dbm">The database manager where parameters are added.</param>
        private void AddMasterPageZoneParameters(MasterPageZone masterPageZone, IDatabaseManager dbm)
        {
            MasterPageZoneElementTypeCollection masterPageZoneElementTypeCollection = new MasterPageZoneElementTypeCollection();
            MasterPageZoneElementCollection masterPageZoneElementCollection = new MasterPageZoneElementCollection();
            PopulateMasterPageZoneCollections(masterPageZone, masterPageZoneElementTypeCollection, masterPageZoneElementCollection);
            dbm.AddParameter("@Name", FieldType.NVarChar, MasterPageLengths.ZoneNameMaxLength, masterPageZone.Name);
            dbm.AddParameter("@SortOrder", FieldType.Int, masterPageZone.SortOrder);
            dbm.AddParameter("@AdminType", FieldType.Int, (int)masterPageZone.AdminType);
            dbm.AddParameter("@ContentType", FieldType.Int, (int)masterPageZone.ContentType);
            dbm.AddParameter("@BeginRender", FieldType.NVarChar, -1, masterPageZone.BeginRender ?? (object)DBNull.Value);
            dbm.AddParameter("@EndRender", FieldType.NVarChar, -1, masterPageZone.EndRender ?? (object)DBNull.Value);
            dbm.AddTypedParameter("@MasterPageZoneElementTypes", FieldType.Structured, masterPageZoneElementTypeCollection.Count == 0 ? null : masterPageZoneElementTypeCollection, "cms.MasterPageZoneElementTypeTableType");
            dbm.AddTypedParameter("@MasterPageZoneElements", FieldType.Structured, masterPageZoneElementCollection.Count == 0 ? null : masterPageZoneElementCollection, "cms.MasterPageZoneElementTableType");
        }

        /// <summary>
        /// Creates a new master page.
        /// </summary>
        /// <param name="masterPage">New master page details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Newly allocated master page identifier.</returns>
        public long Create(MasterPage masterPage, IUnitOfWork unitOfWork = null)
        {
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                dbm.SetSQL(_sqlManager.GetSql("Sql.CreateMasterPage.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, masterPage.TenantId);
                AddMasterPageParameters(masterPage, dbm);
                dbm.AddOutputParameter("@MasterPageId", FieldType.BigInt);
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                dbm.ExecuteNonQuery(outputValues);
                if (localUnitOfWork != null)
                    localUnitOfWork.Commit();
                return (long)outputValues["@MasterPageId"];
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
        /// Gets master page details.
        /// </summary>
        /// <param name="tenantId">Identifies website that master page belongs to.</param>
        /// <param name="masterPageId">Identifies the master page to return.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Master page details (or null if master page not found).</returns>
        public MasterPage Read(long tenantId, long masterPageId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.ReadMasterPage.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@MasterPageId", FieldType.BigInt, masterPageId);
                dbm.ExecuteReader();
                return GetMasterPage(dbm, true);
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Gets master page zone details.
        /// </summary>
        /// <param name="tenantId">Identifies website that master pages belong to.</param>
        /// <param name="masterPageId">Identifies the master page whose zone is returned.</param>
        /// <param name="masterPageZoneId">Identifies the master page zone to return.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Master page zone details (or null if master page zone not found).</returns>
        public MasterPageZone ReadZone(long tenantId, long masterPageId, long masterPageZoneId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.ReadMasterPageZone.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@MasterPageId", FieldType.BigInt, masterPageId);
                dbm.AddParameter("@MasterPageZoneId", FieldType.BigInt, masterPageZoneId);
                dbm.ExecuteReader();
                if (!dbm.Read())
                    return null;
                MasterPageZone masterPageZone = GetMasterPageZoneFromDatabaseManager(dbm);
                dbm.Read();
                while (dbm.Read())
                    masterPageZone.MasterPageZoneElementTypes.Add(GetMasterPageZoneElementTypeFromDatabaseManager(dbm));
                while (dbm.Read())
                    masterPageZone.MasterPageZoneElements.Add(GetMasterPageZoneElementFromDatabaseManager(dbm));
                return masterPageZone;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Gets administration master page identifier.
        /// </summary>
        /// <param name="tenantId">Identifies website that admin master page belongs to.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Admin master page identifier (or null if admin master page not found).</returns>
        private long? GetAdministrationMasterPageId(long tenantId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                long? masterPageId = null;
                dbm.SetStoredProcedure("cms.GetAdministrationMasterPageId");
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.ExecuteReader();
                if (dbm.Read())
                    masterPageId = (long)dbm.DataReaderValue("MasterPageId");
                return masterPageId;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Gets admin master page details.
        /// </summary>
        /// <param name="tenantId">Identifies website that master pages belong to.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Master page details (or null if master page not found).</returns>
        public MasterPage ReadAdministration(long tenantId, IUnitOfWork unitOfWork = null)
        {
            MasterPage masterPage = null;
            long? masterPageId = GetAdministrationMasterPageId(tenantId, unitOfWork);
            if (masterPageId.HasValue)
                masterPage = Read(tenantId, masterPageId.Value, unitOfWork);
            return masterPage;
        }

        /// <summary>
        /// Retrieves count of the number of times an element occurs on a master page.
        /// </summary>
        /// <param name="tenantId">Tenant identifies website whose master pages are searched.</param>
        /// <param name="elementId">The element whose count is returned.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Element count.</returns>
        public int GetElementMasterPageCount(long tenantId, long elementId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetStoredProcedure("cms.GetElementMasterPageCount");
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, elementId);
                dbm.ExecuteReader();
                dbm.Read();
                int masterPageCount = (int)dbm.DataReaderValue("ElementMasterPageCount");
                dbm.Read();
                return masterPageCount;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Updates existing master page.
        /// </summary>
        /// <param name="masterPage">Updated master page details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Update(MasterPage masterPage, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.UpdateMasterPage.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, masterPage.TenantId);
                dbm.AddParameter("@MasterPageId", FieldType.BigInt, masterPage.MasterPageId);
                AddMasterPageDetailsParameters(masterPage, dbm);
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Updates existing master page zone.
        /// </summary>
        /// <param name="masterPageZone">Updated master page zone details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void UpdateZone(MasterPageZone masterPageZone, IUnitOfWork unitOfWork = null)
        {
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                dbm.SetSQL(_sqlManager.GetSql("Sql.UpdateMasterPageZone.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, masterPageZone.TenantId);
                dbm.AddParameter("@MasterPageId", FieldType.BigInt, masterPageZone.MasterPageId);
                dbm.AddParameter("@MasterPageZoneId", FieldType.BigInt, masterPageZone.MasterPageZoneId);
                AddMasterPageZoneParameters(masterPageZone, dbm);
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
        /// Adds, removes and re-orders master page zones.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="masterPageId">Identifier of master page whose zones are added, removed or re-ordered.</param>
        /// <param name="masterPageZones">Updated list of master page zones.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void UpdateZones(long tenantId, long masterPageId, List<MasterPageZone> masterPageZones, IUnitOfWork unitOfWork = null)
        {
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                MasterPageZoneCollection masterPageZoneCollection = new MasterPageZoneCollection();
                foreach (MasterPageZone zone in masterPageZones)
                    masterPageZoneCollection.Add(zone);
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                dbm.SetSQL(_sqlManager.GetSql("Sql.UpdateMasterPageZones.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@MasterPageId", FieldType.BigInt, masterPageId);
                dbm.AddTypedParameter("@MasterPageZones", FieldType.Structured, masterPageZoneCollection.Count == 0 ? null : masterPageZoneCollection, "cms.MasterPageZoneTableType");
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
        /// Set specified master page as admin and ensure all other master pages belonging to the same tenant do not have master page set as admin.
        /// There can only be 1 admin master page.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="masterPageId">Identifier of master page to be made the admin master page.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void SetAdminMasterPage(long tenantId, long masterPageId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.SetAdminMasterPage.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@MasterPageId", FieldType.BigInt, masterPageId);
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Retrieves distinct list of element identifiers that are associated with the supplied list of master page zone element identifiers.
        /// </summary>
        /// <param name="tenantId">Website tenant identifier.</param>
        /// <param name="masterPageId">Master page identifier.</param>
        /// <param name="masterPageZoneId">Master page zone identifier.</param>
        /// <param name="masterPageZoneElementIds">List of master page zone element identifiers, whose associated elements are returned.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Distinct list of element identifiers associated with supplied list of master page zone element identifiers.</returns>
        public List<long> ListPageElementsByMasterPageZoneElementIds(long tenantId, long masterPageId, long masterPageZoneId, List<long> masterPageZoneElementIds, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                MasterPageZoneElementCollection masterPageZoneElementCollection = new MasterPageZoneElementCollection();
                foreach (long masterPageZoneElementId in masterPageZoneElementIds)
                    masterPageZoneElementCollection.Add(new MasterPageZoneElement { MasterPageZoneElementId = masterPageZoneElementId });
                dbm.SetSQL(_sqlManager.GetSql("Sql.ListPageElementsByMasterPageZoneElementIds.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@MasterPageId", FieldType.BigInt, masterPageId);
                dbm.AddParameter("@MasterPageZoneId", FieldType.BigInt, masterPageZoneId);
                dbm.AddTypedParameter("@MasterPageZoneElements", FieldType.Structured, masterPageZoneElementCollection.Count == 0 ? null : masterPageZoneElementCollection, "cms.MasterPageZoneElementTableType");
                dbm.ExecuteReader();
                List<long> elementIds = new List<long>();
                while (dbm.Read())
                    elementIds.Add((long)dbm.DataReaderValue("ElementId"));
                return elementIds;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Retrieves distinct list of element identifiers that are associated with a master page zone.
        /// </summary>
        /// <param name="tenantId">Website tenant identifier.</param>
        /// <param name="masterPageId">Master page identifier.</param>
        /// <param name="masterPageZoneId">Master page zone identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Distinct list of element identifiers associated with master page zone.</returns>
        public List<long> ListPageElementsByMasterPageZone(long tenantId, long masterPageId, long masterPageZoneId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.ListPageElementsByMasterPageZone.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@MasterPageId", FieldType.BigInt, masterPageId);
                dbm.AddParameter("@MasterPageZoneId", FieldType.BigInt, masterPageZoneId);
                dbm.ExecuteReader();
                List<long> elementIds = new List<long>();
                while (dbm.Read())
                    elementIds.Add((long)dbm.DataReaderValue("ElementId"));
                return elementIds;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Deletes page zone elements that are associated with list of master page zone element identifiers.
        /// </summary>
        /// <param name="tenantId">Website tenant identifier.</param>
        /// <param name="masterPageId">Master page identifier.</param>
        /// <param name="masterPageZoneId">Master page zone identifier.</param>
        /// <param name="masterPageZoneElementIds">List of master page zone element identifiers, identifying page zone elements to delete.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void DeletePageZoneElementsByMasterPageZoneElementIds(long tenantId, long masterPageId, long masterPageZoneId, List<long> masterPageZoneElementIds, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                MasterPageZoneElementCollection masterPageZoneElementCollection = new MasterPageZoneElementCollection();
                foreach (long masterPageZoneElementId in masterPageZoneElementIds)
                    masterPageZoneElementCollection.Add(new MasterPageZoneElement { MasterPageZoneElementId = masterPageZoneElementId });
                dbm.SetSQL(_sqlManager.GetSql("Sql.DeletePageZoneElementsByMasterPageZoneElementIds.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@MasterPageId", FieldType.BigInt, masterPageId);
                dbm.AddParameter("@MasterPageZoneId", FieldType.BigInt, masterPageZoneId);
                dbm.AddTypedParameter("@MasterPageZoneElements", FieldType.Structured, masterPageZoneElementCollection.Count == 0 ? null : masterPageZoneElementCollection, "cms.MasterPageZoneElementTableType");
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Deletes page zone elements associated with a master page zone.
        /// </summary>
        /// <param name="tenantId">Website tenant identifier.</param>
        /// <param name="masterPageId">Master page identifier.</param>
        /// <param name="masterPageZoneId">Master page zone identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void DeletePageZoneElementsByMasterPageZone(long tenantId, long masterPageId, long masterPageZoneId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.DeletePageZoneElementsByMasterPageZone.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@MasterPageId", FieldType.BigInt, masterPageId);
                dbm.AddParameter("@MasterPageZoneId", FieldType.BigInt, masterPageZoneId);
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Deletes page zones associated with a master page zone.
        /// </summary>
        /// <param name="tenantId">Website tenant identifier.</param>
        /// <param name="masterPageId">Master page identifier.</param>
        /// <param name="masterPageZoneId">Master page zone identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void DeletePageZonesByMasterPageZone(long tenantId, long masterPageId, long masterPageZoneId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.DeletePageZonesByMasterPageZone.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@MasterPageId", FieldType.BigInt, masterPageId);
                dbm.AddParameter("@MasterPageZoneId", FieldType.BigInt, masterPageZoneId);
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Get elements for master pages belonging to the specified tenant.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Collection of elements.</returns>
        public IEnumerable<MasterPage> ListElementsByMasterPage(long tenantId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                List<MasterPage> masterPages = new List<MasterPage>();
                dbm.SetSQL(_sqlManager.GetSql("Sql.ListMasterPageElements.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.ExecuteReader();
                MasterPage masterPage = null;
                MasterPageZone masterPageZone = null;
                MasterPageZoneElement masterPageZoneElement = null;
                while (dbm.Read())
                {
                    // Get master page details, create new master page each time master page identifier changes 
                    long masterPageId = (long)dbm.DataReaderValue("MasterPageId");
                    if (masterPage == null || masterPage.MasterPageId != masterPageId)
                    {
                        masterPage = new MasterPage { TenantId = tenantId, MasterPageId = masterPageId, Name = (string)dbm.DataReaderValue("MasterPageName"),  MasterPageZones = new List<MasterPageZone>() };
                        masterPages.Add(masterPage);
                        masterPageZone = null;
                        masterPageZoneElement = null;
                    }

                    // Get master page zone details, create new master page zone each time master page zone identifier changes
                    long masterPageZoneId = (long)dbm.DataReaderValue("MasterPageZoneId");
                    if (masterPageZone == null || masterPageZone.MasterPageZoneId != masterPageZoneId)
                    {
                        masterPageZone = new MasterPageZone { TenantId = tenantId, MasterPageId = masterPageId, MasterPageZoneId = masterPageZoneId, Name = (string)dbm.DataReaderValue("MasterPageZoneName"), MasterPageZoneElements = new List<MasterPageZoneElement>() };
                        masterPage.MasterPageZones.Add(masterPageZone);
                        masterPageZoneElement = null;
                    }

                    // Get master page zone element details, create new master page zone element each time master page zone element identifier changes
                    long masterPageZoneElementId = (long)dbm.DataReaderValue("MasterPageZoneElementId");
                    if (masterPageZoneElement == null || masterPageZoneElement.MasterPageZoneElementId != masterPageZoneElementId)
                    {
                        masterPageZoneElement = new MasterPageZoneElement { TenantId = tenantId, MasterPageId = masterPageId, MasterPageZoneId = masterPageZoneId, MasterPageZoneElementId = masterPageZoneElementId };
                        masterPageZone.MasterPageZoneElements.Add(masterPageZoneElement);
                    }

                    // Get element details
                    masterPageZoneElement.Element = new ElementSettings { TenantId = tenantId, ElementId = (long)dbm.DataReaderValue("ElementId"), ElementTypeId = (Guid)dbm.DataReaderValue("ElementTypeId"), Name = (string)dbm.DataReaderValue("Name") };
                    masterPageZoneElement.ElementId = masterPageZoneElement.Element.ElementId;
                }
                return masterPages;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }
    }
}
