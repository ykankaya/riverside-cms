using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Domains
{
    public interface IDomainValidator
    {
        void ValidateCreate(Domain domain, string keyPrefix = null);
        void ValidateUpdate(Domain domain, string keyPrefix = null);
    }
}
