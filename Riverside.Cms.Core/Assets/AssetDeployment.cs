using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Assets
{
    /// <summary>
    /// Contains information about a website's asset deployment.
    /// </summary>
    public class AssetDeployment
    {
        /// <summary>
        /// Identifies website.
        /// </summary>
        public long TenantId { get; set; }

        /// <summary>
        /// Hostname of server or service that assets deployed to.
        /// </summary>
        public string Hostname { get; set; }

        /// <summary>
        /// Date and time that assets deployed.
        /// </summary>
        public DateTime Deployed { get; set; }
    }
}
