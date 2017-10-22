using System;
using System.Collections.Generic;
using System.Text;
using Riverside.UI.Routes;

namespace Riverside.UI.Controls
{
    /// <summary>
    /// Interface for services that return collections of grid columns for display in a grid.
    /// </summary>
    public interface IGridColumnService
    {
        /// <summary>
        /// Returns collection of grid columns for the type of data underlying grid rows whose properties are supplied.
        /// </summary>
        /// <param name="propertyNames">Names of properties whose columns are returned.</param>
        /// <returns>Grid column collection.</returns>
        /// <typeparam name="TItem">The type of data underlying grid rows.</typeparam>
        IEnumerable<IGridColumn> GetGridColumns<TItem>(params string[] propertyNames);

        /// <summary>
        /// Returns collection of grid columns for the type of data underlying grid rows. Ccolumns are returned for all properties belonging to the underlying data.
        /// </summary>
        /// <returns>Grid column collection.</returns>
        /// <typeparam name="TItem">The type of data underlying grid rows.</typeparam>
        IEnumerable<IGridColumn> GetGridColumns<TItem>();

        /// <summary>
        /// Retrieves collection of grid columns where first grid column contains hyperlink.
        /// </summary>
        /// <param name="itemUrlParameters">Determines URL hyperlink on first grid column.</param>
        /// <param name="routePropertyPairs">Holds list of (property name, route value names) pairs that are used to populate URL hyperlink on first grid column.</param>
        /// <param name="propertyNames">Names of properties whose columns are returned.</param>
        /// <returns>Grid column collection.</returns>
        /// <typeparam name="TItem">The type of data underlying grid rows.</typeparam>
        IEnumerable<IGridColumn> GetGridColumns<TItem>(UrlParameters itemUrlParameters, List<RoutePropertyPair> routePropertyPairs, params string[] propertyNames);
    }
}
