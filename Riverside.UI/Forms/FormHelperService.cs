using System;
using System.Collections.Generic;
using System.Text;
using Riverside.Utilities.Validation;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// This service provides form related helper methods.
    /// </summary>
    public class FormHelperService : IFormHelperService
    {
        /// <summary>
        /// Gets a form result containing no errors.
        /// </summary>
        /// <param name="status">Form result status (can be null if no status).</param>
        /// <returns>Form result with empty errors collection .</returns>
        public FormResult GetFormResult(string status = null)
        {
            return new FormResult { Status = status };
        }

        /// <summary>
        /// Gets a form error name.
        /// </summary>
        /// <param name="key">Validation error key (e.g. "User.Email").</param>
        /// <param name="keyPrefix">Validation error key prefix (e.g. "UserGroup").</param>
        /// <returns>Form error name (e.g. "userGroup.user.email").</returns>
        private string GetFormErrorName(string key, string keyPrefix)
        {
            if (key == null)
                return null;
            if (keyPrefix != null)
                key = keyPrefix + "." + key;
            string[] parts = key.Split('.');
            StringBuilder sb = new StringBuilder();
            for (int index = 0; index < parts.Length; index++)
            {
                string part = parts[index];
                part = Char.ToLowerInvariant(part[0]) + part.Substring(1);
                sb.Append(part);
                if (index != parts.Length - 1)
                    sb.Append(".");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns a form result, given a collection of validation errors. Adds the string keyPrefix + "." to the start of every error key. Call this variant of register validation
        /// errors when the service layer business object is a child of the view model object and we need to ensure that validation errors are wired up correctly with
        /// the user interface.
        /// </summary>
        /// <param name="errors">Collection of validation errors.</param>
        /// <param name="keyPrefix">Key prefix (can be null if no key prefix required).</param>
        /// <returns>Form result populated with form errors.</returns>
        public FormResult GetFormResultWithValidationErrors(IEnumerable<ValidationError> errors, string keyPrefix = null)
        {
            // Create form result
            FormResult result = new FormResult { Errors = new List<FormError>() };

            // If errors specified, convert them to form errors
            if (errors != null)
            {
                foreach (ValidationError error in errors)
                {
                    string errorName = GetFormErrorName(error.Key, keyPrefix);
                    result.Errors.Add(new FormError { Name = errorName, Message = error.Message });
                }
            }

            // Return form result
            return result;
        }

        /// <summary>
        /// Returns a form result containing a single generic form error that is not associated with any field.
        /// </summary>
        /// <param name="message">Error message text.</param>
        /// <returns>Form result populated with single generic form error.</returns>
        public FormResult GetFormResultWithErrorMessage(string message)
        {
            return new FormResult
            {
                Errors = new List<FormError> { new FormError { Name = null, Message = message } }
            };
        }
    }
}
