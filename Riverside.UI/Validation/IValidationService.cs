using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Riverside.Utilities.Validation;

namespace Riverside.UI.Validation
{
    /// <summary>
    /// Interface for portal related services that assist with validation.
    /// </summary>
    public interface IValidationService
    {
        /// <summary>
        /// Registers validation errors with model state. Adds the string keyPrefix + "." to the start of every error key. Call this variant of register validation
        /// errors when the service layer business object is a child of the view model object and we need to ensure that validation errors are wired up correctly with
        /// the user interface.
        /// </summary>
        /// <param name="modelState">The state of model binding.</param>
        /// <param name="errors">Collection of validation errors.</param>
        /// <param name="keyPrefix">Key prefix.</param>
        void RegisterValidationErrors(ModelStateDictionary modelState, List<ValidationError> errors, string keyPrefix);

        /// <summary>
        /// Registers validation errors with model state.
        /// </summary>
        /// <param name="modelState">The state of model binding.</param>
        /// <param name="errors">Collection of validation errors.</param>
        void RegisterValidationErrors(ModelStateDictionary modelState, List<ValidationError> errors);
    }
}
