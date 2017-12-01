using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Core.Elements
{
    public interface IListElementServices
    {
        IEnumerable<Type> ListTypes();
    }
}
