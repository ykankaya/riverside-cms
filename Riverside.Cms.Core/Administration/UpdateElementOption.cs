using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Administration
{
    public abstract class UpdateElementOption : AdministrationOption
    {
        public long ElementId { get; set; }
        public Guid ElementTypeId { get; set; }
    }
}
