using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Pages;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Pages
{
    /// <summary>
    /// SQL implementation of page repository.
    /// </summary>
    public class SqlPageRepository : IPageRepository
    {
        // Member variables
        private IDatabaseManagerFactory _databaseManagerFactory;
        private ISqlManager _sqlManager;
        private IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="databaseManagerFactory">Database manager factory.</param>
        /// <param name="sqlManager">For the retrieval of scripts.</param>
        /// <param name="unitOfWorkFactory">Unit of work factory.</param>
        public SqlPageRepository(IDatabaseManagerFactory databaseManagerFactory, ISqlManager sqlManager, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _databaseManagerFactory = databaseManagerFactory;
            _sqlManager = sqlManager;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Gets page from database manager data reader.
        /// </summary>
        /// <param name="dbm">Database manager.</param>
        /// <returns>A populated page object.</returns>
        private Page GetPageFromDatabaseManager(IDatabaseManager dbm)
        {
            long? parentPageId = dbm.DataReaderValue("ParentPageId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("ParentPageId");
            return new Page
            {
                TenantId = (long)dbm.DataReaderValue("TenantId"),
                PageId = (long)dbm.DataReaderValue("PageId"),
                ParentPageId = parentPageId,
                MasterPageId = (long)dbm.DataReaderValue("MasterPageId"),
                Name = (string)dbm.DataReaderValue("Name"),
                Description = dbm.DataReaderValue("Description") == DBNull.Value ? null : (string)dbm.DataReaderValue("Description"),
                Created = (DateTime)dbm.DataReaderValue("Created"),
                Updated = (DateTime)dbm.DataReaderValue("Updated"),
                Occurred = dbm.DataReaderValue("Occurred") == DBNull.Value ? null : (DateTime?)dbm.DataReaderValue("Occurred"),
                ImageTenantId = dbm.DataReaderValue("ImageTenantId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("ImageTenantId"),
                ThumbnailImageUploadId = dbm.DataReaderValue("ThumbnailImageUploadId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("ThumbnailImageUploadId"),
                PreviewImageUploadId = dbm.DataReaderValue("PreviewImageUploadId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("PreviewImageUploadId"),
                ImageUploadId = dbm.DataReaderValue("ImageUploadId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("ImageUploadId"),
                ChildPages = new List<Page>(),
                PageZones = new List<PageZone>(),
                Tags = new List<Tag>()
            };
        }

        /// <summary>
        /// Gets page zone from database manager data reader.
        /// </summary>
        /// <param name="dbm">Database manager.</param>
        /// <returns>A populated page zone object.</returns>
        private PageZone GetPageZoneFromDatabaseManager(IDatabaseManager dbm)
        {
            PageZone pageZone = new PageZone
            {
                TenantId = (long)dbm.DataReaderValue("TenantId"),
                PageId = (long)dbm.DataReaderValue("PageId"),
                PageZoneId = (long)dbm.DataReaderValue("PageZoneId"),
                MasterPageId = (long)dbm.DataReaderValue("MasterPageId"),
                MasterPageZoneId = (long)dbm.DataReaderValue("MasterPageZoneId"),
                PageZoneElements = new List<PageZoneElement>()
            };
            return pageZone;
        }

        /// <summary>
        /// Gets page zone element from database manager data reader.
        /// </summary>
        /// <param name="dbm">Database manager.</param>
        /// <returns>A populated page zone element object.</returns>
        private PageZoneElement GetPageZoneElementFromDatabaseManager(IDatabaseManager dbm)
        {
            return new PageZoneElement
            {
                TenantId = (long)dbm.DataReaderValue("TenantId"),
                PageId = (long)dbm.DataReaderValue("PageId"),
                PageZoneId = (long)dbm.DataReaderValue("PageZoneId"),
                PageZoneElementId = (long)dbm.DataReaderValue("PageZoneElementId"),
                SortOrder = dbm.DataReaderValue("SortOrder") == DBNull.Value ? null : (int?)dbm.DataReaderValue("SortOrder"),
                ElementTypeId = (Guid)dbm.DataReaderValue("ElementTypeId"),
                ElementId = (long)dbm.DataReaderValue("ElementId"),
                MasterPageId = dbm.DataReaderValue("MasterPageId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("MasterPageId"),
                MasterPageZoneId = dbm.DataReaderValue("MasterPageZoneId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("MasterPageZoneId"),
                MasterPageZoneElementId = dbm.DataReaderValue("MasterPageZoneElementId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("MasterPageZoneElementId")
            };
        }

        /// <summary>
        /// Gets tag from database maanger data reader.
        /// </summary>
        /// <param name="dbm">Database maanger.</param>
        /// <returns>A populated tag object.</returns>
        private Tag GetTagFromDatabaseManager(IDatabaseManager dbm)
        {
            return new Tag
            {
                TenantId = (long)dbm.DataReaderValue("TenantId"),
                TagId = (long)dbm.DataReaderValue("TagId"),
                Name = (string)dbm.DataReaderValue("Name"),
                Created = (DateTime)dbm.DataReaderValue("Created"),
                Updated = (DateTime)dbm.DataReaderValue("Updated")
            };
        }

        /// <summary>
        /// Gets page from database manager result.
        /// </summary>
        /// <param name="dbm">Database manager.</param>
        /// <param name="loadZonesAndElements">Indicates depth of data load.</param>
        /// <param name="loadTags">Indicates whether tags should be loaded.</param>
        /// <returns>Page (or null if page not found).</returns>
        private Page GetPage(IDatabaseManager dbm, bool loadZonesAndElements, bool loadTags)
        {
            // Get layout
            if (loadZonesAndElements && !dbm.Read())
                return null;
            Page page = GetPageFromDatabaseManager(dbm);

            // Move data reader on?
            if (loadZonesAndElements || loadTags)
                dbm.Read();

            // Get page zones?
            if (loadZonesAndElements)
            {
                Dictionary<long, PageZone> pageZonesById = new Dictionary<long, PageZone>();
                while (dbm.Read())
                {
                    PageZone pageZone = GetPageZoneFromDatabaseManager(dbm);
                    page.PageZones.Add(pageZone);
                    pageZonesById.Add(pageZone.PageZoneId, pageZone);
                }

                // Get page zone elements
                while (dbm.Read())
                {
                    PageZoneElement pageZoneElement = GetPageZoneElementFromDatabaseManager(dbm);
                    pageZonesById[pageZoneElement.PageZoneId].PageZoneElements.Add(pageZoneElement);
                }
            }

            // Get tags??
            if (loadTags)
            {
                while (dbm.Read())
                {
                    page.Tags.Add(GetTagFromDatabaseManager(dbm));
                }
            }

            // Return the result
            return page;
        }

        /// <summary>
        /// Searches pages.
        /// </summary>
        /// <param name="tenantId">Identifies website whose pages are searched.</param>
        /// <param name="parameters">Search and paging parameters.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Search result.</returns>
        public ISearchResult<Page> Search(long tenantId, ISearchParameters parameters, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                List<Page> pages = new List<Page>();
                dbm.SetStoredProcedure("cms.SearchPages");
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@PageIndex", FieldType.Int, parameters.PageIndex);
                dbm.AddParameter("@PageSize", FieldType.Int, parameters.PageSize);
                dbm.AddParameter("@Search", FieldType.NVarChar, 50, parameters.Search);
                dbm.ExecuteReader();
                while (dbm.Read())
                    pages.Add(GetPage(dbm, false, false));
                dbm.Read();
                int total = (int)dbm.DataReaderValue("Total");
                return new SearchResult<Page> { Items = pages, Total = total };
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Retrieves and populates tags for the specified collection of pages.
        /// </summary>
        /// <param name="dbm">Database manager.</param>
        /// <param name="tenantId">Identifies website.</param>
        /// <param name="pages">Collection of pages.</param>
        private void LoadTagsForPages(IDatabaseManager dbm, long tenantId, IEnumerable<Page> pages)
        {
            Dictionary<long, Page> pagesById = new Dictionary<long, Page>();
            PageCollection pageCollection = new PageCollection();
            foreach (Page page in pages)
            {
                pageCollection.Add(page);
                pagesById.Add(page.PageId, page);
            }

            dbm.SetStoredProcedure("cms.LoadTagsForPages");
            dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
            dbm.AddParameter("@Pages", FieldType.Structured, pageCollection.Count == 0 ? null : pageCollection);
            dbm.ExecuteReader();
            Dictionary<long, Tag> tagsById = new Dictionary<long, Tag>();
            while (dbm.Read())
            {
                Tag tag = GetTagFromDatabaseManager(dbm);
                tagsById.Add(tag.TagId, tag);
            }
            while (dbm.Read())
            {
                long tagId = (long)dbm.DataReaderValue("TagId");
                long pageId = (long)dbm.DataReaderValue("PageId");
                if (pagesById.ContainsKey(pageId) && tagsById.ContainsKey(tagId))
                    pagesById[pageId].Tags.Add(tagsById[tagId]);
            }
        }

        /// <summary>
        /// Retrieves list of pages.
        /// </summary>
        /// <param name="tenantId">Identifies website whose pages are listed.</param>
        /// <param name="parameters">Search and paging parameters.</param>
        /// <param name="pageId">Identifies the parent page whose child pages are returned (set NULL to include root folder of website).</param>
        /// <param name="sortBy">The sort order of pages returned.</param>
        /// <param name="sortAsc">True to sort ascending, false to sort descending.</param>
        /// <param name="recursive">Set true to get all child pages, false if only direct descendants are required.</param>
        /// <param name="pageType">The type of page listed (document or folder).</param>
        /// <param name="loadTags">Indicates whether tags should be loaded for retrieved pages.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Search result.</returns>
        public ISearchResult<Page> List(long tenantId, ISearchParameters parameters, long? pageId, PageSortBy sortBy, bool sortAsc, bool recursive, PageType pageType, bool loadTags, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                List<Page> pages = new List<Page>();
                dbm.SetStoredProcedure(recursive ? "cms.ListPagesRecursive" : "cms.ListPages");
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@PageId", FieldType.BigInt, pageId ?? (object)DBNull.Value);
                dbm.AddParameter("@SortBy", FieldType.Int, (int)sortBy);
                dbm.AddParameter("@SortAsc", FieldType.Bit, sortAsc);
                dbm.AddParameter("@PageType", FieldType.Int, (int)pageType);
                dbm.AddParameter("@PageIndex", FieldType.Int, parameters.PageIndex);
                dbm.AddParameter("@PageSize", FieldType.Int, parameters.PageSize);
                dbm.ExecuteReader();
                while (dbm.Read())
                    pages.Add(GetPage(dbm, false, false));
                dbm.Read();
                int total = (int)dbm.DataReaderValue("Total");
                if (loadTags)
                    LoadTagsForPages(dbm, tenantId, pages);
                return new SearchResult<Page> { Items = pages, Total = total };
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Retrieves list of pages filtered by tags.
        /// </summary>
        /// <param name="tenantId">Identifies website whose pages are listed.</param>
        /// <param name="parameters">Search and paging parameters.</param>
        /// <param name="tags">Tags used to filter pages.</param>
        /// <param name="pageId">Identifies the parent page whose child pages are returned (set NULL to include root folder of website).</param>
        /// <param name="sortBy">The sort order of pages returned.</param>
        /// <param name="sortAsc">True to sort ascending, false to sort descending.</param>
        /// <param name="recursive">Set true to get all child pages, false if only direct descendants are required.</param>
        /// <param name="pageType">The type of page listed (document or folder).</param>
        /// <param name="loadTags">Indicates whether tags should be loaded for retrieved pages.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Search result.</returns>
        public ISearchResult<Page> ListTagged(long tenantId, ISearchParameters parameters, IList<Tag> tags, long? pageId, PageSortBy sortBy, bool sortAsc, bool recursive, PageType pageType, bool loadTags, IUnitOfWork unitOfWork = null)
        {
            TagCollection tagCollection = new TagCollection();
            foreach (Tag tag in tags)
                tagCollection.Add(tag);
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                List<Page> pages = new List<Page>();
                dbm.SetStoredProcedure(recursive ? "cms.ListTaggedPagesRecursive" : "cms.ListTaggedPages");
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@PageId", FieldType.BigInt, pageId ?? (object)DBNull.Value);
                dbm.AddParameter("@SortBy", FieldType.Int, (int)sortBy);
                dbm.AddParameter("@SortAsc", FieldType.Bit, sortAsc);
                dbm.AddParameter("@PageType", FieldType.Int, (int)pageType);
                dbm.AddParameter("@PageIndex", FieldType.Int, parameters.PageIndex);
                dbm.AddParameter("@PageSize", FieldType.Int, parameters.PageSize);
                dbm.AddParameter("@Tags", FieldType.Structured, tagCollection.Count == 0 ? null : tagCollection);
                dbm.ExecuteReader();
                while (dbm.Read())
                    pages.Add(GetPage(dbm, false, false));
                dbm.Read();
                int total = (int)dbm.DataReaderValue("Total");
                if (loadTags)
                    LoadTagsForPages(dbm, tenantId, pages);
                return new SearchResult<Page> { Items = pages, Total = total };
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Retrieves a tag count from a database manager data reader.
        /// </summary>
        /// <param name="dbm">Database manager.</param>
        /// <param name="tenantId">Identifies website.</param>
        /// <returns>Populated tag count.</returns>
        private TagCount GetTagCountFromDatabaseManager(IDatabaseManager dbm, long tenantId)
        {
            return new TagCount
            {
                Count = (int)dbm.DataReaderValue("TagCount"),
                TagId = (long)dbm.DataReaderValue("TagId"),
                Name = (string)dbm.DataReaderValue("Name"),
                TenantId = tenantId
            };
        }

        /// <summary>
        /// Lists tags associated with page.
        /// </summary>
        /// <param name="tenantId">Identifies website whose page tags are listed.</param>
        /// <param name="pageId">Identifies the parent page whose child page tags are returned (set NULL to include root folder of website).</param>
        /// <param name="recursive">Set true to get tags of all child pages, false if only tags of direct descendants are required.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>List of tag counts.</returns>
        public List<TagCount> ListTags(long tenantId, long? pageId, bool recursive, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                List<TagCount> tags = new List<TagCount>();
                dbm.SetStoredProcedure(recursive ? "cms.ListPageTagsRecursive" : "cms.ListPageTags");
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@PageId", FieldType.BigInt, pageId ?? (object)DBNull.Value);
                dbm.ExecuteReader();
                while (dbm.Read())
                    tags.Add(GetTagCountFromDatabaseManager(dbm, tenantId));
                return tags;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Lists tags associated with page that are related to an existing list of tags.
        /// </summary>
        /// <param name="tenantId">Identifies website whose page tags are listed.</param>
        /// <param name="pageId">Identifies the parent page whose child page tags are returned (set NULL to include root folder of website).</param>
        /// <param name="tags">Tags must be related to these tags.</param>
        /// <param name="recursive">Set true to get tags of all child pages, false if only tags of direct descendants are required.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>List of tag counts.</returns>
        public List<TagCount> ListTaggedPageTags(long tenantId, long? pageId, IList<Tag> tags, bool recursive, IUnitOfWork unitOfWork = null)
        {
            TagCollection tagCollection = new TagCollection();
            foreach (Tag tag in tags)
                tagCollection.Add(tag);
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                List<TagCount> tagCounts = new List<TagCount>();
                dbm.SetStoredProcedure(recursive ? "cms.ListTaggedPageTagsRecursive" : "cms.ListTaggedPageTags");
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@PageId", FieldType.BigInt, pageId ?? (object)DBNull.Value);
                dbm.AddParameter("@Tags", FieldType.Structured, tagCollection.Count == 0 ? null : tagCollection);
                dbm.ExecuteReader();
                while (dbm.Read())
                    tagCounts.Add(GetTagCountFromDatabaseManager(dbm, tenantId));
                return tagCounts;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Gets tags with specified names. The number of tags returned may not match the number of names specified, if tags do not exist for some names.
        /// </summary>
        /// <param name="tenantId">Identifies website.</param>
        /// <param name="names">List of tag names.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>List of tags, identified by name.</returns>
        public List<Tag> ListNamedTags(long tenantId, List<string> names, IUnitOfWork unitOfWork = null)
        {
            TagCollection tagCollection = new TagCollection();
            foreach (string name in names)
                tagCollection.Add(new Tag { Name = name });
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                List<Tag> tags = new List<Tag>();
                dbm.SetStoredProcedure("cms.ListNamedTags");
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@Tags", FieldType.Structured, tagCollection.Count == 0 ? null : tagCollection);
                dbm.ExecuteReader();
                while (dbm.Read())
                    tags.Add(GetTagFromDatabaseManager(dbm));
                return tags;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// List pages by master page.
        /// </summary>
        /// <param name="tenantId">Identifies website.</param>
        /// <param name="masterPageId">Master page whose pages are returned.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>The pages associated with a master page.</returns>
        public List<Page> ListPagesByMasterPage(long tenantId, long masterPageId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                List<Page> pages = new List<Page>();
                dbm.SetSQL(_sqlManager.GetSql("Sql.ListPagesByMasterPage.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@MasterPageId", FieldType.BigInt, masterPageId);
                dbm.ExecuteReader();
                while (dbm.Read())
                    pages.Add(GetPage(dbm, false, false));
                return pages;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Constructs page zone and page zone element collections for calls to stored procedures that contain user defined table types.
        /// </summary>
        /// <param name="page">Contains page information used to populate collections.</param>
        /// <param name="pageZoneCollection">Page zone collection.</param>
        /// <param name="pageZoneElementCollection">Page zone element collection.</param>
        private void GetPageCollections(Page page, out PageZoneCollection pageZoneCollection, out PageZoneElementCollection pageZoneElementCollection)
        {
            pageZoneCollection = new PageZoneCollection();
            pageZoneElementCollection = new PageZoneElementCollection();
            foreach (PageZone pageZone in page.PageZones)
            {
                pageZoneCollection.Add(pageZone);
                foreach (PageZoneElement pageZoneElement in pageZone.PageZoneElements)
                {
                    pageZoneElementCollection.Add(pageZoneElement);
                }
            }
        }

        /// <summary>
        /// Creates a new page.
        /// </summary>
        /// <param name="page">New page details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Newly allocated page identifier.</returns>
        public long Create(Page page, IUnitOfWork unitOfWork = null)
        {
            PageZoneCollection pageZoneCollection;
            PageZoneElementCollection pageZoneElementCollection;
            GetPageCollections(page, out pageZoneCollection, out pageZoneElementCollection);
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                dbm.SetSQL(_sqlManager.GetSql("Sql.CreatePage.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, page.TenantId);
                dbm.AddParameter("@ParentPageId", FieldType.BigInt, page.ParentPageId ?? (object)DBNull.Value);
                dbm.AddParameter("@MasterPageId", FieldType.BigInt, page.MasterPageId);
                dbm.AddParameter("@Name", FieldType.NVarChar, 256, page.Name);
                dbm.AddParameter("@Description", FieldType.NVarChar, -1, page.Description ?? (object)DBNull.Value);
                dbm.AddParameter("@Created", FieldType.DateTime, page.Created);
                dbm.AddParameter("@Updated", FieldType.DateTime, page.Updated);
                dbm.AddParameter("@Occurred", FieldType.DateTime, page.Occurred ?? (object)DBNull.Value);
                dbm.AddParameter("@ImageTenantId", FieldType.BigInt, page.ImageTenantId ?? (object)DBNull.Value);
                dbm.AddParameter("@ThumbnailImageUploadId", FieldType.BigInt, page.ThumbnailImageUploadId ?? (object)DBNull.Value);
                dbm.AddParameter("@PreviewImageUploadId", FieldType.BigInt, page.PreviewImageUploadId ?? (object)DBNull.Value);
                dbm.AddParameter("@ImageUploadId", FieldType.BigInt, page.ImageUploadId ?? (object)DBNull.Value);
                dbm.AddTypedParameter("@PageZones", FieldType.Structured, pageZoneCollection.Count == 0 ? null : pageZoneCollection, "cms.PageZoneTableType");
                dbm.AddTypedParameter("@PageZoneElements", FieldType.Structured, pageZoneElementCollection.Count == 0 ? null : pageZoneElementCollection, "cms.PageZoneElementTableType");
                dbm.AddOutputParameter("@PageId", FieldType.BigInt);
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                dbm.ExecuteNonQuery(outputValues);
                if (localUnitOfWork != null)
                    localUnitOfWork.Commit();
                return (long)outputValues["@PageId"];
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
        /// Sets page tags.
        /// </summary>
        /// <param name="page">Contains information required to update page tags.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void UpdateTags(Page page, IUnitOfWork unitOfWork = null)
        {
            TagCollection tagCollection = new TagCollection();
            foreach (Tag tag in page.Tags)
                tagCollection.Add(tag);
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                dbm.SetStoredProcedure("cms.UpdatePageTags");
                dbm.AddParameter("@TenantId", FieldType.BigInt, page.TenantId);
                dbm.AddParameter("@PageId", FieldType.BigInt, page.PageId);
                dbm.AddParameter("@Created", FieldType.DateTime, page.Updated);
                dbm.AddParameter("@Tags", FieldType.Structured, tagCollection.Count == 0 ? null : tagCollection);
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
        /// Gets page details.
        /// </summary>
        /// <param name="tenantId">Identifies website that pages belong to.</param>
        /// <param name="pageId">Identifies the page to return.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Page details (or null if page not found).</returns>
        public Page Read(long tenantId, long pageId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.ReadPage.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@PageId", FieldType.BigInt, pageId);
                dbm.ExecuteReader();
                return GetPage(dbm, true, true);
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Gets home page identifier.
        /// </summary>
        /// <param name="tenantId">Identifies website that home page belongs to.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Home page identifier (or null if home page not found).</returns>
        private long? GetHomePageId(long tenantId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                long? pageId = null;
                dbm.SetStoredProcedure("cms.GetHomePageId");
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.ExecuteReader();
                if (dbm.Read())
                    pageId = (long)dbm.DataReaderValue("PageId");
                return pageId;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Gets home page details.
        /// </summary>
        /// <param name="tenantId">Identifies website that home page belongs to.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Home page details (or null if home page not found).</returns>
        public Page ReadHome(long tenantId, IUnitOfWork unitOfWork = null)
        {
            Page page = null;
            long? pageId = GetHomePageId(tenantId, unitOfWork);
            if (pageId.HasValue)
                page = Read(tenantId, pageId.Value, unitOfWork);
            return page;
        }

        /// <summary>
        /// Gets page hierarchy (only primary page information is returned by this call, no zones or elements).
        /// </summary>
        /// <param name="tenantId">Website that page belongs to.</param>
        /// <param name="pageId">The page whose hierarchy is returned.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Page hierarchy, with parent pages populated (or null if page not found).</returns>
        public Page ReadHierarchy(long tenantId, long pageId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                Page root = null;
                dbm.SetStoredProcedure("cms.ReadPageHierarchy");
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@PageId", FieldType.BigInt, pageId);
                dbm.ExecuteReader();
                if (dbm.Read())
                {
                    root = GetPage(dbm, false, false);
                    Page page = root;
                    while (dbm.Read())
                    {
                        Page childPage = GetPage(dbm, false, false);
                        page.ChildPages.Add(childPage);
                        childPage.ParentPage = page;
                        page = childPage;
                    }
                }
                return root;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Updates a page (not including zones and elements).
        /// </summary>
        /// <param name="page">Updated page details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Update(Page page, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetStoredProcedure("cms.UpdatePage");
                dbm.AddParameter("@TenantId", FieldType.BigInt, page.TenantId);
                dbm.AddParameter("@PageId", FieldType.BigInt, page.PageId);
                dbm.AddParameter("@ParentPageId", FieldType.BigInt, page.ParentPageId ?? (object)DBNull.Value);
                dbm.AddParameter("@MasterPageId", FieldType.BigInt, page.MasterPageId);
                dbm.AddParameter("@Name", FieldType.NVarChar, 256, page.Name);
                dbm.AddParameter("@Description", FieldType.NVarChar, -1, page.Description ?? (object)DBNull.Value);
                dbm.AddParameter("@Created", FieldType.DateTime, page.Created);
                dbm.AddParameter("@Updated", FieldType.DateTime, page.Updated);
                dbm.AddParameter("@Occurred", FieldType.DateTime, page.Occurred ?? (object)DBNull.Value);
                dbm.AddParameter("@ImageTenantId", FieldType.BigInt, page.ImageTenantId ?? (object)DBNull.Value);
                dbm.AddParameter("@ThumbnailImageUploadId", FieldType.BigInt, page.ThumbnailImageUploadId ?? (object)DBNull.Value);
                dbm.AddParameter("@PreviewImageUploadId", FieldType.BigInt, page.PreviewImageUploadId ?? (object)DBNull.Value);
                dbm.AddParameter("@ImageUploadId", FieldType.BigInt, page.ImageUploadId ?? (object)DBNull.Value);
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Retrieves count of the number of times an element occurs on a page.
        /// </summary>
        /// <param name="tenantId">Tenant identifies website whose pages are searched.</param>
        /// <param name="elementId">The element whose count is returned.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Element count.</returns>
        public int GetElementPageCount(long tenantId, long elementId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetStoredProcedure("cms.GetElementPageCount");
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, elementId);
                dbm.ExecuteReader();
                dbm.Read();
                int pageCount = (int)dbm.DataReaderValue("ElementPageCount");
                dbm.Read();
                return pageCount;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Updates a zone's elements.
        /// </summary>
        /// <param name="tenantId">The tenant identifying website that page belongs to.</param>
        /// <param name="pageId">The page that zone belongs to.</param>
        /// <param name="pageZoneId">Identifies zone within page that is being updated.</param>
        /// <param name="pageZoneElements">Updated list of page zone elements.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void UpdatePageZoneElements(long tenantId, long pageId, long pageZoneId, List<PageZoneElement> pageZoneElements, IUnitOfWork unitOfWork = null)
        {
            PageZoneElementCollection pageZoneElementCollection = new PageZoneElementCollection();
            foreach (PageZoneElement pageZoneElement in pageZoneElements)
                pageZoneElementCollection.Add(pageZoneElement);
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                dbm.SetSQL(_sqlManager.GetSql("Sql.UpdatePageZoneElements.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@PageId", FieldType.BigInt, pageId);
                dbm.AddParameter("@PageZoneId", FieldType.BigInt, pageZoneId);
                dbm.AddTypedParameter("@PageZoneElements", FieldType.Structured, pageZoneElementCollection.Count == 0 ? null : pageZoneElementCollection, "cms.PageZoneElementTableType");
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
        /// Get all of the page zones associated with a master page zone.
        /// </summary>
        /// <param name="tenantId">Website tenant identifier.</param>
        /// <param name="masterPageId">Master page identifier.</param>
        /// <param name="masterPageZoneId">Master page zone identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>List of page zones associated with the given master page zone.</returns>
        public List<PageZone> ListPageZonesByMasterPageZoneId(long tenantId, long masterPageId, long masterPageZoneId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.ListPageZonesByMasterPageZoneId.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@MasterPageId", FieldType.BigInt, masterPageId);
                dbm.AddParameter("@MasterPageZoneId", FieldType.BigInt, masterPageZoneId);
                dbm.ExecuteReader();
                List<PageZone> pageZones = new List<PageZone>();
                while (dbm.Read())
                    pageZones.Add(GetPageZoneFromDatabaseManager(dbm));
                return pageZones;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Creates multiple page zone elements.
        /// </summary>
        /// <param name="tenantId">Identifies website where page zone elements created.</param>
        /// <param name="pageZoneElements">List of page zone elements to create.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void CreatePageZoneElements(long tenantId, List<PageZoneElement> pageZoneElements, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                PageZoneElementCollection pageZoneElementCollection = new PageZoneElementCollection();
                foreach (PageZoneElement pageZoneElement in pageZoneElements)
                    pageZoneElementCollection.Add(pageZoneElement);
                dbm.SetSQL(_sqlManager.GetSql("Sql.CreatePageZoneElements.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddTypedParameter("@PageZoneElements", FieldType.Structured, pageZoneElementCollection.Count == 0 ? null : pageZoneElementCollection, "cms.PageZoneElementTableType");
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Creates multiple page zones.
        /// </summary>
        /// <param name="tenantId">Identifies website where page zones created.</param>
        /// <param name="pageZones">List of page zones to create.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void CreatePageZones(long tenantId, List<PageZone> pageZones, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                PageZoneCollection pageZoneCollection = new PageZoneCollection();
                foreach (PageZone pageZone in pageZones)
                    pageZoneCollection.Add(pageZone);
                dbm.SetSQL(_sqlManager.GetSql("Sql.CreatePageZones.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddTypedParameter("@PageZones", FieldType.Structured, pageZoneCollection.Count == 0 ? null : pageZoneCollection, "cms.PageZoneTableType");
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
        /// <param name="tenantId">Identifies website whose page zone elements are deleted.</param>
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
        /// <param name="tenantId">Identifies website whose page zone elements are deleted.</param>
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
        /// Converts page zone elements from format that is compatible with editable master page zone to format that is compatible with configurable master page zone.
        /// For page zone elements associated with a given master page zone, sets master page details NULL and updates sort order so NOT NULL.
        /// </summary>
        /// <param name="tenantId">Identifies website whose page zone elements are updated.</param>
        /// <param name="masterPageId">Master page identifier.</param>
        /// <param name="masterPageZoneId">Master page zone identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void UpdatePageZoneElementsForConfigurableAdminType(long tenantId, long masterPageId, long masterPageZoneId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.UpdatePageZoneElementsForConfigurableAdminType.sql"));
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
    }
}