using System;
using System.Collections.Generic;
using System.Text;
using Riverside.UI.Web;
using Riverside.Utilities.Http;

namespace Riverside.UI.Controls
{
    /// <summary>
    /// A service that provides access to breadcrumb collections.
    /// </summary>
    public class BreadcrumbService : IBreadcrumbService
    {
        // Member variables
        private IControlConfigurationService _controlConfigurationService;
        private IWebHelperService _webHelperService;

        // Constants
        private const string BreadcrumbsKey = "RiversideCmsBreadcrumbs";

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="controlConfigurationService">Provides configuration values for portal controls.</param>
        /// <param name="webHelperService">Provides access to HTTP-specific information about an individual HTTP request.</param>
        public BreadcrumbService(IControlConfigurationService controlConfigurationService, IWebHelperService webHelperService)
        {
            _controlConfigurationService = controlConfigurationService;
            _webHelperService = webHelperService;
        }

        /// <summary>
        /// Adds a new breadcrumb to the specified breadcrumb collection.
        /// </summary>
        /// <param name="breadcrumbsId">Identifies the breadcrumbs collection that the breadcrumb is added to.</param>
        /// <param name="breadcrumb">The breadcrumb that is added to the breadcrumbs collection.</param>
        public void AddBreadcrumb(string breadcrumbsId, Breadcrumb breadcrumb)
        {
            List<Breadcrumb> breadcrumbs = GetBreadcrumbs(breadcrumbsId);
            breadcrumbs.Add(breadcrumb);
        }

        /// <summary>
        /// Adds a number of breadcrumbs to the specified breadcrumb collection.
        /// </summary>
        /// <param name="breadcrumbsId">Identifies the breadcrumbs collection that the breadcrumb is added to.</param>
        /// <param name="breadcrumbs">The breadcrumbs that are added to the breadcrumbs collection.</param>
        public void AddBreadcrumbs(string breadcrumbsId, params Breadcrumb[] breadcrumbs)
        {
            if (breadcrumbs != null && breadcrumbs.Length > 0)
            {
                List<Breadcrumb> breadcrumbsById = GetBreadcrumbs(breadcrumbsId);
                foreach (Breadcrumb breadcrumb in breadcrumbs)
                    breadcrumbsById.Add(breadcrumb);
            }
        }

        /// <summary>
        /// Retrieves list of navigation breadcrumbs for the specified breadcrumb collection.
        /// </summary>
        /// <param name="breadcrumbsId">Identifies the breadcrumbs collection that is returned.</param>
        /// <returns>Breadcrumb navigation.</returns>
        public List<Breadcrumb> GetBreadcrumbs(string breadcrumbsId)
        {
            // Get dictionary containing lists of breadcumbs
            string key = _controlConfigurationService.GetBreadcrumbsKey();
            Dictionary<string, List<Breadcrumb>> breadcrumbsDict = _webHelperService.GetItem<Dictionary<string, List<Breadcrumb>>>(key);
            if (breadcrumbsDict == null)
            {
                breadcrumbsDict = new Dictionary<string, List<Breadcrumb>>();
                _webHelperService.SetItem<Dictionary<string, List<Breadcrumb>>>(key, breadcrumbsDict);
            }

            // Get list of breadcrumbs for the specified id
            if (!breadcrumbsDict.ContainsKey(breadcrumbsId))
                breadcrumbsDict.Add(breadcrumbsId, new List<Breadcrumb>());
            return breadcrumbsDict[breadcrumbsId];
        }
    }
}
