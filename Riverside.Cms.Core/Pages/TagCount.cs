using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Pages
{
    /// <summary>
    /// Holds tag usage information.
    /// </summary>
    public class TagCount
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
        /// The number of times a tag has been used.
        /// </summary>
        public int Count { get; set; }

		/// <summary>
		/// The relative size of this tag (a value from 1 to 10 inclusive).
		/// </summary>
		public int Size { get; set; }
    }
}
