using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Webs
{
    /// <summary>
    /// Interface for types that validate website create, update and delete actions.
    /// </summary>
    public interface IWebValidator
    {
        /// <summary>
        /// Validates create website model. Throws validation error exception if validation fails.
        /// </summary>
        /// <param name="web">The website to validate.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        void ValidateCreate(Web web, string keyPrefix = null);

        /// <summary>
        /// Validates update website model. Throws validation error exception if validation fails.
        /// </summary>
        /// <param name="web">Updated website details.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        void ValidateUpdate(Web web, string keyPrefix = null);
    }
}
