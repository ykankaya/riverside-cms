using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Elements.Resources;

namespace Riverside.Cms.Elements.Forums
{
    public class CreateThreadInfo
    {
        public long TenantId { get; set; }
        public long ElementId { get; set; }

        [Display(ResourceType = typeof(ElementResource), Name = "ForumSubjectLabel")]
        [Required(ErrorMessageResourceType = typeof(ElementResource), ErrorMessageResourceName = "ForumSubjectRequiredMessage")]
        [StringLength(ForumLengths.SubjectMaxLength, ErrorMessageResourceType = typeof(ElementResource), ErrorMessageResourceName = "ForumSubjectMaxLengthMessage")]
        public string Subject { get; set; }

        [Display(ResourceType = typeof(ElementResource), Name = "ForumNotifyLabel")]
        public bool Notify { get; set; }

        public long UserId { get; set; }

        [Display(ResourceType = typeof(ElementResource), Name = "ForumMessageLabel")]
        [Required(ErrorMessageResourceType = typeof(ElementResource), ErrorMessageResourceName = "ForumMessageRequiredMessage")]
        public string Message { get; set; }
    }
}
