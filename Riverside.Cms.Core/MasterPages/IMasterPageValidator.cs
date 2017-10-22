using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.MasterPages
{
    /// <summary>
    /// Interface for classes that validate master page actions.
    /// </summary>
    public interface IMasterPageValidator
    {
        /// <summary>
        /// Validates update master page zone model. Throws validation error exception if validation fails.
        /// </summary>
        /// <param name="masterPageZone">Updated master page zone details.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        void ValidateUpdateZone(MasterPageZone masterPageZone, string keyPrefix = null);
    }
}
