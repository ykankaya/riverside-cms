using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Pages;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.Forums
{
    public class SqlForumRepository : IForumRepository
    {
        private IDatabaseManagerFactory _databaseManagerFactory;
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private ISqlManager _sqlManager;

        public SqlForumRepository(IDatabaseManagerFactory databaseManagerFactory, IUnitOfWorkFactory unitOfWorkFactory, ISqlManager sqlManager)
        {
            _databaseManagerFactory = databaseManagerFactory;
            _unitOfWorkFactory = unitOfWorkFactory;
            _sqlManager = sqlManager;
        }

        public void Create(ForumSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.CreateForum.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@ThreadCount", FieldType.Int, settings.ThreadCount);
                dbm.AddParameter("@PostCount", FieldType.Int, settings.PostCount);
                dbm.AddParameter("@OwnerTenantId", FieldType.BigInt, settings.OwnerTenantId ?? (object)DBNull.Value);
                dbm.AddParameter("@OwnerUserId", FieldType.BigInt, settings.OwnerUserId ?? (object)DBNull.Value);
                dbm.AddParameter("@OwnerOnlyThreads", FieldType.Bit, settings.OwnerOnlyThreads);
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public long CreateThread(CreateThreadInfo info, DateTime created, IUnitOfWork unitOfWork = null)
        {
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                string sql = _sqlManager.GetSql("Sql.CreateForumThread.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, info.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, info.ElementId);
                dbm.AddParameter("@Subject", FieldType.NVarChar, 256, info.Subject);
                dbm.AddParameter("@Notify", FieldType.Bit, info.Notify);
                dbm.AddParameter("@UserId", FieldType.BigInt, info.UserId);
                dbm.AddParameter("@Message", FieldType.NVarChar, -1, info.Message);
                dbm.AddParameter("@Created", FieldType.DateTime, created);
                dbm.AddOutputParameter("@ThreadId", FieldType.BigInt);
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                dbm.ExecuteNonQuery(outputValues);
                if (localUnitOfWork != null)
                    localUnitOfWork.Commit();
                return (long)outputValues["@ThreadId"];
            }
            catch (Exception)
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

        public long CreatePost(CreatePostInfo info, DateTime created, IUnitOfWork unitOfWork = null)
        {
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                string sql = _sqlManager.GetSql("Sql.CreateForumPost.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, info.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, info.ElementId);
                dbm.AddParameter("@ThreadId", FieldType.BigInt, info.ThreadId);
                dbm.AddParameter("@ParentPostId", FieldType.BigInt, info.ParentPostId == null ? (object)DBNull.Value : (object)info.ParentPostId);
                dbm.AddParameter("@UserId", FieldType.BigInt, info.UserId);
                dbm.AddParameter("@Message", FieldType.NVarChar, -1, info.Message);
                dbm.AddParameter("@Created", FieldType.DateTime, created);
                dbm.AddOutputParameter("@PostId", FieldType.BigInt);
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                dbm.ExecuteNonQuery(outputValues);
                if (localUnitOfWork != null)
                    localUnitOfWork.Commit();
                return (long)outputValues["@PostId"];
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
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.CopyForum.sql");
                dbm.SetSQL(sql);
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

        public void Read(ForumSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.ReadForum.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.ExecuteReader();
                dbm.Read();
                settings.OwnerOnlyThreads = (bool)dbm.DataReaderValue("OwnerOnlyThreads");
                settings.OwnerUserId = dbm.DataReaderValue("OwnerUserId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("OwnerUserId");
                settings.OwnerTenantId = dbm.DataReaderValue("OwnerTenantId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("OwnerTenantId");
                settings.PostCount = (int)dbm.DataReaderValue("PostCount");
                settings.ThreadCount = (int)dbm.DataReaderValue("ThreadCount");
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public void Update(ForumSettings settings, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.UpdateForum.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, settings.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, settings.ElementId);
                dbm.AddParameter("@OwnerTenantId", FieldType.BigInt, settings.OwnerTenantId ?? (object)DBNull.Value);
                dbm.AddParameter("@OwnerUserId", FieldType.BigInt, settings.OwnerUserId ?? (object)DBNull.Value);
                dbm.AddParameter("@OwnerOnlyThreads", FieldType.Bit, settings.OwnerOnlyThreads);
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
                string sql = _sqlManager.GetSql("Sql.DeleteForum.sql");
                dbm.SetSQL(sql);
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

        public ForumThread GetThread(long tenantId, long elementId, long threadId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                ForumThread thread = null;
                string sql = _sqlManager.GetSql("Sql.ReadForumThread.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, elementId);
                dbm.AddParameter("@ThreadId", FieldType.BigInt, threadId);
                dbm.ExecuteReader();
                if (dbm.Read())
                    thread = GetThread(dbm);
                return thread;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public ForumThreadAndUser GetThreadAndUser(long tenantId, long elementId, long threadId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                ForumThreadAndUser thread = null;
                string sql = _sqlManager.GetSql("Sql.ReadForumThreadAndUser.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, elementId);
                dbm.AddParameter("@ThreadId", FieldType.BigInt, threadId);
                dbm.ExecuteReader();
                if (dbm.Read())
                {
                    thread = new ForumThreadAndUser
                    {
                        Thread = GetThread(dbm),
                        User = GetUser(dbm)
                    };
                }
                return thread;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public ForumPostAndUser GetPostAndUser(long tenantId, long elementId, long threadId, long postId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                ForumPostAndUser post = null;
                string sql = _sqlManager.GetSql("Sql.ReadForumPostAndUser.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, elementId);
                dbm.AddParameter("@ThreadId", FieldType.BigInt, threadId);
                dbm.AddParameter("@PostId", FieldType.BigInt, postId);
                dbm.ExecuteReader();
                if (dbm.Read())
                {
                    post = new ForumPostAndUser
                    {
                        Post = GetPost(dbm),
                        User = GetUser(dbm)
                    };
                }
                return post;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public ForumPost GetPost(long tenantId, long elementId, long threadId, long postId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                ForumPost post = null;
                string sql = _sqlManager.GetSql("Sql.ReadForumPost.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, elementId);
                dbm.AddParameter("@ThreadId", FieldType.BigInt, threadId);
                dbm.AddParameter("@PostId", FieldType.BigInt, postId);
                dbm.ExecuteReader();
                if (dbm.Read())
                    post = GetPost(dbm);
                return post;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public void IncrementThreadViews(long tenantId, long elementId, long threadId, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.IncrementForumThreadViews.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, elementId);
                dbm.AddParameter("@ThreadId", FieldType.BigInt, threadId);
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        private ForumPost GetPost(IDatabaseManager dbm)
        {
            return new ForumPost
            {
                TenantId = (long)dbm.DataReaderValue("TenantId"),
                ElementId = (long)dbm.DataReaderValue("ElementId"),
                ThreadId = (long)dbm.DataReaderValue("ThreadId"),
                PostId = (long)dbm.DataReaderValue("PostId"),
                ParentPostId = dbm.DataReaderValue("ParentPostId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("ParentPostId"),
                UserId = (long)dbm.DataReaderValue("UserId"),
                Message = (string)dbm.DataReaderValue("Message"),
                SortOrder = (int)dbm.DataReaderValue("SortOrder"),
                Created = (DateTime)dbm.DataReaderValue("Created"),
                Updated = (DateTime)dbm.DataReaderValue("Updated")
            };
        }

        private ForumThread GetThread(IDatabaseManager dbm)
        {
            return new ForumThread
            {
                Created = (DateTime)dbm.DataReaderValue("Created"),
                ElementId = (long)dbm.DataReaderValue("ElementId"),
                LastMessageCreated = (DateTime)dbm.DataReaderValue("LastMessageCreated"),
                LastPostId = dbm.DataReaderValue("LastPostId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("LastPostId"),
                Message = (string)dbm.DataReaderValue("Message"),
                Notify = (bool)dbm.DataReaderValue("Notify"),
                Replies = (int)dbm.DataReaderValue("Replies"),
                Subject = (string)dbm.DataReaderValue("Subject"),
                ThreadId = (long)dbm.DataReaderValue("ThreadId"),
                Updated = (DateTime)dbm.DataReaderValue("Updated"),
                UserId = (long)dbm.DataReaderValue("UserId"),
                Views = (int)dbm.DataReaderValue("Views"),
                TenantId = (long)dbm.DataReaderValue("TenantId")
            };
        }

        private ForumUser GetUser(IDatabaseManager dbm, string prefix = "")
        {
            return new ForumUser
            {
                Alias = (string)dbm.DataReaderValue(prefix + "Alias"),
                Height = dbm.DataReaderValue(prefix + "Height") == DBNull.Value ? null : (int?)dbm.DataReaderValue(prefix + "Height"),
                Width = dbm.DataReaderValue(prefix + "Width") == DBNull.Value ? null : (int?)dbm.DataReaderValue(prefix + "Width"),
                Uploaded = dbm.DataReaderValue(prefix + "Uploaded") == DBNull.Value ? null : (DateTime?)dbm.DataReaderValue(prefix + "Uploaded")
            };
        }

        public ForumPosts ListPosts(long tenantId, long elementId, long threadId, int pageIndex, int pageSize, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.ListForumThreadPosts.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, elementId);
                dbm.AddParameter("@ThreadId", FieldType.BigInt, threadId);
                dbm.AddParameter("@PageIndex", FieldType.Int, pageIndex);
                dbm.AddParameter("@PageSize", FieldType.Int, pageSize);
                dbm.ExecuteReader();
                ForumPosts posts = new ForumPosts();
                while (dbm.Read())
                {
                    posts.Add(new ForumPostAndUser
                    {
                        Post = GetPost(dbm),
                        User = GetUser(dbm)
                    });
                }
                dbm.Read();
                posts.Total = (int)dbm.DataReaderValue("Replies");
                return posts;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public ForumThreads ListThreads(long tenantId, long elementId, int pageIndex, int pageSize, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.ListForumThreads.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, elementId);
                dbm.AddParameter("@PageIndex", FieldType.Int, pageIndex);
                dbm.AddParameter("@PageSize", FieldType.Int, pageSize);
                dbm.ExecuteReader();
                ForumThreads threads = new ForumThreads();
                while (dbm.Read())
                {
                    threads.Add(new ForumThreadExtended
                    {
                        Thread = GetThread(dbm),
                        User = GetUser(dbm),
                        LastPostUserId = dbm.DataReaderValue("LastPostUserId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("LastPostUserId"),
                        LastPostUser = dbm.DataReaderValue("LastPostAlias") == DBNull.Value ? null : GetUser(dbm, "LastPost")
                    });
                }
                dbm.Read();
                threads.Total = (int)dbm.DataReaderValue("ThreadCount");
                return threads;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public ForumThreads ListLatestThreads(long tenantId, long? pageId, int pageSize, bool recursive, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                int pageIndex = 0;
                ForumThreads threads = new ForumThreads();
                string sql = _sqlManager.GetSql(recursive ? "Sql.ListLatestThreadsRecursive.sql" : "Sql.ListLatestThreads.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@PageId", FieldType.BigInt, pageId ?? (object)DBNull.Value);
                dbm.AddParameter("@PageIndex", FieldType.Int, pageIndex);
                dbm.AddParameter("@PageSize", FieldType.Int, pageSize);
                dbm.ExecuteReader();
                while (dbm.Read())
                {
                    threads.Add(new ForumThreadExtended
                    {
                        Thread = GetThread(dbm),
                        User = GetUser(dbm),
                        LastPostUserId = dbm.DataReaderValue("LastPostUserId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("LastPostUserId"),
                        LastPostUser = dbm.DataReaderValue("LastPostAlias") == DBNull.Value ? null : GetUser(dbm, "LastPost"),
                        PageId = (long)dbm.DataReaderValue("PageId")
                    });
                }
                return threads;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public ForumThreads ListTaggedLatestThreads(long tenantId, long? pageId, int pageSize, IList<Tag> tags, bool recursive, IUnitOfWork unitOfWork = null)
        {
            TagCollection tagCollection = new TagCollection();
            foreach (Tag tag in tags)
                tagCollection.Add(tag);
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                int pageIndex = 0;
                ForumThreads threads = new ForumThreads();
                string sql = _sqlManager.GetSql(recursive ? "Sql.ListTaggedLatestThreadsRecursive.sql" : "Sql.ListTaggedLatestThreads.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@PageId", FieldType.BigInt, pageId ?? (object)DBNull.Value);
                dbm.AddParameter("@PageIndex", FieldType.Int, pageIndex);
                dbm.AddParameter("@PageSize", FieldType.Int, pageSize);
                dbm.AddTypedParameter("@Tags", FieldType.Structured, tagCollection.Count == 0 ? null : tagCollection, "cms.TagTableType");
                dbm.ExecuteReader();
                while (dbm.Read())
                {
                    threads.Add(new ForumThreadExtended
                    {
                        Thread = GetThread(dbm),
                        User = GetUser(dbm),
                        LastPostUserId = dbm.DataReaderValue("LastPostUserId") == DBNull.Value ? null : (long?)dbm.DataReaderValue("LastPostUserId"),
                        LastPostUser = dbm.DataReaderValue("LastPostAlias") == DBNull.Value ? null : GetUser(dbm, "LastPost"),
                        PageId = (long)dbm.DataReaderValue("PageId")
                    });
                }
                return threads;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public IDictionary<long, int> ListPageForumCounts(long tenantId, IEnumerable<Page> pages, IUnitOfWork unitOfWork = null)
        {
            PageCollection pageCollection = new PageCollection();
            foreach (Page page in pages)
                pageCollection.Add(page);
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.ListPageForumCounts.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddTypedParameter("@Pages", FieldType.Structured, pageCollection.Count == 0 ? null : pageCollection, "cms.PageTableType");
                dbm.ExecuteReader();
                Dictionary<long, int> countsByPageId = new Dictionary<long, int>();
                while (dbm.Read())
                {
                    long pageId = (long)dbm.DataReaderValue("PageId");
                    int threadCount = (int)dbm.DataReaderValue("ThreadCount");
                    int postCount = (int)dbm.DataReaderValue("PostCount");
                    countsByPageId.Add(pageId, threadCount + postCount);
                }
                return countsByPageId;
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public void UpdateThread(UpdateThreadInfo info, DateTime updated, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.UpdateForumThread.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, info.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, info.ElementId);
                dbm.AddParameter("@ThreadId", FieldType.BigInt, info.ThreadId);
                dbm.AddParameter("@Subject", FieldType.NVarChar, 256, info.Subject);
                dbm.AddParameter("@Message", FieldType.NVarChar, -1, info.Message);
                dbm.AddParameter("@Notify", FieldType.Bit, info.Notify);
                dbm.AddParameter("@Updated", FieldType.DateTime, updated);
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public void UpdatePost(UpdatePostInfo info, DateTime updated, IUnitOfWork unitOfWork = null)
        {
            IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork);
            try
            {
                string sql = _sqlManager.GetSql("Sql.UpdateForumPost.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, info.TenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, info.ElementId);
                dbm.AddParameter("@ThreadId", FieldType.BigInt, info.ThreadId);
                dbm.AddParameter("@PostId", FieldType.BigInt, info.PostId);
                dbm.AddParameter("@Message", FieldType.NVarChar, -1, info.Message);
                dbm.AddParameter("@Updated", FieldType.DateTime, updated);
                dbm.ExecuteNonQuery();
            }
            finally
            {
                if (unitOfWork == null)
                    dbm.Dispose();
            }
        }

        public void DeleteThread(long tenantId, long elementId, long threadId, IUnitOfWork unitOfWork = null)
        {
            IUnitOfWork localUnitOfWork = unitOfWork == null ? _unitOfWorkFactory.CreateUnitOfWork() : null;
            try
            {
                IDatabaseManager dbm = _databaseManagerFactory.GetDatabaseManager(unitOfWork ?? localUnitOfWork);
                string sql = _sqlManager.GetSql("Sql.DeleteForumThread.sql");
                dbm.SetSQL(sql);
                dbm.AddParameter("@TenantId", FieldType.BigInt, tenantId);
                dbm.AddParameter("@ElementId", FieldType.BigInt, elementId);
                dbm.AddParameter("@ThreadId", FieldType.BigInt, threadId);
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
