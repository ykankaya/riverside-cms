using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Riverside.UI.Controls;

namespace Riverside.Cms.Core.Administration
{
    public class NavigationViewModel
    {
        [JsonProperty(PropertyName = "breadcrumbs")]
        public List<NavigationItemViewModel> BreadcrumbItems { get; set; }

        [JsonProperty(PropertyName = "hamburgerItems")]
        public List<HamburgerItem> HamburgerItems { get; set; }

        [JsonProperty(PropertyName = "actions")]
        public List<NavigationItemViewModel> ActionItems { get; set; }
    }
}
