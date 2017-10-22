using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.UI.Controls
{
    /// <summary>
    /// Interface for services that provide access to breadcrumb collections.
    /// </summary>
    public interface IBreadcrumbService
    {
        /// <summary>
        /// Adds a new breadcrumb to the specified breadcrumb collection.
        /// </summary>
        /// <param name="breadcrumbsId">Identifies the breadcrumbs collection that the breadcrumb is added to.</param>
        /// <param name="breadcrumb">The breadcrumb that is added to the breadcrumbs collection.</param>
        void AddBreadcrumb(string breadcrumbsId, Breadcrumb breadcrumb);

        /// <summary>
        /// Adds a number of breadcrumbs to the specified breadcrumb collection.
        /// </summary>
        /// <param name="breadcrumbsId">Identifies the breadcrumbs collection that the breadcrumb is added to.</param>
        /// <param name="breadcrumbs">The breadcrumbs that are added to the breadcrumbs collection.</param>
        void AddBreadcrumbs(string breadcrumbsId, params Breadcrumb[] breadcrumbs);

        /// <summary>
        /// Retrieves list of navigation breadcrumbs for the specified breadcrumb collection.
        /// </summary>
        /// <param name="breadcrumbsId">Identifies the breadcrumbs collection that is returned.</param>
        /// <returns>Breadcrumb navigation.</returns>
        List<Breadcrumb> GetBreadcrumbs(string breadcrumbsId);
    }
}
