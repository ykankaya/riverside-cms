using System;
using System.Collections.Generic;
using System.Text;
using Riverside.UI.Routes;
using Riverside.Utilities.Data;

namespace Riverside.UI.Grids
{
    /// <summary>
    /// Services that implement this interface create grid view models from business models.
    /// </summary>
    public interface IGridService
    {
        /// <summary>
        /// Returns grid view model given search parameters and a search result.
        /// </summary>
        /// <typeparam name="TItem">The type of business model found in the search result.</typeparam>
        /// <param name="searchParameters">Search parameters.</param>
        /// <param name="searchResult">Search result.</param>
        /// <param name="properties">Model properties that determine grid column headers.</param>
        /// <param name="urlParameters">URL parameters determining hyperlinks found in first grid column.</param>
        /// <param name="routePropertyPairs">Determines route values dynamically extracted from business models.</param>
        /// <param name="noItemsMessage">String that is displayed when search result contains no items.</param>
        /// <returns>Grid of items.</returns>
        Grid GetGrid<TItem>(ISearchParameters searchParameters, ISearchResult<TItem> searchResult, List<string> properties, UrlParameters urlParameters, List<RoutePropertyPair> routePropertyPairs, string noItemsMessage);
    }
}
