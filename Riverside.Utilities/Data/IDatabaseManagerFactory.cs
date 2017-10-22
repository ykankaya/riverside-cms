using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Data
{
    public interface IDatabaseManagerFactory
    {
        /// <summary>
        /// Retrieves a database manager for executing database commands. If a unit of work is not specified, a new database manager is created and returned by this method.
        /// If a unit of work is specified, this method looks to see if a database manager exists for the specified unit of work. If one does exist, this is returned. Otherwise
        /// a new database manager is created, associated with the unit of work and returned.
        /// </summary>
        /// <param name="unitOfWork">Unit of work (null if no unit of work).</param>
        /// <returns>A database manager.</returns>
        IDatabaseManager GetDatabaseManager(IUnitOfWork unitOfWork = null);
    }
}
