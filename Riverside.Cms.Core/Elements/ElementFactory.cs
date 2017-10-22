using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;
using Riverside.Utilities.Injection;
using Riverside.Utilities.Reflection;

namespace Riverside.Cms.Core.Elements
{
    /// <summary>
    /// Interface for factories that create and retrieve models and services related to elements.
    /// </summary>
    public class ElementFactory : IElementFactory
    {
        // Static member variables
        private static Dictionary<Guid, IBasicElementService> _elementServices;

        // Member variables
        private IInjectionService  _injectionService;
        private IListElementServices _listElementServices;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="injectionService">Creates types, injecting dependent components.</param>
        /// <param name="listElementServices">Retrieves element services.</param>
        public ElementFactory(IInjectionService injectionService, IListElementServices listElementServices)
        {
            _injectionService  = injectionService;
            _listElementServices = listElementServices;
        }

        /// <summary>
        /// Populates dictionary of element sevices keyed by element type ID. Looks for all types that implement IBasicElementService, creates instances of them and
        /// registers them against their element type IDs.
        /// </summary>
        /// <returns>Element services keyed by element type ID.</returns>
        private Dictionary<Guid, IBasicElementService> GetElementServices()
        { 
            Dictionary<Guid, IBasicElementService> elementServiceDict = new Dictionary<Guid, IBasicElementService>();
            List<Type> elementServiceTypes = _listElementServices.ListTypes();
            for (int index = 0; index < elementServiceTypes.Count; index++)
            {
                Type type = elementServiceTypes[index];
                IBasicElementService elementService = _injectionService.CreateType<IBasicElementService>(type);
                elementServiceDict.Add(elementService.ElementTypeId, elementService);
            }
            return elementServiceDict;
        }

        /// <summary>
        /// Retrieves element service for a given element type.
        /// </summary>
        /// <param name="elementTypeId">Type of element.</param>
        /// <returns>Element service for the specified type of element.</returns>
        public IBasicElementService GetElementService(Guid elementTypeId)
        {
            // Get local copy of reference to dictionary
            Dictionary<Guid, IBasicElementService> elementServices = _elementServices;

            // If reference does not exist, create the dictionary 
            if (elementServices == null)
            {
                elementServices = GetElementServices();
                _elementServices = elementServices;
            }

            // But we will continue to work with the local copy of reference to dictionary, which we is thread safe - throw exception if a element service does not exist
            if (!elementServices.ContainsKey(elementTypeId))
                throw new ElementFactoryException(string.Format("Unable to locate element service for element type id {0}", elementTypeId));

            // We have an element service for the specified element type id, so return it
            return elementServices[elementTypeId];
        }

        /// <summary>
        /// Retrieves custom element validator for a given element type.
        /// </summary>
        /// <param name="elementTypeId">Type of element.</param>
        /// <returns>Custom element validator (or null if no custom element validator exists for the specified element type).</returns>
        public ICustomElementValidator GetElementValidator(Guid elementTypeId)
        {
            return null;
        }
    }
}
