using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Riverside.Utilities.Validation
{
    /// <summary>
    /// Validates an object's data annotations.
    /// </summary>
    public class ModelValidator : IModelValidator
    {
        /// <summary>
        /// Validates model. Throws validation error exception if any data annotations fail to validate.
        /// </summary>
        /// <param name="model">The model to validate.</param>
        /// <param name="keyPrefix">Validation key prefix.</param>
        public void Validate(object model, string keyPrefix = null)
        {
            List<ValidationError> errors = new List<ValidationError>();
            ValidationContext context = new ValidationContext(model, null, null);
            List<ValidationResult> results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(model, context, results, true))
            {
                foreach (ValidationResult result in results)
                {
                    foreach (string memberName in result.MemberNames)
                    {
                        errors.Add(new ValidationError(memberName, result.ErrorMessage, keyPrefix));
                    }
                }
            }
            if (errors.Count > 0)
                throw new ValidationErrorException(errors);
        }
    }
}
