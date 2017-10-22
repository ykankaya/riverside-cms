using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Assets
{
    public interface IAssetRepository
    {
        void RegisterDeployment(long tenantId, string hostname, DateTime deployed, IUnitOfWork unitOfWork = null);
        AssetDeployment ReadDeployment(long tenantId, string hostname, IUnitOfWork unitOfWork = null);
    }
}
