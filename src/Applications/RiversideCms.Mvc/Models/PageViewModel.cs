using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Riverside.Cms.Services.Core.Client;

namespace RiversideCms.Mvc.Models
{
    public class PageViewModel
    {
        public MasterPage MasterPage { get; set; }
        public Page Page { get; set; }

        public List<PageZoneViewModel> PageZoneViewModels { get; set; }
    }
}
