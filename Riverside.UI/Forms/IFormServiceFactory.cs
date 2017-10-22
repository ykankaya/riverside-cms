using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Interface for factories that can create form services.
    /// </summary>
    public interface IFormServiceFactory
    {
        /// <summary>
        /// Gets form service for the specified form identifier.
        /// </summary>
        /// <param name="formId">Form identifier.</param>
        /// <returns>Applicable form service.</returns>
        IFormService GetFormService(string formId);
    }
}
