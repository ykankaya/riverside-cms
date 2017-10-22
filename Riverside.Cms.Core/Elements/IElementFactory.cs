using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Core.Elements
{
    /// <summary>
    /// Interface for factories that create and retrieve models and services related to elements.
    /// </summary>
    public interface IElementFactory
    {
        /// <summary>
        /// Retrieves element service for a given element type.
        /// </summary>
        /// <param name="elementTypeId">Type of element.</param>
        /// <returns>Element service for the specified type of element.</returns>
        IBasicElementService GetElementService(Guid elementTypeId);

        /// <summary>
        /// Retrieves custom element validator for a given element type.
        /// </summary>
        /// <param name="elementTypeId">Type of element.</param>
        /// <returns>Custom element validator (or null if no custom element validator exists for the specified element type).</returns>
        ICustomElementValidator GetElementValidator(Guid elementTypeId);
    }
}
