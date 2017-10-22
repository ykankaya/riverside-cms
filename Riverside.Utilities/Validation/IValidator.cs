using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Validation
{
    public interface IValidator<T>
    {
        /// <summary>
        /// Validates model. Throws validation error exception if validation fails.
        /// </summary>
        /// <param name="model">The model to validate.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        void Validate(T model, string keyPrefix = null);
    }
}
