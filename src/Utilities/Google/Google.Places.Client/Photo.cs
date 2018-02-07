using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Utilities.Google.Places.Client
{
    public class Photo
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public List<string> HtmlAttributions { get; set; }
        public string PhotoReference { get; set; }
    }
}
