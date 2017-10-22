using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;
using Riverside.UI.Routes;
using Riverside.Utilities.Data;

namespace Riverside.UI.Controls
{
    /// <summary>
    /// View model required by grids.
    /// </summary>
    /// <typeparam name="TItem">The type of data underlying grid rows.</typeparam>
    public class Grid<TItem>
    {
        /// <summary>
        /// Search results that are displayed within the grid control.
        /// </summary>
        [JsonProperty(PropertyName = "searchResult")]
        public ISearchResult<TItem> SearchResult { get; set; }

        /// <summary>
        /// Search parameters determining paging.
        /// </summary>
        [JsonProperty(PropertyName = "searchParameters")]
        public ISearchParameters SearchParameters { get; set; }

        /// <summary>
        /// Columns displayed on grid.
        /// </summary>
        [JsonProperty(PropertyName = "columns")]
        public IEnumerable<IGridColumn> Columns { get; set; }

        /// <summary>
        /// Action buttons that are rendered along top of grid.
        /// </summary>
        [JsonProperty(PropertyName = "buttons")]
        public List<Button> Buttons { get; set; }

        /// <summary>
        /// URL parameters that determine search and paging links.
        /// </summary>
        public UrlParameters UrlParameters { get; set; }

        /// <summary>
        /// Message that is displayed when grid is empty. Set null for the default empty message.
        /// </summary>
        [JsonProperty(PropertyName = "noItemsMessage")]
        public string NoItemsMessage { get; set; }

        /// <summary>
        /// Grid search terms.
        /// </summary>
        [JsonProperty(PropertyName = "search")]
        [Display(ResourceType = typeof(GridResource), Name = "SearchLabel")]
        [StringLength(50, ErrorMessageResourceType = typeof(GridResource), ErrorMessageResourceName = "SearchMaxLengthMessage")]
        public string Search { get; set; }
    }
}
