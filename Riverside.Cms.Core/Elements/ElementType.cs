using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Elements
{
    public class ElementType
    {
        public Guid ElementTypeId { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, ElementTypeId);
        }
    }
}
