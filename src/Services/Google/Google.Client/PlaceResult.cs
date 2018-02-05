using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Cms.Services.Google.Client
{
    public class PlaceResult
    {
        public double Rating { get; set; }
        public Geometry Geometry { get; set; }
        public int UtcOffset { get; set; }
        public List<AddressComponent> AddressComponents { get; set; }
        public List<Photo> Photos { get; set; }
        public List<PlaceReview> Reviews { get; set; }
        public OpeningHours OpeningHours { get; set; }
        public string AdrAddress { get; set; }
        public string FormattedAddress { get; set; }
        public string FormattedPhoneNumber { get; set; }
        public string Icon { get; set; }
        public string Id { get; set; }
        public string InternationalPhoneNumber { get; set; }
        public string Name { get; set; }
        public string PlaceId { get; set; }
        public string Reference { get; set; }
        public string Scope { get; set; }
        public string Url { get; set; }
        public string Vicinity { get; set; }
        public string Website { get; set; }
        public List<string> Types { get; set; }
    }
}
