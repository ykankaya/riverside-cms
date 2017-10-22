using System;
using System.Collections.Generic;
using System.Text;
using Riverside.Utilities.Validation;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Implement this interface to provide form related helper methods.
    /// </summary>
    public interface IFormHelperService
    {
        /// <summary>
        /// Gets a form result containing no errors.
        /// </summary>
        /// <param name="status">Form result status (can be null if no status).</param>
        /// <returns>Form result with empty errors collection .</returns>
        FormResult GetFormResult(string status = null);

        /// <summary>
        /// Returns a form result, given a collection of validation errors. Adds the string keyPrefix + "." to the start of every error key. Call this variant of register validation
        /// errors when the service layer business object is a child of the view model object and we need to ensure that validation errors are wired up correctly with
        /// the user interface.
        /// </summary>
        /// <param name="errors">Collection of validation errors.</param>
        /// <param name="keyPrefix">Key prefix (can be null if no key prefix required).</param>
        /// <returns>Form result populated with form errors.</returns>
        FormResult GetFormResultWithValidationErrors(IEnumerable<ValidationError> errors, string keyPrefix = null);

        /// <summary>
        /// Returns a form result containing a single generic form error that is not associated with any field.
        /// </summary>
        /// <param name="message">Error message text.</param>
        /// <returns>Form result populated with single generic form error.</returns>
        FormResult GetFormResultWithErrorMessage(string message);
    }
}
