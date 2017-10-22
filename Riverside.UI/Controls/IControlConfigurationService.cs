using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.UI.Controls
{
    /// <summary>
    /// Interface for services that provide configuration settings for portal controls.
    /// </summary>
    public interface IControlConfigurationService
    {
        /// <summary>
        /// Gets HttpContext item key that is used to store breadcumbs collections.
        /// </summary>
        /// <returns>Breadcrumbs prefix.</returns>
        string GetBreadcrumbsKey();
    }
}
