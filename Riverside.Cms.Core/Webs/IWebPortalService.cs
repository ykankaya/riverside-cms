using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Administration;
using Riverside.UI.Grids;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Webs
{
    /// <summary>
    /// Interface for services that implement website related portal functionality.
    /// </summary>
    public interface IWebPortalService
    {
        /// <summary>
        /// Gets view model for website search.
        /// </summary>
        /// <param name="page">1-based page index (null if not specified).</param>
        /// <param name="search">Search terms.</param>
        /// <returns>View model for website search.</returns>
        AdminPageViewModel<Grid> GetSearchViewModel(int? page, string search);

        /// <summary>
        /// Gets search grid.
        /// </summary>
        /// <param name="page">1-based page index (null if not specified).</param>
        /// <param name="search">Search terms.</param>
        /// <returns>Grid view model.</returns>
        Grid GetSearchGrid(int? page, string search);

        /// <summary>
        /// Gets view model for website create.
        /// </summary>
        /// <returns>Form model for website create.</returns>
        AdminPageViewModel<WebViewModel> GetCreateViewModel();

        /// <summary>
        /// Handles create website form submission.
        /// </summary>
        /// <param name="viewModel">View model containing new website details.</param>
        /// <returns>View model indicating whether or not action was successful.</returns>
        WebViewModel PostCreateViewModel(WebViewModel viewModel);

        /// <summary>
        /// Get view model for website read, update or delete.
        /// </summary>
        /// <param name="tenantId">Identifies website whose details are returned.</param>
        /// <param name="action">The SCRUD action being performed.</param>
        /// <returns>View model for website read, update or delete.</returns>
        WebViewModel ReadUpdateDelete(long tenantId, DataAction action);
    }
}
