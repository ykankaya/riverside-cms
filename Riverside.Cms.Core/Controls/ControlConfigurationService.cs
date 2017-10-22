using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.UI.Controls;

namespace Riverside.Cms.Core.Controls
{
    public class ControlConfigurationService : IControlConfigurationService
    {
        public string GetBreadcrumbsKey()
        {
            return "RiversideCmsBreadcrumbs";
        }
    }
}
