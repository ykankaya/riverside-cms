using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        // Member variables
        private bool _disposed;
        private Dictionary<string, List<ITransactional>> _itemsByType;
        private List<ITransactional> _items;

        /// <summary>
        /// Disposes all work items, then empties list of items maintained by this unit of work. Exceptions thrown during disposal are caught and ignored (i.e. not propogated upwards).
        /// </summary>
        private void DisposeManagedResources()
        {
            if (_items != null)
            {
                foreach (ITransactional item in _items)
                {
                    if (item is IDisposable)
                    {
                        try
                        {
                            ((IDisposable)item).Dispose();
                        }
                        catch
                        {
                        }
                    }
                }
                _items.Clear();
            }
            if (_itemsByType != null)
                _itemsByType.Clear();
        }

        /// <summary>
        /// Dispose(bool disposing) executes in two distinct scenarios. If disposing equals true, the method has been called directly or indirectly by
        /// a user's code. Managed and unmanaged resources can be disposed. If disposing equals false, the method has been called by the runtime from
        /// inside the finalizer and you should not reference other objects. Only unmanaged resources can be disposed.
        /// </summary>
        /// <param name="disposing">True if function called from user code, false if called from finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called
            if (!_disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources
                if (disposing)
                {
                    // Dispose managed resources
                    DisposeManagedResources();
                }

                // Clean up unmanaged resources here (there are none)
            }
            _disposed = true;
        }

        /// <summary>
        /// This destructor will run only if the Dispose method does not get called.
        /// </summary>
        ~UnitOfWork()
        {
            Dispose(false);
        }

        /// <summary>
        /// Implement IDisposable.
        /// </summary>
        public void Dispose()
        {
            // Clean up this object
            Dispose(true);

            // Take this object off the finalization queue and prevent finalization code for this object from executing a second time
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Commit work items. Exceptions thrown during work item rollbacks are caught and ignored (i.e. not propogated upwards).
        /// </summary>
        public void Commit()
        {
            if (_items != null)
            {
                foreach (ITransactional item in _items)
                {
                    try
                    {
                        item.Commit();
                    }
                    catch
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Rollback work items. Exceptions thrown during work item rollbacks are caught and ignored (i.e. not propogated upwards).
        /// </summary>
        public void Rollback()
        {
            if (_items != null)
            {
                foreach (ITransactional item in _items)
                {
                    try
                    {
                        item.Rollback();
                    }
                    catch
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Finds first work item with a given type. If work item not found, then null is returned.
        /// </summary>
        /// <param name="t">The type of work item that is returned.</param>
        /// <returns>Identified work item (or null if no work items of type specified found).</returns>
        public ITransactional FindWorkItem(Type t)
        {
            if (_itemsByType != null && _itemsByType.ContainsKey(t.FullName))
                return _itemsByType[t.FullName][0];
            return null;
        }

        /// <summary>
        /// Registers a work item.
        /// </summary>
        /// <param name="t">The type of work item that is registered.</param>
        /// <param name="item">The item that is added to this unit of work.</param>
        public void RegisterWorkItem(Type t, ITransactional item)
        {
            if (_items == null)
                _items = new List<ITransactional>();
            if (_itemsByType == null)
                _itemsByType = new Dictionary<string, List<ITransactional>>();
            _items.Add(item);
            string typeName = t.FullName;
            if (!_itemsByType.ContainsKey(typeName))
                _itemsByType.Add(typeName, new List<ITransactional>());
            _itemsByType[typeName].Add(item);
        }
    }
}
