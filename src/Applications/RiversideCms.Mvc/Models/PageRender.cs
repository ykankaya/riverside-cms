using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Riverside.Cms.Services.Core.Client;

namespace RiversideCms.Mvc.Models
{
    public class PageRender
    {
        public PageView View { get; set; }
        public Dictionary<long, ElementRender> Elements { get; set; }
    }
}
