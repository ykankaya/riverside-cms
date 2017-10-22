using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.Forms
{
    public class SqlFormRepository : IFormRepository
    {
        private IDatabaseManagerFactory _databaseManagerFactory;
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private ISqlManager _sqlManager;

        public SqlFormRepository(IDatabaseManagerFactory databaseManagerFactory, IUnitOfWorkFactory unitOfWorkFactory, ISqlManager sqlManager)
        {
            _databaseManagerFactory = databaseManagerFactory;
            _unitOfWorkFactory = unitOfWorkFactory;
            _sqlManager = sqlManager;
        }

        private FormFieldCollection GetFormFieldCollection(FormSettings settings)
        {
            FormFieldCollection formFieldCollection = new FormFieldCollection();
            foreach (FormElementField field in settings.Fields)
                formFieldCollection.Add(field);
            return formFieldCollection;
        }

        public void Create(FormSettings settings, IUnitOfWork unitOfWork = null)
        {
            FormFieldCollection formFieldCollection = GetFormFieldCollection(settings);
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                dbm.SetSQL(_sqlManager.GetSql("Sql.CreateForm.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@RecipientEmail", FieldType.NVarChar, -1, settings.RecipientEmail);
                dbm.AddParameter("@SubmitButtonLabel", FieldType.NVarChar, 100, settings.SubmitButtonLabel);
                dbm.AddParameter("@SubmittedMessage", FieldType.NVarChar, -1, settings.SubmittedMessage);
                dbm.AddTypedParameter("@FormFields", FieldType.Structured, formFieldCollection.Count == 0 ? null : formFieldCollection, "element.FormFieldTableType");
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
                dbm.SetSQL(_sqlManager.GetSql("Sql.CopyForm.sql"));
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

        public void Read(FormSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.ReadForm.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.ExecuteReader();
                dbm.Read();
                settings.RecipientEmail = (string)dbm.DataReaderValue("RecipientEmail");
                settings.SubmitButtonLabel = (string)dbm.DataReaderValue("SubmitButtonLabel");
                settings.SubmittedMessage = (string)dbm.DataReaderValue("SubmittedMessage");
                settings.Fields = new List<FormElementField>();
                dbm.Read();
                while (dbm.Read())
                {
                    settings.Fields.Add(new FormElementField
                    {
                        TenantId = (long)dbm.DataReaderValue("TenantId"),
                        ElementId = (long)dbm.DataReaderValue("ElementId"),
                        FormFieldId = (long)dbm.DataReaderValue("FormFieldId"),
                        SortOrder = (int)dbm.DataReaderValue("SortOrder"),
                        FieldType = (FormElementFieldType)(int)dbm.DataReaderValue("FormFieldType"),
                        Label = (string)dbm.DataReaderValue("Label"),
                        Required = (bool)dbm.DataReaderValue("Required")
                    });
                }
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public void Update(FormSettings settings, IUnitOfWork unitOfWork = null)
        {
            FormFieldCollection formFieldCollection = GetFormFieldCollection(settings);
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                dbm.SetSQL(_sqlManager.GetSql("Sql.UpdateForm.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@RecipientEmail", FieldType.NVarChar, -1, settings.RecipientEmail);
                dbm.AddParameter("@SubmitButtonLabel", FieldType.NVarChar, 100, settings.SubmitButtonLabel);
                dbm.AddParameter("@SubmittedMessage", FieldType.NVarChar, -1, settings.SubmittedMessage);
                dbm.AddTypedParameter("@FormFields", FieldType.Structured, formFieldCollection.Count == 0 ? null : formFieldCollection, "element.FormFieldTableType");
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
                dbm.SetSQL(_sqlManager.GetSql("Sql.DeleteForm.sql"));
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
