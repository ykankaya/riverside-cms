using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Data
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork();
        }
    }
}
