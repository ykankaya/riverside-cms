using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Pages
{
    /// <summary>
    /// Holds tagging information.
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// The website that this tag is associated with.
        /// </summary>
        public long TenantId { get; set; }

        /// <summary>
        /// Unique tag identifier.
        /// </summary>
        public long TagId { get; set; }

        /// <summary>
        /// Tag name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Date and time tag created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Date and time tag last updated.
        /// </summary>
        public DateTime Updated { get; set; }
    }
}
