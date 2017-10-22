using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Administration;
using Riverside.UI.Grids;

namespace Riverside.Cms.Core.MasterPages
{
    public interface IMasterPagePortalService
    {
        AdminPageViewModel<Grid> GetSearchViewModel(long tenantId, int? page, string search);
        Grid GetSearchGrid(long tenantId, int? page, string search);
        AdminPageViewModel<MasterPageViewModel> Read(long tenantId, long masterPageId);
    }
}
