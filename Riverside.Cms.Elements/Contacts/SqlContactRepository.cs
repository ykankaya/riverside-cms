using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.Contacts
{
    public class SqlContactRepository : IContactRepository
    {
        private IDatabaseManagerFactory _databaseManagerFactory;
        private ISqlManager _sqlManager;

        public SqlContactRepository(IDatabaseManagerFactory databaseManagerFactory, ISqlManager sqlManager)
        {
            _databaseManagerFactory = databaseManagerFactory;
            _sqlManager = sqlManager;
        }

        public void Create(ContactSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.CreateContact.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@DisplayName", FieldType.NVarChar, ContactLengths.DisplayNameMaxLength, settings.DisplayName ?? (object)DBNull.Value);
                dbm.AddParameter("@Preamble", FieldType.NVarChar, -1, settings.Preamble ?? (object)DBNull.Value);
                dbm.AddParameter("@Address", FieldType.NVarChar, ContactLengths.AddressMaxLength, settings.Address ?? (object)DBNull.Value);
                dbm.AddParameter("@Email", FieldType.NVarChar, ContactLengths.EmailMaxLength, settings.Email ?? (object)DBNull.Value);
                dbm.AddParameter("@FacebookUsername", FieldType.NVarChar, ContactLengths.FacebookUsernameMaxLength, settings.FacebookUsername ?? (object)DBNull.Value);
                dbm.AddParameter("@InstagramUsername", FieldType.NVarChar, ContactLengths.InstagramUsernameMaxLength, settings.InstagramUsername ?? (object)DBNull.Value);
                dbm.AddParameter("@LinkedInCompanyUsername", FieldType.NVarChar, ContactLengths.LinkedInCompanyUsernameMaxLength, settings.LinkedInCompanyUsername ?? (object)DBNull.Value);
                dbm.AddParameter("@LinkedInPersonalUsername", FieldType.NVarChar, ContactLengths.LinkedInPersonalUsernameMaxLength, settings.LinkedInPersonalUsername ?? (object)DBNull.Value);
                dbm.AddParameter("@TelephoneNumber1", FieldType.NVarChar, ContactLengths.TelephoneNumber1MaxLength, settings.TelephoneNumber1 ?? (object)DBNull.Value);
                dbm.AddParameter("@TelephoneNumber2", FieldType.NVarChar, ContactLengths.TelephoneNumber2MaxLength, settings.TelephoneNumber2 ?? (object)DBNull.Value);
                dbm.AddParameter("@TwitterUsername", FieldType.NVarChar, ContactLengths.TwitterUsernameMaxLength, settings.TwitterUsername ?? (object)DBNull.Value);
                dbm.AddParameter("@YouTubeChannelId", FieldType.NVarChar, ContactLengths.YouTubeChannelIdMaxLength, settings.YouTubeChannelId ?? (object)DBNull.Value);
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
                dbm.SetSQL(_sqlManager.GetSql("Sql.CopyContact.sql"));
                dbm.AddParameter("@SourceTenantId",  FieldType.BigInt, sourceTenantId);
                dbm.AddParameter("@SourceElementId", FieldType.BigInt, sourceElementId);
                dbm.AddParameter("@DestTenantId",    FieldType.BigInt, destTenantId);
                dbm.AddParameter("@DestElementId",   FieldType.BigInt, destElementId);
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public void Read(ContactSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.ReadContact.sql"));
                dbm.AddParameter("@TenantId",  FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.ExecuteReader();
                dbm.Read();
                settings.DisplayName = dbm.DataReaderValue("DisplayName") == DBNull.Value ? null : (string)dbm.DataReaderValue("DisplayName");
                settings.Preamble = dbm.DataReaderValue("Preamble") == DBNull.Value ? null : (string)dbm.DataReaderValue("Preamble");
                settings.Address = dbm.DataReaderValue("Address") == DBNull.Value ? null : (string)dbm.DataReaderValue("Address");
                settings.Email = dbm.DataReaderValue("Email") == DBNull.Value ? null : (string)dbm.DataReaderValue("Email");
                settings.FacebookUsername = dbm.DataReaderValue("FacebookUsername") == DBNull.Value ? null : (string)dbm.DataReaderValue("FacebookUsername");
                settings.InstagramUsername = dbm.DataReaderValue("InstagramUsername") == DBNull.Value ? null : (string)dbm.DataReaderValue("InstagramUsername");
                settings.LinkedInCompanyUsername = dbm.DataReaderValue("LinkedInCompanyUsername") == DBNull.Value ? null : (string)dbm.DataReaderValue("LinkedInCompanyUsername");
                settings.LinkedInPersonalUsername = dbm.DataReaderValue("LinkedInPersonalUsername") == DBNull.Value ? null : (string)dbm.DataReaderValue("LinkedInPersonalUsername");
                settings.TelephoneNumber1 = dbm.DataReaderValue("TelephoneNumber1") == DBNull.Value ? null : (string)dbm.DataReaderValue("TelephoneNumber1");
                settings.TelephoneNumber2 = dbm.DataReaderValue("TelephoneNumber2") == DBNull.Value ? null : (string)dbm.DataReaderValue("TelephoneNumber2");
                settings.TwitterUsername = dbm.DataReaderValue("TwitterUsername") == DBNull.Value ? null : (string)dbm.DataReaderValue("TwitterUsername");
                settings.YouTubeChannelId = dbm.DataReaderValue("YouTubeChannelId") == DBNull.Value ? null : (string)dbm.DataReaderValue("YouTubeChannelId");
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public void Update(ContactSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.UpdateContact.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@DisplayName", FieldType.NVarChar, ContactLengths.DisplayNameMaxLength, settings.DisplayName ?? (object)DBNull.Value);
                dbm.AddParameter("@Preamble", FieldType.NVarChar, -1, settings.Preamble ?? (object)DBNull.Value);
                dbm.AddParameter("@Address", FieldType.NVarChar, ContactLengths.AddressMaxLength, settings.Address ?? (object)DBNull.Value);
                dbm.AddParameter("@Email", FieldType.NVarChar, ContactLengths.EmailMaxLength, settings.Email ?? (object)DBNull.Value);
                dbm.AddParameter("@FacebookUsername", FieldType.NVarChar, ContactLengths.FacebookUsernameMaxLength, settings.FacebookUsername ?? (object)DBNull.Value);
                dbm.AddParameter("@InstagramUsername", FieldType.NVarChar, ContactLengths.InstagramUsernameMaxLength, settings.InstagramUsername ?? (object)DBNull.Value);
                dbm.AddParameter("@LinkedInCompanyUsername", FieldType.NVarChar, ContactLengths.LinkedInCompanyUsernameMaxLength, settings.LinkedInCompanyUsername ?? (object)DBNull.Value);
                dbm.AddParameter("@LinkedInPersonalUsername", FieldType.NVarChar, ContactLengths.LinkedInPersonalUsernameMaxLength, settings.LinkedInPersonalUsername ?? (object)DBNull.Value);
                dbm.AddParameter("@TelephoneNumber1", FieldType.NVarChar, ContactLengths.TelephoneNumber1MaxLength, settings.TelephoneNumber1 ?? (object)DBNull.Value);
                dbm.AddParameter("@TelephoneNumber2", FieldType.NVarChar, ContactLengths.TelephoneNumber2MaxLength, settings.TelephoneNumber2 ?? (object)DBNull.Value);
                dbm.AddParameter("@TwitterUsername", FieldType.NVarChar, ContactLengths.TwitterUsernameMaxLength, settings.TwitterUsername ?? (object)DBNull.Value);
                dbm.AddParameter("@YouTubeChannelId", FieldType.NVarChar, ContactLengths.YouTubeChannelIdMaxLength, settings.YouTubeChannelId ?? (object)DBNull.Value);
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
                dbm.SetSQL(_sqlManager.GetSql("Sql.DeleteContact.sql"));
                dbm.AddParameter("@TenantId",  FieldType.BigInt, tenantId);
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
