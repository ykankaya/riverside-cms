using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.Testimonials
{
    /// <summary>
    /// SQL implementation of repository that stores and manipulates testimonial elements.
    /// </summary>
    public class SqlTestimonialRepository : ITestimonialRepository
    {
        // Member variables
        private IDatabaseManagerFactory _databaseManagerFactory;
        private ISqlManager _sqlManager;
        private IUnitOfWorkFactory _unitOfWorkFactory;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="databaseManagerFactory">Database manager factory.</param>
        /// <param name="sqlManager">For the retrieval of SQL embedded resource files.</param>
        /// <param name="unitOfWorkFactory">Can be used to create a unit of work where transactions required.</param>
        public SqlTestimonialRepository(IDatabaseManagerFactory databaseManagerFactory, ISqlManager sqlManager, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _databaseManagerFactory = databaseManagerFactory;
            _unitOfWorkFactory = unitOfWorkFactory;
            _sqlManager = sqlManager;
        }

        /// <summary>
        /// Constructs testimonial comment collection for calls to stored procedures that contain user defined table types.
        /// </summary>
        /// <param name="settings">Testimonial settings.</param>
        /// <returns>A populated testimonial comment collection.</returns>
        private TestimonialCommentCollection GetTestimonialCommentCollection(TestimonialSettings settings)
        {
            TestimonialCommentCollection testimonialCommentCollection = new TestimonialCommentCollection();
            foreach (TestimonialComment comment in settings.Comments)
                testimonialCommentCollection.Add(comment);
            return testimonialCommentCollection;
        }

        /// <summary>
        /// Creates a testimonial element.
        /// </summary>
        /// <param name="settings">New element settings.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Create(TestimonialSettings settings, IUnitOfWork unitOfWork = null)
        {
            TestimonialCommentCollection testimonialCommentCollection = GetTestimonialCommentCollection(settings);
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                dbm.SetSQL(_sqlManager.GetSql("Sql.CreateTestimonial.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@DisplayName", FieldType.NVarChar, 256, settings.DisplayName);
                dbm.AddParameter("@Preamble", FieldType.NVarChar, -1, settings.Preamble);
                dbm.AddTypedParameter("@TestimonialComments", FieldType.Structured, testimonialCommentCollection.Count == 0 ? null : testimonialCommentCollection, "element.TestimonialCommentTableType");
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
        /// Copies testimonial specific data from source element to destination element.
        /// </summary>
        /// <param name="sourceTenantId">Source tenant identifier.</param>
        /// <param name="sourceElementId">Identifies source element.</param>
        /// <param name="destTenantId">Destination tenant identifier.</param>
        /// <param name="destElementId">Identifies destination element.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null)
        {
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                dbm.SetSQL(_sqlManager.GetSql("Sql.CopyTestimonial.sql"));
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

        /// <summary>
        /// Populates element settings.
        /// </summary>
        /// <param name="settings">Element settings to be populated.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Read(TestimonialSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                dbm.SetSQL(_sqlManager.GetSql("Sql.ReadTestimonial.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.ExecuteReader();
                dbm.Read();
                settings.DisplayName = (string)dbm.DataReaderValue("DisplayName");
                settings.Preamble = (string)dbm.DataReaderValue("Preamble");
                settings.Comments = new List<TestimonialComment>();
                dbm.Read();
                while (dbm.Read())
                {
                    settings.Comments.Add(new TestimonialComment
                    {
                        TenantId = (long)dbm.DataReaderValue("TenantId"),
                        ElementId = (long)dbm.DataReaderValue("ElementId"),
                        TestimonialCommentId = (long)dbm.DataReaderValue("TestimonialCommentId"),
                        SortOrder = (int)dbm.DataReaderValue("SortOrder"),
                        Comment = (string)dbm.DataReaderValue("Comment"),
                        Author = (string)dbm.DataReaderValue("Author"),
                        AuthorTitle = (string)dbm.DataReaderValue("AuthorTitle"),
                        CommentDate = (string)dbm.DataReaderValue("CommentDate")
                    });
                }
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        /// <summary>
        /// Updates testimonial details.
        /// </summary>
        /// <param name="settings">Updated element settings.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Update(TestimonialSettings settings, IUnitOfWork unitOfWork = null)
        {
            TestimonialCommentCollection testimonialCommentCollection = GetTestimonialCommentCollection(settings);
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                dbm.SetSQL(_sqlManager.GetSql("Sql.UpdateTestimonial.sql"));
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@DisplayName", FieldType.NVarChar, 256, settings.DisplayName);
                dbm.AddParameter("@Preamble", FieldType.NVarChar, -1, settings.Preamble);
                dbm.AddTypedParameter("@TestimonialComments", FieldType.Structured, testimonialCommentCollection.Count == 0 ? null : testimonialCommentCollection, "element.TestimonialCommentTableType");
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
        /// Deletes a testimonial element.
        /// </summary>
        /// <param name="tenantId">The tenant that element to delete belongs to.</param>
        /// <param name="elementId">Identifies the element to delete.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        public void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null)
        {
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                dbm.SetSQL(_sqlManager.GetSql("Sql.DeleteTestimonial.sql"));
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
