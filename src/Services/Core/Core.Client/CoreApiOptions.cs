using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Services.Core.Client
{
    public class CoreApiOptions
    {
        public string ApiBaseUrl { get; set; }

        public string ApiPageUrl { get; set; }
        public string ApiPageZonesUrl { get; set; }
        public string ApiPageZoneUrl { get; set; }
        public string ApiPageZoneElementsUrl { get; set; }
        public string ApiPageZoneElementUrl { get; set; }

        public string ApiMasterPageUrl { get; set; }
        public string ApiMasterPageZonesUrl { get; set; }
        public string ApiMasterPageZoneUrl { get; set; }
        public string ApiMasterPageZoneElementsUrl { get; set; }
        public string ApiMasterPageZoneElementUrl { get; set; }
    }
}
