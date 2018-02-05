using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Services.Google.Client
{
    public class GoogleClientException : Exception
    {
        public GoogleClientException() { }
        public GoogleClientException(string message) : base(message) { }
        public GoogleClientException(string message, Exception inner) : base(message, inner) { }
    }
}
