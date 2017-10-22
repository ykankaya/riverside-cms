using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Data
{
    public interface ITransactional
    {
        void Commit();
        void Rollback();
    }
}
