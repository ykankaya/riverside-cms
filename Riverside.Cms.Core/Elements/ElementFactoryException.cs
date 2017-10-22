using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Elements
{
    /// <summary>
    /// Type of exception that may be thrown when a known fault condition arrises within an element factory.
    /// </summary>
    public class ElementFactoryException : Exception
    {
        public ElementFactoryException() { }
        public ElementFactoryException(string message) : base(message) { }
        public ElementFactoryException(string message, Exception inner) : base(message, inner) { }
    }
}
