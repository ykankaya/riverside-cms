using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Assets
{
    public interface IAssetService
    {
        void RegisterDeployment(long tenantId, string hostname, IUnitOfWork unitOfWork = null);
        AssetDeployment ReadDeployment(long tenantId, string hostname, IUnitOfWork unitOfWork = null);
        void Deploy(long tenantId, string hostname, IUnitOfWork unitOfWork = null);
        string GetAssetStyleSheetPath(long tenantId);
        string GetFontOptionStyleSheetPath(long tenantId, string fontOption);
        string GetColourOptionStyleSheetPath(long tenantId, string colourOption);
        List<string> GetFontOptions(long tenantId);
        List<string> GetColourOptions(long tenantId);
    }
}
