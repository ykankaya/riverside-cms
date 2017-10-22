using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.MasterPages;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Templates;
using Riverside.Utilities.Data;
using Riverside.Utilities.Drawing;

namespace Riverside.Cms.Core.Templates
{
    /// <summary>
    /// SQL implementation of template repository.
    /// </summary>
    public class SqlTemplateRepository : ITemplateRepository
    {
        // Member variables
        private IDatabaseManagerFactory _databaseManagerFactory;
        private ISqlManager _sqlManager;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="databaseManagerFactory">Database manager factory.</param>
        /// <param name="sqlManager">For the retrieval of named SQL scripts.</param>
        public SqlTemplateRepository(IDatabaseManagerFactory databaseManagerFactory, ISqlManager sqlManager)
        {
            _databaseManagerFactory = databaseManagerFactory;
            _sqlManager = sqlManager;
        }

        /// <summary>
        /// Gets template from database manager data reader.
        /// </summary>
        /// <param name="dbm">Database manager.</param>
        /// <returns>A populated template object.</returns>
        private Template GetTemplateFromDatabaseManager(IDatabaseManager dbm)
        {
            return new Template
            {
                TenantId = (long)dbm.DataReaderValue("TenantId"),
                Name = (string)dbm.DataReaderValue("Name"),
                Description = dbm.DataReaderValue("Description") == DBNull.Value ? null : (string)dbm.DataReaderValue("Description"),
                CreateUserEnabled = (bool)dbm.DataReaderValue("CreateUserEnabled"),
                UserHasImage = (bool)dbm.DataReaderValue("UserHasImage"),
                UserThumbnailImageWidth = dbm.DataReaderValue("UserThumbnailImageWidth") == DBNull.Value ? null : (int?)dbm.DataReaderValue("UserThumbnailImageWidth"),
                UserThumbnailImageHeight = dbm.DataReaderValue("UserThumbnailImageHeight") == DBNull.Value ? null : (int?)dbm.DataReaderValue("UserThumbnailImageHeight"),
                UserThumbnailImageResizeMode = dbm.DataReaderValue("UserThumbnailImageResizeMode") == DBNull.Value ? null : (ResizeMode?)(int)dbm.DataReaderValue("UserThumbnailImageResizeMode"),
                UserPreviewImageWidth = dbm.DataReaderValue("UserPreviewImageWidth") == DBNull.Value ? null : (int?)dbm.DataReaderValue("UserPreviewImageWidth"),
                UserPreviewImageHeight = dbm.DataReaderValue("UserPreviewImageHeight") == DBNull.Value ? null : (int?)dbm.DataReaderValue("UserPreviewImageHeight"),
                UserPreviewImageResizeMode = dbm.DataReaderValue("UserPreviewImageResizeMode") == DBNull.Value ? null : (ResizeMode?)(int)dbm.DataReaderValue("UserPreviewImageResizeMode"),
                UserImageMinWidth = dbm.DataReaderValue("UserImageMinWidth") == DBNull.Value ? null : (int?)dbm.DataReaderValue("UserImageMinWidth"),
                UserImageMinHeight = dbm.DataReaderValue("UserImageMinHeight") == DBNull.Value ? null : (int?)dbm.DataReaderValue("UserImageMinHeight")
            };
        }

        /// <summary>
        /// Gets template page from database manager data reader.
        /// </summary>
        /// <param name="dbm">Database manager.</param>
        /// <returns>A populated template page object.</returns>
        private TemplatePage GetTemplatePageFromDatabaseManager(IDatabaseManager dbm)
        {
            TemplatePage templatePage = new TemplatePage
            {
                TenantId = (long)dbm.DataReaderValue("TenantId"),
                TemplatePageId = (long)dbm.DataReaderValue("TemplatePageId"),
                Name = (string)dbm.DataReaderValue("Name"),
                PageName = (string)dbm.DataReaderValue("PageName"),
                PageDescription = dbm.DataReaderValue("PageDescription") == DBNull.Value ? null : (string)dbm.DataReaderValue("PageDescription"),
                PageType = (PageType)(int)dbm.DataReaderValue("PageType"),
                HasOccurred = (bool)dbm.DataReaderValue("HasOccurred"),
                HasImage = (bool)dbm.DataReaderValue("HasImage"),
                ThumbnailImageWidth = dbm.DataReaderValue("ThumbnailImageWidth") == DBNull.Value ? null : (int?)dbm.DataReaderValue("ThumbnailImageWidth"),
                ThumbnailImageHeight = dbm.DataReaderValue("ThumbnailImageHeight") == DBNull.Value ? null : (int?)dbm.DataReaderValue("ThumbnailImageHeight"),
                ThumbnailImageResizeMode = dbm.DataReaderValue("ThumbnailImageResizeMode") == DBNull.Value ? null : (ResizeMode?)(int)dbm.DataReaderValue("ThumbnailImageResizeMode"),
                PreviewImageWidth = dbm.DataReaderValue("PreviewImageWidth") == DBNull.Value ? null : (int?)dbm.DataReaderValue("PreviewImageWidth"),
                PreviewImageHeight = dbm.DataReaderValue("PreviewImageHeight") == DBNull.Value ? null : (int?)dbm.DataReaderValue("PreviewImageHeight"),
                PreviewImageResizeMode = dbm.DataReaderValue("PreviewImageResizeMode") == DBNull.Value ? null : (ResizeMode?)(int)dbm.DataReaderValue("PreviewImageResizeMode"),
                ImageMinWidth = dbm.DataReaderValue("ImageMinWidth") == DBNull.Value ? null : (int?)dbm.DataReaderValue("ImageMinWidth"),
                ImageMinHeight = dbm.DataReaderValue("ImageMinHeight") == DBNull.Value ? null : (int?)dbm.DataReaderValue("ImageMinHeight"),
                AncestorPageLevel = dbm.DataReaderValue("AncestorPageLevel") == DBNull.Value ? null : (PageLevel?)(int)dbm.DataReaderValue("AncestorPageLevel"),
                ParentTemplatePageId = dbm.DataReaderValue("ParentTemplatePageId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("ParentTemplatePageId"),
                ShowOnNavigation = (bool)dbm.DataReaderValue("ShowOnNavigation"),
                Creatable = (bool)dbm.DataReaderValue("Creatable"),
                Deletable = (bool)dbm.DataReaderValue("Deletable"),
                Taggable = (bool)dbm.DataReaderValue("Taggable"),
                Administration = (bool)dbm.DataReaderValue("Administration"),
                BeginRender = dbm.DataReaderValue("BeginRender") == DBNull.Value ? null : (string)dbm.DataReaderValue("BeginRender"),
                EndRender = dbm.DataReaderValue("EndRender") == DBNull.Value ? null : (string)dbm.DataReaderValue("EndRender"),
                ChildTemplatePages = new List<TemplatePage>(),
                TemplatePageZones = new List<TemplatePageZone>()
            };
            return templatePage;
        }

        /// <summary>
        /// Gets template page zone from database manager data reader.
        /// </summary>
        /// <param name="dbm">Database manager.</param>
        /// <returns>A populated template page zone object.</returns>
        private TemplatePageZone GetTemplatePageZoneFromDatabaseManager(IDatabaseManager dbm)
        {
            TemplatePageZone templatePageZone = new TemplatePageZone
            {
                TenantId = (long)dbm.DataReaderValue("TenantId"),
                TemplatePageId = (long)dbm.DataReaderValue("TemplatePageId"),
                TemplatePageZoneId = (long)dbm.DataReaderValue("TemplatePageZoneId"),
                Name = (string)dbm.DataReaderValue("Name"),
                AdminType = (MasterPageZoneAdminType)(int)dbm.DataReaderValue("AdminType"),
                ContentType = (MasterPageZoneContentType)(int)dbm.DataReaderValue("ContentType"),
                BeginRender = dbm.DataReaderValue("BeginRender") == DBNull.Value ? null : (string)dbm.DataReaderValue("BeginRender"),
                EndRender = dbm.DataReaderValue("EndRender") == DBNull.Value ? null : (string)dbm.DataReaderValue("EndRender"),
                SortOrder = (int)dbm.DataReaderValue("SortOrder"),
                TemplatePageZoneElementTypes = new List<TemplatePageZoneElementType>(),
                TemplatePageZoneElements = new List<TemplatePageZoneElement>()
            };
            return templatePageZone;
        }

        /// <summary>
        /// Gets template page zone element type from database manager data reader.
        /// </summary>
        /// <param name="dbm">Database manager.</param>
        /// <returns>A populated template page zone element type object.</returns>
        private TemplatePageZoneElementType GetTemplatePageZoneElementTypeFromDatabaseManager(IDatabaseManager dbm)
        {
            TemplatePageZoneElementType templatePageZoneElementType = new TemplatePageZoneElementType
            {
                TenantId = (long)dbm.DataReaderValue("TenantId"),
                TemplatePageId = (long)dbm.DataReaderValue("TemplatePageId"),
                TemplatePageZoneId = (long)dbm.DataReaderValue("TemplatePageZoneId"),
                ElementTypeId = (Guid)dbm.DataReaderValue("ElementTypeId")
            };
            return templatePageZoneElementType;
        }

        /// <summary>
        /// Gets template page zone element from database manager data reader.
        /// </summary>
        /// <param name="dbm">Database manager.</param>
        /// <returns>A populated template page zone element object.</returns>
        private TemplatePageZoneElement GetTemplatePageZoneElementFromDatabaseManager(IDatabaseManager dbm)
        {
            TemplatePageZoneElement templatePageZoneElement = new TemplatePageZoneElement
            {
                TenantId = (long)dbm.DataReaderValue("TenantId"),
                TemplatePageId = (long)dbm.DataReaderValue("TemplatePageId"),
                TemplatePageZoneId = (long)dbm.DataReaderValue("TemplatePageZoneId"),
                TemplatePageZoneElementId = (long)dbm.DataReaderValue("TemplatePageZoneElementId"),
                SortOrder = (int)dbm.DataReaderValue("SortOrder"),
                ElementTypeId = (Guid)dbm.DataReaderValue("ElementTypeId"),
                ElementId = (long)dbm.DataReaderValue("ElementId"),
                BeginRender = dbm.DataReaderValue("BeginRender") == DBNull.Value ? null : (string)dbm.DataReaderValue("BeginRender"),
                EndRender = dbm.DataReaderValue("EndRender") == DBNull.Value ? null : (string)dbm.DataReaderValue("EndRender")
            };
            return templatePageZoneElement;
        }

        /// <summary>
        /// Turns templatePagesById into a hierarchy of pages.
        /// </summary>
        /// <param name="templatePagesById">Template pages keyed by template page ID.</param>
        /// <returns>Root template page, with child template pages populated.</returns>
        private TemplatePage GetTemplatePageHierarchy(Dictionary<long, TemplatePage> templatePagesById)
        {
            TemplatePage root = null;
            foreach (KeyValuePair<long, TemplatePage> kvp in templatePagesById)
            {
                TemplatePage page = kvp.Value;
                if (!page.ParentTemplatePageId.HasValue)
                    root = page;
                else
                    templatePagesById[page.ParentTemplatePageId.Value].ChildTemplatePages.Add(page);
            }
            return root;
        }

        /// <summary>
        /// Gets template from database manager result.
        /// </summary>
        /// <param name="dbm">Database manager.</param>
        /// <param name="loadAll">Set true to load template template pages, zones, configurable element types and elements. Otherwise only main template details loaded.</param>
        /// <returns>Template (or null if template not found).</returns>
        private Template GetTemplate(IDatabaseManager dbm, bool loadAll)
        {
            // Get template
            Template template = GetTemplateFromDatabaseManager(dbm);

            // If we are not reading entire template we can return result now
            if (!loadAll)
                return template;

            // Get template pages and then construct template page hierarchy
            dbm.Read();
            Dictionary<long, TemplatePage> templatePagesById = new Dictionary<long, TemplatePage>();
            while (dbm.Read())
            {
                TemplatePage templatePage = GetTemplatePageFromDatabaseManager(dbm);
                templatePagesById.Add(templatePage.TemplatePageId, templatePage);
            }
            template.Page = GetTemplatePageHierarchy(templatePagesById);

            // Get template page zones
            Dictionary<long, TemplatePageZone> templatePageZonesById = new Dictionary<long, TemplatePageZone>();
            while (dbm.Read())
            {
                TemplatePageZone templatePageZone = GetTemplatePageZoneFromDatabaseManager(dbm);
                templatePagesById[templatePageZone.TemplatePageId].TemplatePageZones.Add(templatePageZone);
                templatePageZonesById.Add(templatePageZone.TemplatePageZoneId, templatePageZone);
            }

            // Get the element types that can exist in a template page zone when admin type is configurable
            while (dbm.Read())
            {
                TemplatePageZoneElementType templatePageZoneElementType = GetTemplatePageZoneElementTypeFromDatabaseManager(dbm);
                templatePageZonesById[templatePageZoneElementType.TemplatePageZoneId].TemplatePageZoneElementTypes.Add(templatePageZoneElementType);
            }

            // Get template page zone elements
            while (dbm.Read())
            {
                TemplatePageZoneElement templatePageZoneElement = GetTemplatePageZoneElementFromDatabaseManager(dbm);
                templatePageZonesById[templatePageZoneElement.TemplatePageZoneId].TemplatePageZoneElements.Add(templatePageZoneElement);
            }

            // Return the result
            return template;
        }

        /// <summary>
        /// Searches templates.
        /// </summary>
        /// <param name="parameters">Search and paging parameters.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Search result.</returns>
        public ISearchResult<Template> Search(ISearchParameters parameters, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                List<Template> templates = new List<Template>();
                dbm.SetStoredProcedure("cms.SearchTemplates");
                dbm.AddParameter("@PageIndex", FieldType.Int, parameters.PageIndex);
                dbm.AddParameter("@PageSize", FieldType.Int, parameters.PageSize);
                dbm.AddParameter("@Search", FieldType.NVarChar, 50, parameters.Search);
                dbm.ExecuteReader();
                while (dbm.Read())
                    templates.Add(GetTemplate(dbm, false));
                dbm.Read();
                int total = (int)dbm.DataReaderValue("Total");
                return new SearchResult<Template> { Items = templates, Total = total };
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Get template details.
        /// </summary>
        /// <param name="tenantId">Unique template identifier.</param>
        /// <param name="loadAll">Set true to load entire template rather than just top level template details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Template details (or null if template not found).</returns>
        public Template Read(long tenantId, bool loadAll, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.ReadTemplate.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@LoadAll", FieldType.Bit, loadAll);
                dbm.ExecuteReader();
                if (dbm.Read())
                    return GetTemplate(dbm, loadAll);
                return null;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }
    }
}
