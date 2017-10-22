using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Elements
{
    public class SqlElementRepository : IElementRepository
    {
        private IDatabaseManagerFactory _databaseManagerFactory;
        private ISqlManager _sqlManager;

        public SqlElementRepository(IDatabaseManagerFactory databaseManagerFactory, ISqlManager sqlManager)
        {
            _databaseManagerFactory = databaseManagerFactory;
            _sqlManager = sqlManager;
        }

        public long Create(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetStoredProcedure("cms.CreateElement");
                dbm.AddParameter("@TenantId",      FieldType.BigInt,           settings.TenantId);
                dbm.AddParameter("@ElementTypeId", FieldType.UniqueIdentifier, settings.ElementTypeId);
                dbm.AddParameter("@Name",          FieldType.NVarChar, 50,     settings.Name);
                dbm.AddOutputParameter("@ElementId", FieldType.BigInt);
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                dbm.ExecuteNonQuery(outputValues);
                return (long)outputValues["@ElementId"];
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public long Copy(long sourceTenantId, long sourceElementId, long destTenantId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetStoredProcedure("cms.CopyElement");
                dbm.AddParameter("@SourceTenantId", FieldType.BigInt, sourceTenantId);
                dbm.AddParameter("@SourceElementId", FieldType.BigInt, sourceElementId);
                dbm.AddParameter("@DestTenantId", FieldType.BigInt, destTenantId);
                dbm.AddOutputParameter("@DestElementId", FieldType.BigInt);
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                dbm.ExecuteNonQuery(outputValues);
                return (long)outputValues["@DestElementId"];
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public void Read(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetStoredProcedure("cms.ReadElement");
                dbm.AddParameter("@TenantId",  FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.ExecuteReader();
                dbm.Read();
                settings.Name = (string)dbm.DataReaderValue("Name");
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        private IElementSettings GetElementSettingsFromDatabaseManager(IDatabaseManager dbm)
        {
            return new ElementSettings {
                TenantId      = (long)dbm.DataReaderValue("TenantId"),
                ElementId     = (long)dbm.DataReaderValue("ElementId"),
                ElementTypeId = (Guid)dbm.DataReaderValue("ElementTypeId"),
                Name          = (string)dbm.DataReaderValue("Name")
            };
        }

        public IElementSettings Read(long tenantId, long elementId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                IElementSettings elementSettings = null;
                dbm.SetStoredProcedure("cms.ReadElement");
                dbm.AddParameter("@TenantId",  FieldType.BigInt, tenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, elementId);
                dbm.ExecuteReader();
                if (dbm.Read())
                    elementSettings = GetElementSettingsFromDatabaseManager(dbm);
                return elementSettings;
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
                dbm.SetStoredProcedure("cms.DeleteElement");
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

        public IEnumerable<ElementType> ListTypes(IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                List<ElementType> elementTypes = new List<ElementType>();
                dbm.SetStoredProcedure("cms.ListElementTypes");
                dbm.ExecuteReader();
                while (dbm.Read())
                    elementTypes.Add(new ElementType { Name = (string)dbm.DataReaderValue("Name"), ElementTypeId = (Guid)dbm.DataReaderValue("ElementTypeId") });
                return elementTypes;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public IEnumerable<IElementSettings> ListElements(long tenantId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                List<IElementSettings> elements = new List<IElementSettings>();
                dbm.SetSQL(_sqlManager.GetSql("Sql.ListElements.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.ExecuteReader();
                while (dbm.Read())
                    elements.Add(GetElementSettingsFromDatabaseManager(dbm));
                return elements;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }
    }
}
