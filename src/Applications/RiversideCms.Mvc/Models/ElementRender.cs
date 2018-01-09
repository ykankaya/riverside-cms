using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Riverside.Cms.Services.Element.Client;

namespace RiversideCms.Mvc.Models
{
    public class ElementRender
    {
        public string PartialViewName { get; set; }
        public ElementView ElementView { get; set; }
    }
}
