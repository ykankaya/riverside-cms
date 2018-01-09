using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Services.Element.Client
{
    public class ElementClientException : Exception
    {
        public ElementClientException() { }
        public ElementClientException(string message) : base(message) { }
        public ElementClientException(string message, Exception inner) : base(message, inner) { }
    }
}
