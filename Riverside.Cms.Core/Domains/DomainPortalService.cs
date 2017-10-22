using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Domains;
using Riverside.Cms.Core.Resources;
using Riverside.Cms.Core.Controls;
using Riverside.UI.Controls;
using Riverside.UI.Routes;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Domains
{
    public class DomainPortalService : IDomainPortalService
    {
        private IDomainService _domainService;
        private IGridColumnService _gridColumnService;

        public DomainPortalService(IDomainService domainService, IGridColumnService gridColumnService)
        {
            _gridColumnService = gridColumnService;
            _domainService = domainService;
        }

        public Grid<Domain> Search(long tenantId, int? page, string search, bool modelStateValid)
        {
            // Construct grid object that will be returned
            Grid<Domain> grid = new Grid<Domain> { Search = search, NoItemsMessage = DomainResource.NoDomainsMessage };

            // Get search grid action buttons
            Button createDomainButton = new Button
            {
                Text = DomainResource.CreateDomainButtonLabel,
                Icon = Icon.Create,
                UrlParameters = new UrlParameters { ControllerName = "domains", ActionName = "create" }
            };
            grid.Buttons = new List<Button> { createDomainButton };

            // Grid paging URL parameters
            grid.UrlParameters = new UrlParameters
            {
                ControllerName = "domains",
                ActionName = "search",
                RouteValues = new { search = search }
            };

            // If model state valid, get list of domains that will be used to populate grid
            if (modelStateValid)
            {
                // Get domains to display in grid
                grid.SearchParameters = new SearchParameters
                {
                    PageIndex = page == null ? 0 : page.Value - 1,
                    PageSize = 10,
                    Search = search
                };
                grid.SearchResult = _domainService.Search(tenantId, grid.SearchParameters);

                // Set action that is performed when user clicks on domain in grid
                UrlParameters itemUrlParameters = new UrlParameters { ControllerName = "domains", ActionName = "update" };

                // Set domain property and route value that are used to render action links
                List<RoutePropertyPair> routePropertyPairs = new List<RoutePropertyPair> { new RoutePropertyPair { PropertyName = DomainPropertyNames.DomainId, RouteValueName = "domainid" } };

                // Get grid columns
                grid.Columns = _gridColumnService.GetGridColumns<Domain>(itemUrlParameters, routePropertyPairs, DomainPropertyNames.Url, DomainPropertyNames.RedirectUrl);
            }

            // Return the result
            return grid;
        }

        public DomainViewModel Create()
        {
            Button createButton = new Button
            {
                Text = DomainResource.CreateDomainTabLabel,
                Icon = Icon.Create,
                UrlParameters = new UrlParameters { ControllerName = "domains", ActionName = "create" },
                State = ButtonState.Active
            };

            return new DomainViewModel
            {
                Buttons = new List<Button> { createButton }
            };
        }

        private List<Button> GetActionButtons(long tenantId, long domainId, DataAction action)
        {
            Button readButton = new Button
            {
                Text = DomainResource.ReadDomainTabLabel,
                Icon = Icon.Read,
                UrlParameters = new UrlParameters { ControllerName = "domains", ActionName = "read", RouteValues = new { domainid = domainId } },
                State = action == DataAction.Read ? ButtonState.Active : ButtonState.Enabled
            };
            Button updateButton = new Button
            {
                Text = DomainResource.UpdateDomainTabLabel,
                Icon = Icon.Update,
                UrlParameters = new UrlParameters { ControllerName = "domains", ActionName = "update", RouteValues = new { domainid = domainId } },
                State = action == DataAction.Update ? ButtonState.Active : ButtonState.Enabled
            };
            Button deleteButton = new Button
            {
                Text = DomainResource.DeleteDomainTabLabel,
                Icon = Icon.Delete,
                UrlParameters = new UrlParameters { ControllerName = "domains", ActionName = "delete", RouteValues = new { domainid = domainId } },
                State = action == DataAction.Delete ? ButtonState.Active : ButtonState.Enabled
            };
            return new List<Button> { readButton, updateButton, deleteButton };
        }

        public DomainViewModel ReadUpdateDelete(long tenantId, long domainId, DataAction action)
        {
            return new DomainViewModel
            {
                Buttons = GetActionButtons(tenantId, domainId, action),
                Domain = _domainService.Read(tenantId, domainId)
            };
        }
    }
}
