using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Riverside.UI.Routes;
using Riverside.Utilities.Annotations;

namespace Riverside.UI.Controls
{
    /// <summary>
    /// Grid service that returns collections of grid columns for display in a grid.
    /// </summary>
    public class GridColumnService : IGridColumnService
    {
        // Member variables
        private IDataAnnotationsService _dataAnnotationsService;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="dataAnnotationsService">Data annotations helper service.</param>
        public GridColumnService(IDataAnnotationsService dataAnnotationsService)
        {
            _dataAnnotationsService = dataAnnotationsService;
        }

        /// <summary>
        /// Returns collection of grid columns for the type of data underlying grid rows whose properties are supplied.
        /// </summary>
        /// <param name="propertyNames">Names of properties whose columns are returned.</param>
        /// <returns>Grid column collection.</returns>
        /// <typeparam name="TItem">The type of data underlying grid rows.</typeparam>
        public IEnumerable<IGridColumn> GetGridColumns<TItem>(params string[] propertyNames)
        {
            Type itemType = typeof(TItem);
            List<IGridColumn> columns = new List<IGridColumn>();
            foreach (string propertyName in propertyNames)
            {
                PropertyInfo propertyInfo = itemType.GetProperty(propertyName);
                columns.Add(new GridColumn
                {
                    DisplayName = _dataAnnotationsService.GetPropertyDisplayName<TItem>(propertyName),
                    ItemPropertyName = propertyName
                });
            }
            return columns;
        }

        /// <summary>
        /// Returns collection of grid columns for the type of data underlying grid rows. Ccolumns are returned for all properties belonging to the underlying data.
        /// </summary>
        /// <returns>Grid column collection.</returns>
        /// <typeparam name="TItem">The type of data underlying grid rows.</typeparam>
        public IEnumerable<IGridColumn> GetGridColumns<TItem>()
        {
            return GetGridColumns<TItem>(typeof(TItem).GetProperties().Select(m => m.Name).ToArray());
        }

        /// <summary>
        /// Retrieves collection of grid columns where first grid column contains hyperlink.
        /// </summary>
        /// <param name="itemUrlParameters">Determines URL hyperlink on first grid column.</param>
        /// <param name="routePropertyPairs">Holds list of (property name, route value names) pairs that are used to populate URL hyperlink on first grid column.</param>
        /// <param name="propertyNames">Names of properties whose columns are returned.</param>
        /// <returns>Grid column collection.</returns>
        /// <typeparam name="TItem">The type of data underlying grid rows.</typeparam>
        public IEnumerable<IGridColumn> GetGridColumns<TItem>(UrlParameters itemUrlParameters, List<RoutePropertyPair> routePropertyPairs, params string[] propertyNames)
        {
            // Create list of columns
            List<IGridColumn> columns = new List<IGridColumn>();

            // Add URL grid column
            columns.Add(new GridUrlColumn
            {
                ItemPropertyName = propertyNames[0],
                DisplayName = _dataAnnotationsService.GetPropertyDisplayName<TItem>(propertyNames[0]),
                RoutePropertyPairs = routePropertyPairs,
                UrlParameters = itemUrlParameters
            });

            // Add the remaining grid columns
            columns.AddRange(GetGridColumns<TItem>(propertyNames.Skip(1).ToArray()));

            // Return the resulting list of columns
            return columns;
        }
    }
}
