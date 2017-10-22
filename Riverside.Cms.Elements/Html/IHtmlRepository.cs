using Riverside.Utilities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Html
{
    public interface IHtmlRepository
    {
        void Create(HtmlSettings settings, IUnitOfWork unitOfWork = null);
        long CreateUpload(HtmlUpload upload, IUnitOfWork unitOfWork = null);
        void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null);
        void Read(HtmlSettings settings, IUnitOfWork unitOfWork = null);
        HtmlUpload ReadUpload(long tenantId, long elementId, long htmlUploadId, IUnitOfWork unitOfWork = null);
        void Update(HtmlSettings settings, IUnitOfWork unitOfWork = null);
        void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null);
    }
}
