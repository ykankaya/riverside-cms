using System;
using System.Collections.Generic;
using System.Text;
using Riverside.Utilities.Injection;
using Riverside.Utilities.Reflection;

namespace Riverside.UI.Forms
{
    /// <summary>
    /// Locates form services given a form identifier.
    /// </summary>
    public class FormServiceFactory : IFormServiceFactory
    {
        // Static member variables
        private static Dictionary<Guid, IFormService> _formServices;

        // Member variables
        private IInjectionService _injectionService;
        private IListFormServices _listFormServices;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="injectionService">Creates types, injecting dependent components.</param>
        /// <param name="listFormServices">Used to find types of form services.</param>
        public FormServiceFactory(IInjectionService injectionService, IListFormServices listFormServices)
        {
            _injectionService = injectionService;
            _listFormServices = listFormServices;
        }

        /// <summary>
        /// Populates dictionary of form sevices keyed by form ID. Looks for all types that implement IFormService, creates instances of them and
        /// registers them against their form IDs.
        /// </summary>
        /// <returns>Form services keyed by form ID.</returns>
        private Dictionary<Guid, IFormService> GetFormServices()
        {
            Dictionary<Guid, IFormService> formServiceDict = new Dictionary<Guid, IFormService>();
            List<Type> formServiceTypes = _listFormServices.ListTypes();
            foreach (Type type in formServiceTypes)
            {
                IFormService formService = _injectionService.CreateType<IFormService>(type);
                formServiceDict.Add(formService.FormId, formService);
            }
            return formServiceDict;
        }

        /// <summary>
        /// Gets form service for the specified form identifier.
        /// </summary>
        /// <param name="formId">Form identifier.</param>
        /// <returns>Applicable form service.</returns>
        public IFormService GetFormService(string formId)
        {
            // Get local copy of reference to dictionary
            Dictionary<Guid, IFormService> formServices = _formServices;

            // If reference does not exist, create the dictionary 
            if (formServices == null)
            {
                formServices = GetFormServices();
                _formServices = formServices;
            }

            // But we will continue to work with the local copy of reference to dictionary, which we is thread safe - throw exception if a form service does not exist
            Guid formIdGuid = new Guid(formId);
            if (!formServices.ContainsKey(formIdGuid))
                throw new FormFactoryException(string.Format("Unable to locate form service for form id {0}", formId));

            // We have a form service for the specified form id, so return it
            return formServices[formIdGuid];
        }
    }
}
