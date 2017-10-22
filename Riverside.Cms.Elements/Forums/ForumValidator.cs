using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Validation;

namespace Riverside.Cms.Elements.Forums
{
    public class ForumValidator : IForumValidator
    {
        private IModelValidator _modelValidator;

        public ForumValidator(IModelValidator modelValidator)
        {
            _modelValidator = modelValidator;
        }

        public void ValidateCreateThread(CreateThreadInfo info)
        {
            _modelValidator.Validate(info);
        }

        public void ValidateCreatePost(CreatePostInfo info)
        {
            _modelValidator.Validate(info);
        }

        public void ValidateUpdateThread(UpdateThreadInfo info)
        {
            _modelValidator.Validate(info);
        }

        public void ValidateUpdatePost(UpdatePostInfo info)
        {
            _modelValidator.Validate(info);
        }

        public void ValidateDeleteThread(DeleteThreadInfo info)
        {
            // Nothing to do
        }
    }
}
