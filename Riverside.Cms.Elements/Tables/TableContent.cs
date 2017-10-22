using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;

namespace Riverside.Cms.Elements.Tables
{
    /// <summary>
    /// Holds dynamic table content.
    /// </summary>
    public class TableContent : ElementContent
    {
        /// <summary>
        /// List of table headers. Set null if no headers.
        /// </summary>
        public List<string> Headers { get; set; }

        /// <summary>
        /// List of table rows. Leave empty if no rows.
        /// </summary>
        public List<List<string>> Rows { get; set; }
    }
}
