using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Services.Google.Client
{
    public class PlaceReview
    {
        public string AuthorName { get; set; }
        public string AuthorUrl { get; set; }
        public string Language { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public string RelativeTimeDescription { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public int Time { get; set; }
    }
}
