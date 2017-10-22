using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Data
{
    public class DatabaseManagerFactory : IDatabaseManagerFactory
    {
        /// <summary>
        /// Retrieves a database manager for executing database commands. If a unit of work is not specified, a new database manager is created and returned by this method.
        /// If a unit of work is specified, this method looks to see if a database manager exists for the specified unit of work. If one does exist, this is returned. Otherwise
        /// a new database manager is created, associated with the unit of work and returned.
        /// </summary>
        /// <param name="unitOfWork">Unit of work (null if no unit of work).</param>
        /// <returns>A database manager.</returns>
        public IDatabaseManager GetDatabaseManager(IUnitOfWork unitOfWork = null)
        {
            // No unit of work
            if (unitOfWork == null)
                return new DatabaseManager(ConnectionString, false);

            // Unit of work specified, so try to find any active database managers already registered as tasks
            ITransactional workItem = unitOfWork.FindWorkItem(typeof(IDatabaseManager));
            if (workItem != null)
                return (IDatabaseManager)workItem;

            // If database manager not found, create one and associate it with the unit of work
            IDatabaseManager databaseManager = new DatabaseManager(ConnectionString, true);
            unitOfWork.RegisterWorkItem(typeof(IDatabaseManager), databaseManager);
            return databaseManager;
        }

        /// <summary>
        /// The connection string that is used.
        /// </summary>
        protected virtual string ConnectionString { get; set; }
    }
}
