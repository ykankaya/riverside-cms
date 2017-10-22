using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Utilities.Data;
using Riverside.Utilities.Security;

namespace Riverside.Cms.Core.Authentication
{
    public class SqlUserRepository : IUserRepository
    {
        private IDatabaseManagerFactory _databaseManagerFactory;
        private ISqlManager _sqlManager;
        private IUnitOfWorkFactory _unitOfWorkFactory;

        public SqlUserRepository(IDatabaseManagerFactory databaseManagerFactory, ISqlManager sqlManager, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _databaseManagerFactory = databaseManagerFactory;
            _sqlManager = sqlManager;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        private Role GetRoleFromDatabaseManager(IDatabaseManager dbm)
        {
            Role role = new Role
            {
                RoleId = (long)dbm.DataReaderValue("RoleId"),
                Name = (string)dbm.DataReaderValue("Name")
            };
            return role;
        }

        private User GetUserFromDatabaseManager(IDatabaseManager dbm)
        {
            User user = new User
            {
                TenantId = (long)dbm.DataReaderValue("TenantId"),
                UserId = (long)dbm.DataReaderValue("UserId"),
                Alias = (string)dbm.DataReaderValue("Alias"),
                Email = (string)dbm.DataReaderValue("Email"),
                Confirmed = (bool)dbm.DataReaderValue("Confirmed"),
                Enabled = (bool)dbm.DataReaderValue("Enabled"),
                LockedOut = (bool)dbm.DataReaderValue("LockedOut"),
                PasswordSaltedHash = dbm.DataReaderValue("PasswordSaltedHash") == DBNull.Value ? null : (string)dbm.DataReaderValue("PasswordSaltedHash"),
                PasswordSalt = dbm.DataReaderValue("PasswordSalt") == DBNull.Value ? null : (string)dbm.DataReaderValue("PasswordSalt"),
                PasswordChanged = dbm.DataReaderValue("PasswordChanged") == DBNull.Value ? null : (DateTime?)dbm.DataReaderValue("PasswordChanged"),
                LastPasswordFailure = dbm.DataReaderValue("LastPasswordFailure") == DBNull.Value ? null : (DateTime?)dbm.DataReaderValue("LastPasswordFailure"),
                PasswordFailures = (int)dbm.DataReaderValue("PasswordFailures"),
                ResetPasswordTokenValue = dbm.DataReaderValue("ResetPasswordTokenValue") == DBNull.Value ? null : ((Guid)dbm.DataReaderValue("ResetPasswordTokenValue")).ToString(),
                ResetPasswordTokenExpiry = dbm.DataReaderValue("ResetPasswordTokenExpiry") == DBNull.Value ? null : (DateTime?)dbm.DataReaderValue("ResetPasswordTokenExpiry"),
                ConfirmTokenValue = dbm.DataReaderValue("ConfirmTokenValue") == DBNull.Value ? null : ((Guid)dbm.DataReaderValue("ConfirmTokenValue")).ToString(),
                ConfirmTokenExpiry = dbm.DataReaderValue("ConfirmTokenExpiry") == DBNull.Value ? null : (DateTime?)dbm.DataReaderValue("ConfirmTokenExpiry"),
                ImageTenantId = dbm.DataReaderValue("ImageTenantId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("ImageTenantId"),
                ThumbnailImageUploadId = dbm.DataReaderValue("ThumbnailImageUploadId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("ThumbnailImageUploadId"),
                PreviewImageUploadId = dbm.DataReaderValue("PreviewImageUploadId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("PreviewImageUploadId"),
                ImageUploadId = dbm.DataReaderValue("ImageUploadId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("ImageUploadId"),
                Roles = new List<Role>()
            };
            dbm.Read();
            while (dbm.Read())
                user.Roles.Add(GetRoleFromDatabaseManager(dbm));
            return user;
        }

        public User ReadUser(long tenantId, long userId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                User user = null;
                dbm.SetSQL(_sqlManager.GetSql("Sql.ReadUser.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@UserId", FieldType.BigInt, userId);
                dbm.ExecuteReader();
                if (dbm.Read())
                    user = GetUserFromDatabaseManager(dbm);
                return user;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public User ReadUserByAlias(long tenantId, string alias, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                User user = null;
                dbm.SetSQL(_sqlManager.GetSql("Sql.ReadUserByAlias.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@Alias", FieldType.NVarChar, 50, alias);
                dbm.ExecuteReader();
                if (dbm.Read())
                    user = GetUserFromDatabaseManager(dbm);
                return user;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public User ReadUserByEmail(long tenantId, string email, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                User user = null;
                dbm.SetSQL(_sqlManager.GetSql("Sql.ReadUserByEmail.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@Email", FieldType.NVarChar, 256, email);
                dbm.ExecuteReader();
                if (dbm.Read())
                    user = GetUserFromDatabaseManager(dbm);
                return user;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public User ReadUserByConfirmToken(long tenantId, Token confirmToken, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                User user = null;
                dbm.SetSQL(_sqlManager.GetSql("Sql.ReadUserByConfirmToken.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@ConfirmTokenValue", FieldType.UniqueIdentifier, confirmToken.Value);
                dbm.AddParameter("@ConfirmTokenExpiry", FieldType.DateTime, confirmToken.Expiry);
                dbm.ExecuteReader();
                if (dbm.Read())
                    user = GetUserFromDatabaseManager(dbm);
                return user;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public User ReadUserByResetPasswordToken(long tenantId, Token resetPasswordToken, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                User user = null;
                dbm.SetSQL(_sqlManager.GetSql("Sql.ReadUserByResetPasswordToken.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@ResetPasswordTokenValue", FieldType.UniqueIdentifier, resetPasswordToken.Value);
                dbm.AddParameter("@ResetPasswordTokenExpiry", FieldType.DateTime, resetPasswordToken.Expiry);
                dbm.ExecuteReader();
                if (dbm.Read())
                    user = GetUserFromDatabaseManager(dbm);
                return user;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public long CreateUser(long tenantId, string email, string alias, List<string> roles, Token confirmToken, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                UserRoleCollection userRoleCollection = new UserRoleCollection();
                foreach (string role in roles)
                    userRoleCollection.Add(role);
                dbm.SetSQL(_sqlManager.GetSql("Sql.CreateUser.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@Alias", FieldType.NVarChar, 50, alias);
                dbm.AddParameter("@Email", FieldType.NVarChar, 256, email);
                dbm.AddParameter("@ConfirmTokenValue", FieldType.UniqueIdentifier, confirmToken.Value);
                dbm.AddParameter("@ConfirmTokenExpiry", FieldType.DateTime, confirmToken.Expiry);
                dbm.AddTypedParameter("@UserRoles", FieldType.Structured, userRoleCollection.Count == 0 ? null : userRoleCollection, "cms.UserRoleTableType");
                dbm.AddOutputParameter("@UserId", FieldType.BigInt);
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                dbm.ExecuteNonQuery(outputValues);
                return (long)outputValues["@UserId"];
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public void UpdateUser(User user, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetStoredProcedure("cms.UpdateUser");
                dbm.AddParameter("@TenantId", FieldType.BigInt, user.TenantId);
                dbm.AddParameter("@UserId", FieldType.BigInt, user.UserId);
                dbm.AddParameter("@Alias", FieldType.NVarChar, 50, user.Alias);
                dbm.AddParameter("@Email", FieldType.NVarChar, 256, user.Email);
                dbm.AddParameter("@Confirmed", FieldType.Bit, user.Confirmed);
                dbm.AddParameter("@Enabled", FieldType.Bit, user.Enabled);
                dbm.AddParameter("@LockedOut", FieldType.Bit, user.LockedOut);
                dbm.AddParameter("@PasswordSaltedHash", FieldType.VarChar, 344, user.PasswordSaltedHash ?? (object)DBNull.Value);
                dbm.AddParameter("@PasswordSalt", FieldType.VarChar, 24, user.PasswordSalt ?? (object)DBNull.Value);
                dbm.AddParameter("@PasswordChanged", FieldType.DateTime, user.PasswordChanged ?? (object)DBNull.Value);
                dbm.AddParameter("@LastPasswordFailure", FieldType.DateTime, user.LastPasswordFailure ?? (object)DBNull.Value);
                dbm.AddParameter("@PasswordFailures", FieldType.Int, user.PasswordFailures);
                dbm.AddParameter("@ResetPasswordTokenValue", FieldType.UniqueIdentifier, user.ResetPasswordTokenValue != null ? new Guid(user.ResetPasswordTokenValue) : (object)DBNull.Value);
                dbm.AddParameter("@ResetPasswordTokenExpiry", FieldType.DateTime, user.ResetPasswordTokenExpiry ?? (object)DBNull.Value);
                dbm.AddParameter("@ConfirmTokenValue", FieldType.UniqueIdentifier, user.ConfirmTokenValue != null ? new Guid(user.ConfirmTokenValue) : (object)DBNull.Value);
                dbm.AddParameter("@ConfirmTokenExpiry", FieldType.DateTime, user.ConfirmTokenExpiry ?? (object)DBNull.Value);
                dbm.AddParameter("@ImageTenantId", FieldType.BigInt, user.ImageTenantId ?? (object)DBNull.Value);
                dbm.AddParameter("@ThumbnailImageUploadId", FieldType.BigInt, user.ThumbnailImageUploadId ?? (object)DBNull.Value);
                dbm.AddParameter("@PreviewImageUploadId", FieldType.BigInt, user.PreviewImageUploadId ?? (object)DBNull.Value);
                dbm.AddParameter("@ImageUploadId", FieldType.BigInt, user.ImageUploadId ?? (object)DBNull.Value);
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
