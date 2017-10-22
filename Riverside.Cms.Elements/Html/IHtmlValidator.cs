using Riverside.Cms.Core.Uploads;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Html
{
    public interface IHtmlValidator
    {
        Size ValidatePrepareImages(long tenantId, long masterPageId, CreateUploadModel model, string keyPrefix = null);
    }
}
