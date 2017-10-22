using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Elements.Resources;

namespace Riverside.Cms.Elements.Forums
{
    public class CreatePostInfo
    {
        public long TenantId { get; set; }
        public long ElementId { get; set; }
        public long ThreadId { get; set; }
        public long? ParentPostId { get; set; }
        public long UserId { get; set; }

        [Display(ResourceType = typeof(ElementResource), Name = "ForumMessageLabel")]
        [Required(ErrorMessageResourceType = typeof(ElementResource), ErrorMessageResourceName = "ForumMessageRequiredMessage")]
        public string Message { get; set; }
    }
}
