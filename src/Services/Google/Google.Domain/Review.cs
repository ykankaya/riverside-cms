using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Services.Google.Domain
{
    public class Review
    {
        public long TenantId { get; set; }

        public long RatingId { get; set; }
        public DateTime Time { get; set; }
        public int Rating { get; set; }
        public string AuthorName { get; set; }
        public string AuthorUrl { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public string Text { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
