using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Forms
{
    public class FormElementField
    {
        public long TenantId { get; set; }
        public long ElementId { get; set; }
        public long FormFieldId { get; set; }
        public int SortOrder { get; set; }
        public string Label { get; set; }
        public FormElementFieldType FieldType { get; set; }
        public bool Required { get; set; }
    }
}
