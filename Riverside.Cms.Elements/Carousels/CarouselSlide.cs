using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Riverside.Cms.Elements.Carousels
{
    public class CarouselSlide
    {
        public long TenantId { get; set; }
        public long ElementId { get; set; }
        public long CarouselSlideId { get; set; }
        public long ImageTenantId { get; set; }
        public long ThumbnailImageUploadId { get; set; }
        public long PreviewImageUploadId { get; set; }
        public long ImageUploadId { get; set; }
        public string Description { get; set; }
        public long? PageTenantId { get; set; }
        public long? PageId { get; set; }
        public string PageText { get; set; }
        public int SortOrder { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
