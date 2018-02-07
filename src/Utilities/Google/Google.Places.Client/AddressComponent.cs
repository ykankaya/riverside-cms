using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Utilities.Google.Places.Client
{
    public class AddressComponent
    {
        public string LongName { get; set; }
        public string ShortName { get; set; }
        public List<string> Types { get; set; }
    }
}
