using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;

namespace Riverside.Cms.Elements.Tables
{
    /// <summary>
    /// Determines what is displayed in a table element.
    /// </summary>
    public class TableSettings : ElementSettings
    {
        /// <summary>
        /// Display name is a header that is rendered at the top of table element. Can be left null if a header is not required.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// If set, preamble text rendered above table information. Can be left null if not required.
        /// </summary>
        public string Preamble { get; set; }

        /// <summary>
        /// Set true to display headers. In which case, first row is used to populate headers.
        /// </summary>
        public bool ShowHeaders { get; set; }

        /// <summary>
        /// CSV data that makes up table headers and rows.
        /// </summary>
        public string Rows { get; set; }
    }
}
