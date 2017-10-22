using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Riverside.Cms.Elements.Carousels
{
    public class CarouselSlideViewModel
    {
        [JsonProperty(PropertyName = "carouselSlideId")]
        public string CarouselSlideId { get; set; }

        [JsonProperty(PropertyName = "thumbnailImageUploadId")]
        public string ThumbnailImageUploadId { get; set; }

        [JsonProperty(PropertyName = "previewImageUploadId")]
        public string PreviewImageUploadId { get; set; }

        [JsonProperty(PropertyName = "imageUploadId")]
        public string ImageUploadId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "pageId")]
        public string PageId { get; set; }

        [JsonProperty(PropertyName = "pageText")]
        public string PageText { get; set; }

        [JsonProperty(PropertyName = "buttonUrl")]
        public string ButtonUrl { get; set; }

        [JsonProperty(PropertyName = "buttonText")]
        public string ButtonText { get; set; }

        [JsonProperty(PropertyName = "imageUrl")]
        public string ImageUrl { get; set; }
    }
}
