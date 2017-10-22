using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Riverside.Utilities.Validation;

namespace Riverside.UI.Validation
{
    /// <summary>
    /// Service that assists with model validation in ASP.NET MVC.
    /// </summary>
    public class ValidationService : IValidationService
    {
        /// <summary>
        /// Registers validation errors with model state. Adds the string keyPrefix + "." to the start of every error key. Call this variant of register validation
        /// errors when the service layer business object is a child of the view model object and we need to ensure that validation errors are wired up correctly with
        /// the user interface.
        /// </summary>
        /// <param name="modelState">The state of model binding.</param>
        /// <param name="errors">Collection of validation errors.</param>
        /// <param name="keyPrefix">Key prefix.</param>
        public void RegisterValidationErrors(ModelStateDictionary modelState, List<ValidationError> errors, string keyPrefix)
        {
            foreach (ValidationError error in errors)
            {
                string key = error.Key ?? string.Empty;
                if (keyPrefix != null && key != string.Empty)
                    key = keyPrefix + "." + key;
                modelState.AddModelError(key, error.Message);
            }
        }

        /// <summary>
        /// Registers validation errors with model state.
        /// </summary>
        /// <param name="modelState">The state of model binding.</param>
        /// <param name="errors">Collection of validation errors.</param>
        public void RegisterValidationErrors(ModelStateDictionary modelState, List<ValidationError> errors)
        {
            RegisterValidationErrors(modelState, errors, null);
        }
    }
}
