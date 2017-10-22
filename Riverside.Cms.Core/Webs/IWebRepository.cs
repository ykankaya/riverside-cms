using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Webs
{
    /// <summary>
    /// Interfaces for repositories that manage webs.
    /// </summary>
    public interface IWebRepository
    {
        /// <summary>
        /// Searches websites.
        /// </summary>
        /// <param name="parameters">Search and paging parameters.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Search result.</returns>
        ISearchResult<Web> Search(ISearchParameters parameters, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Creates a new website.
        /// </summary>
        /// <param name="web">New website details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Create(Web web, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Gets website details.
        /// </summary>
        /// <param name="tenantId">Identifies website whose details are returned.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Website details (or null if website not found).</returns>
        Web Read(long tenantId, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Gets website details.
        /// </summary>
        /// <param name="name">Name of website whose details are returned.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <returns>Website details (or null if website not found).</returns>
        Web ReadByName(string name, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Updates a website's details.
        /// </summary>
        /// <param name="web">Updated website details.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Update(Web web, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Update theme options.
        /// </summary>
        /// <param name="tenantId">Identifies website whose theme options are updated.</param>
        /// <param name="fontOption">Font option.</param>
        /// <param name="colourOption">Colour option.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void UpdateThemeOptions(long tenantId, string fontOption, string colourOption, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// Deletes a website.
        /// </summary>
        /// <param name="tenantId">Identifies website to delete.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        void Delete(long tenantId, IUnitOfWork unitOfWork = null);
    }
}
