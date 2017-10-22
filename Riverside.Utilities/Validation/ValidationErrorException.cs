using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Validation
{
    public class ValidationErrorException : Exception
    {
        public ValidationErrorException() { }
        public ValidationErrorException(string message) : base(message) { }
        public ValidationErrorException(string message, Exception inner) : base(message, inner) { }
        public ValidationErrorException(string message, List<ValidationError> errors) : base(message) { Errors = errors; }
        public ValidationErrorException(string message, ValidationError error) : base(message) { Errors = new List<ValidationError> { error }; }
        public ValidationErrorException(string message, List<ValidationError> errors, Exception inner) : base(message, inner) { Errors = errors; }
        public ValidationErrorException(string message, ValidationError error, Exception inner) : base(message, inner) { Errors = new List<ValidationError> { error }; }
        public ValidationErrorException(ValidationError error) : this(error.Message, error) { }
        public ValidationErrorException(ValidationError error, Exception inner) : this(error.Message, error, inner) { }
        public ValidationErrorException(List<ValidationError> errors) : this(errors[0].Message, errors) { }
        public ValidationErrorException(List<ValidationError> errors, Exception inner) : this(errors[0].Message, errors, inner) { }

        public List<ValidationError> Errors { get; set; }
    }
}
