using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Pages;

namespace Riverside.Cms.Core.Elements
{
    public interface IPageLink
    {
        Page Page { get; set; }
        string LinkText { get; set; }
    }
}
