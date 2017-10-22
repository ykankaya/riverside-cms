using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Domains;
using Riverside.Cms.Core.Webs;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Domains
{
    public class SqlDomainRepository : IDomainRepository
    {
        private IDatabaseManagerFactory _databaseManagerFactory;

        public SqlDomainRepository(IDatabaseManagerFactory databaseManagerFactory)
        {
            _databaseManagerFactory = databaseManagerFactory;
        }

        private Domain GetDomainFromDatabaseManager(IDatabaseManager dbm)
        {
            Domain domain = new Domain {
                TenantId    = (long)dbm.DataReaderValue("TenantId"),
                DomainId    = (long)dbm.DataReaderValue("DomainId"),
                Url         = (string)dbm.DataReaderValue("Url"),
                RedirectUrl = dbm.DataReaderValue("RedirectUrl") == DBNull.Value ? null : (string)dbm.DataReaderValue("RedirectUrl")
            };
            domain.Web = new Web { TenantId = domain.TenantId, Name = (string)dbm.DataReaderValue("Name") };
            return domain;
        }

        public ISearchResult<Domain> Search(long tenantId, ISearchParameters parameters, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                List<Domain> domains = new List<Domain>();
                dbm.SetStoredProcedure("cms.SearchDomains");
                dbm.AddParameter("@TenantId",  FieldType.BigInt, tenantId);
                dbm.AddParameter("@PageIndex", FieldType.Int, parameters.PageIndex);
                dbm.AddParameter("@PageSize",  FieldType.Int, parameters.PageSize);
                dbm.AddParameter("@Search",    FieldType.NVarChar, 50, parameters.Search);
                dbm.ExecuteReader();
                while (dbm.Read())
                    domains.Add(GetDomainFromDatabaseManager(dbm));
                dbm.Read();
                int total = (int)dbm.DataReaderValue("Total");
                return new SearchResult<Domain> { Items = domains, Total = total };
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public long Create(Domain domain, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetStoredProcedure("cms.CreateDomain");
                dbm.AddParameter("@TenantId", FieldType.BigInt, domain.TenantId);
                dbm.AddParameter("@Url", FieldType.NVarChar, 256, domain.Url);
                dbm.AddParameter("@RedirectUrl", FieldType.NVarChar, 256, domain.RedirectUrl ?? (object)DBNull.Value);
                dbm.AddOutputParameter("@DomainId", FieldType.BigInt);
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                dbm.ExecuteNonQuery(outputValues);
                return (long)outputValues["@DomainId"];
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public Domain Read(long tenantId, long domainId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                Domain domain = null;
                dbm.SetStoredProcedure("cms.ReadDomain");
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@DomainId", FieldType.BigInt, domainId);
                dbm.ExecuteReader();
                if (dbm.Read())
                    domain = GetDomainFromDatabaseManager(dbm);
                return domain;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public Domain ReadByUrl(string url, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                Domain domain = null;
                dbm.SetStoredProcedure("cms.ReadDomainByUrl");
                dbm.AddParameter("@Url", FieldType.NVarChar, 256, url);
                dbm.ExecuteReader();
                if (dbm.Read())
                    domain = GetDomainFromDatabaseManager(dbm);
                return domain;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public void Update(Domain domain, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetStoredProcedure("cms.UpdateDomain");
                dbm.AddParameter("@TenantId", FieldType.BigInt, domain.TenantId);
                dbm.AddParameter("@DomainId", FieldType.BigInt, domain.DomainId);
                dbm.AddParameter("@Url", FieldType.NVarChar, 256, domain.Url);
                dbm.AddParameter("@RedirectUrl", FieldType.NVarChar, 256, domain.RedirectUrl ?? (object)DBNull.Value);
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public void Delete(long tenantId, long domainId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetStoredProcedure("cms.DeleteDomain");
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@DomainId", FieldType.BigInt, domainId);
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public void DeleteByTenant(long tenantId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetStoredProcedure("cms.DeleteDomainsByTenant");
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
