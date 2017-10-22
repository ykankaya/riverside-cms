using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Type of exception that may be thrown when a known fault condition arrises within a form factory.
    /// </summary>
    public class FormFactoryException : Exception
    {
        public FormFactoryException() { }
        public FormFactoryException(string message) : base(message) { }
        public FormFactoryException(string message, Exception inner) : base(message, inner) { }
    }
}
