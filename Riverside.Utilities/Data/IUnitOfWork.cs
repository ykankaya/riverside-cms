using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Data
{
    public interface IUnitOfWork : ITransactional, IDisposable
    {
        /// <summary>
        /// Registers a work item.
        /// </summary>
        /// <param name="t">The type of work item that is registered.</param>
        /// <param name="item">The item that is added to this unit of work.</param>
        void RegisterWorkItem(Type t, ITransactional item);

        /// <summary>
        /// Finds first work item with a given type. If work item not found, then null is returned.
        /// </summary>
        /// <param name="t">The type of work item that is returned.</param>
        /// <returns>Identified work item (or null if no work items of type specified found).</returns>
        ITransactional FindWorkItem(Type t);
    }
}
