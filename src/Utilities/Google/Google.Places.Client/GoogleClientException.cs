using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Utilities.Google.Places.Client
{
    public class GoogleClientException : Exception
    {
        public GoogleClientException() { }
        public GoogleClientException(string message) : base(message) { }
        public GoogleClientException(string message, Exception inner) : base(message, inner) { }
    }
}
