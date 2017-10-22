using System.ComponentModel.DataAnnotations;
using Riverside.Cms.Elements.Resources;

namespace Riverside.Cms.Elements.Forums
{
    public class UpdatePostInfo
    {
        public long TenantId { get; set; }
        public long ElementId { get; set; }
        public long ThreadId { get; set; }
        public long PostId { get; set; }
        public long UserId { get; set; }

        [Display(ResourceType = typeof(ElementResource), Name = "ForumMessageLabel")]
        [Required(ErrorMessageResourceType = typeof(ElementResource), ErrorMessageResourceName = "ForumMessageRequiredMessage")]
        public string Message { get; set; }
    }
}
