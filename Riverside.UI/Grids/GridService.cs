using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Riverside.UI.Resources;
using Riverside.UI.Routes;
using Riverside.Utilities.Annotations;
using Riverside.Utilities.Data;

namespace Riverside.UI.Grids
{
    /// <summary>
    /// Services that implement this interface create grid view models from business models.
    /// </summary>
    public class GridService : IGridService
    {
        // Member variables
        private IDataAnnotationsService _dataAnnotationsService;

        /// <summary>
        /// Constructor sets dependent components.
        /// </summary>
        /// <param name="dataAnnotationsService">Data annotations service.</param>
        public GridService(IDataAnnotationsService dataAnnotationsService)
        {
            _dataAnnotationsService = dataAnnotationsService;
        }

        /// <summary>
        /// Uses route value property names and a model instance to create a route value dictionary.
        /// </summary>
        /// <typeparam name="TItem">The type of business model found in the search result.</typeparam>
        /// <param name="item">Business model from search result.</param>
        /// <param name="routePropertyPairs">Holds the names of route values and the corresponding names of object properties that are used to populate route values.</param>
        /// <returns>A populated RouteValueDictionary.</returns>
        private Dictionary<string, object> GetRouteValues<TItem>(TItem item, List<RoutePropertyPair> routePropertyPairs)
        {
            Dictionary<string, object> routeValues = new Dictionary<string, object>();
            foreach (RoutePropertyPair routePropertyPair in routePropertyPairs)
            {
                object value = item.GetType().GetProperty(routePropertyPair.PropertyName).GetValue(item);
                routeValues.Add(routePropertyPair.RouteValueName, value);
            }
            return routeValues;
        }

        /// <summary>
        /// Constructs a grid row from a model and a list of properties.
        /// </summary>
        /// <typeparam name="TItem">The type of business model found in the search result.</typeparam>
        /// <param name="item">Business model from search result.</param>
        /// <param name="properties">List of properties that will be used to construct grid row.</param>
        /// <param name="urlHelper">URL helper.</param>
        /// <param name="urlParameters">URL parameters.</param>
        /// <param name="routePropertyPairs">Holds the names of route values and the corresponding names of object properties that are used to populate route values.</param>
        /// <returns>GridRow.</returns>
        private GridRow GetGridRow<TItem>(TItem item, List<string> properties, UrlHelper urlHelper, UrlParameters urlParameters, List<RoutePropertyPair> routePropertyPairs)
        {
            GridRow gridRow = new GridRow { Cells = new List<IGridCell>() };
            for (int index = 0; index < properties.Count; index++)
            {
                string property = properties[index];
                IGridCell gridCell = null;
                PropertyInfo propertyInfo = item.GetType().GetProperty(property);
                object value = propertyInfo.GetValue(item);
                if (propertyInfo.PropertyType == typeof(string))
                    gridCell = new TextCell { Value = (string)value };
                if (index == 0 && urlParameters != null)
                    gridCell.Url = "#"; // TODO: urlHelper.GetUrl(urlParameters, GetRouteValues<TItem>(item, routePropertyPairs));
                gridRow.Cells.Add(gridCell);
            }
            return gridRow;
        }

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
        public Grid GetGrid<TItem>(ISearchParameters searchParameters, ISearchResult<TItem> searchResult, List<string> properties, UrlParameters urlParameters, List<RoutePropertyPair> routePropertyPairs, string noItemsMessage)
        {
            // Construct grid object
            Grid grid = new Grid
            {
                EmptyMessage = searchResult.Items.Count() == 0 ? noItemsMessage : null,
                Headers = new List<GridHeader>(),
                Rows = new List<GridRow>(),
                Search = searchParameters.Search,
            };

            // Grid pager
            int pageCount = ((searchResult.Total - 1) / searchParameters.PageSize) + 1;
            if (pageCount > 1)
            {
                grid.Pager = new Pager
                {
                    FirstButtonLabel = PagerResource.FirstButtonLabel,
                    PreviousButtonLabel = PagerResource.PreviousButtonLabel,
                    NextButtonLabel = PagerResource.NextButtonLabel,
                    LastButtonLabel = PagerResource.LastButtonLabel,
                    PageCount = pageCount,
                    Summary = string.Format(PagerResource.PageAndCountText, searchParameters.PageIndex, pageCount),
                    Page = searchParameters.PageIndex + 1
                };
            }

            // Populate headers
            foreach (string property in properties)
                grid.Headers.Add(new GridHeader { Label = _dataAnnotationsService.GetPropertyDisplayName<TItem>(property) });

            // Populate rows
            UrlHelper urlHelper = null;// new UrlHelper();
            foreach (TItem item in searchResult.Items)
                grid.Rows.Add(GetGridRow(item, properties, urlHelper, urlParameters, routePropertyPairs));

            // Return the result
            return grid;
        }
    }
}
