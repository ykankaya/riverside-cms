using Riverside.Utilities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.NavBars
{
    public class SqlNavBarRepository : INavBarRepository
    {
        private IDatabaseManagerFactory _databaseManagerFactory;
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private ISqlManager _sqlManager;

        public SqlNavBarRepository(IDatabaseManagerFactory databaseManagerFactory, IUnitOfWorkFactory unitOfWorkFactory, ISqlManager sqlManager)
        {
            _databaseManagerFactory = databaseManagerFactory;
            _unitOfWorkFactory = unitOfWorkFactory;
            _sqlManager = sqlManager;
        }

        private NavBarTabCollection GetNavBarTabCollection(NavBarSettings settings)
        {
            NavBarTabCollection navBarTabCollection = new NavBarTabCollection();
            foreach (NavBarTab tab in settings.Tabs)
                navBarTabCollection.Add(tab);
            return navBarTabCollection;
        }

        public void Create(NavBarSettings settings, IUnitOfWork unitOfWork = null)
        {
            NavBarTabCollection navBarTabCollection = GetNavBarTabCollection(settings);
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                string sql = _sqlManager.GetSql("Sql.CreateNavBar.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@Name", FieldType.NVarChar, 50, settings.NavBarName ?? (object)DBNull.Value);
                dbm.AddParameter("@ShowLoggedOnUserOptions", FieldType.Bit, settings.ShowLoggedOnUserOptions);
                dbm.AddParameter("@ShowLoggedOffUserOptions", FieldType.Bit, settings.ShowLoggedOffUserOptions);
                dbm.AddTypedParameter("@NavBarTabs", FieldType.Structured, navBarTabCollection.Count == 0 ? null : navBarTabCollection, "element.NavBarTabTableType");
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

        public void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null)
        {
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                string sql = _sqlManager.GetSql("Sql.CopyNavBar.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@SourceTenantId", FieldType.BigInt, sourceTenantId);
                dbm.AddParameter("@SourceElementId", FieldType.BigInt, sourceElementId);
                dbm.AddParameter("@DestTenantId", FieldType.BigInt, destTenantId);
                dbm.AddParameter("@DestElementId", FieldType.BigInt, destElementId);
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

        public void Read(NavBarSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.ReadNavBar.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.ExecuteReader();
                dbm.Read();
                settings.NavBarName = dbm.DataReaderValue("Name") == DBNull.Value ? null : (string)dbm.DataReaderValue("Name");
                settings.ShowLoggedOnUserOptions = (bool)dbm.DataReaderValue("ShowLoggedOnUserOptions");
                settings.ShowLoggedOffUserOptions = (bool)dbm.DataReaderValue("ShowLoggedOffUserOptions");
                settings.Tabs = new List<NavBarTab>();
                dbm.Read();
                while (dbm.Read())
                {
                    settings.Tabs.Add(new NavBarTab
                    {
                        ElementId = (long)dbm.DataReaderValue("ElementId"),
                        NavBarTabId = (long)dbm.DataReaderValue("NavBarTabId"),
                        Name = (string)dbm.DataReaderValue("Name"),
                        SortOrder = (int)dbm.DataReaderValue("SortOrder"),
                        TenantId = (long)dbm.DataReaderValue("TenantId"),
                        PageId = (long)dbm.DataReaderValue("PageId")
                    });
                }
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public void Update(NavBarSettings settings, IUnitOfWork unitOfWork = null)
        {
            NavBarTabCollection navBarTabCollection = GetNavBarTabCollection(settings);
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                string sql = _sqlManager.GetSql("Sql.UpdateNavBar.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@Name", FieldType.NVarChar, 50, settings.NavBarName ?? (object)DBNull.Value);
                dbm.AddParameter("@ShowLoggedOnUserOptions", FieldType.Bit, settings.ShowLoggedOnUserOptions);
                dbm.AddParameter("@ShowLoggedOffUserOptions", FieldType.Bit, settings.ShowLoggedOffUserOptions);
                dbm.AddTypedParameter("@NavBarTabs", FieldType.Structured, navBarTabCollection.Count == 0 ? null : navBarTabCollection, "element.NavBarTabTableType");
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
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                string sql = _sqlManager.GetSql("Sql.DeleteNavBar.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, elementId);
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
    }
}
