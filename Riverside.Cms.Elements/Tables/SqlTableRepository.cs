using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.Tables
{
    /// <summary>
    /// Repository for table elements.
    /// </summary>
    public class SqlTableRepository : ITableRepository
    {
        // Member variables
        private IDatabaseManagerFactory _databaseManagerFactory;
        private ISqlManager _sqlManager;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="databaseManagerFactory">Database connectivity.</param>
        /// <param name="sqlManager">SQL resource file retrieval.</param>
        public SqlTableRepository(IDatabaseManagerFactory databaseManagerFactory, ISqlManager sqlManager)
        {
            _databaseManagerFactory = databaseManagerFactory;
            _sqlManager = sqlManager;
        }

        /// <summary>
        /// Creates a new table element.
        /// </summary>
        /// <param name="settings">Table settings.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Create(TableSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.CreateTable.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@DisplayName", FieldType.NVarChar, TableLengths.DisplayNameMaxLength, settings.DisplayName);
                dbm.AddParameter("@Preamble", FieldType.NVarChar, -1, settings.Preamble);
                dbm.AddParameter("@ShowHeaders", FieldType.Bit, settings.ShowHeaders);
                dbm.AddParameter("@Rows", FieldType.NVarChar, -1, settings.Rows);
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Copies a table element.
        /// </summary>
        /// <param name="sourceTenantId">Source tenant identifier.</param>
        /// <param name="sourceElementId">Source element identifier.</param>
        /// <param name="destTenantId">Destination tenant identifier.</param>
        /// <param name="destElementId">Destination element identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.CopyTable.sql"));
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

        /// <summary>
        /// Reads a table element.
        /// </summary>
        /// <param name="settings">Table settings.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Read(TableSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.ReadTable.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.ExecuteReader();
                dbm.Read();
                settings.DisplayName = (string)dbm.DataReaderValue("DisplayName");
                settings.Preamble = (string)dbm.DataReaderValue("Preamble");
                settings.ShowHeaders = (bool)dbm.DataReaderValue("ShowHeaders");
                settings.Rows = (string)dbm.DataReaderValue("Rows");
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Updates a table element.
        /// </summary>
        /// <param name="settings">Table settings.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Update(TableSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.UpdateTable.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@DisplayName", FieldType.NVarChar, TableLengths.DisplayNameMaxLength, settings.DisplayName);
                dbm.AddParameter("@Preamble", FieldType.NVarChar, -1, settings.Preamble);
                dbm.AddParameter("@ShowHeaders", FieldType.Bit, settings.ShowHeaders);
                dbm.AddParameter("@Rows", FieldType.NVarChar, -1, settings.Rows);
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Deletes a table element.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="elementId">Element identifier.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.DeleteTable.sql"));
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
