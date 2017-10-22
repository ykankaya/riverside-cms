using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Elements.Html
{
    public interface IHtmlUrlService
    {
        string GetHtmlUploadUrl(long elementId, long uploadId);
    }
}
