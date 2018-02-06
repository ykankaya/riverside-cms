using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Services.Google.Places.Client
{
    public class OpeningHours
    {
        public bool OpenNow { get; set; }
        public List<Period> Periods { get; set; }
        public List<string> WeekdayText { get; set; }
    }
}
