using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Services.Core.Client
{
    public class CoreClientException : Exception
    {
        public CoreClientException() { }
        public CoreClientException(string message) : base(message) { }
        public CoreClientException(string message, Exception inner) : base(message, inner) { }
    }
}
