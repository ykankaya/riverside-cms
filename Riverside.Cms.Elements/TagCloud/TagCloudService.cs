using System;
using System.Collections.Generic;
using System.Linq;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Elements.Resources;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.TagCloud
{
    public class TagCloudService : IAdvancedElementService
    {
        private IPagePortalService _pagePortalService;
        private IPageService _pageService;
        private ITagCloudRepository _tagCloudRepository;

        public TagCloudService(IPagePortalService pagePortalService, IPageService pageService, ITagCloudRepository tagCloudRepository)
        {
            _pagePortalService = pagePortalService;
            _pageService = pageService;
            _tagCloudRepository = tagCloudRepository;
        }

        public Guid ElementTypeId
        {
            get
            {
                return new Guid("b910c231-7dbd-4cad-92ef-775981e895b4");
            }
        }

        public IElementSettings New(long tenantId)
        {
            return new TagCloudSettings { TenantId = tenantId, ElementTypeId = ElementTypeId, NoTagsMessage = ElementResource.TagCloudDefaultNoTagsMessage };
        }

        public IElementInfo NewInfo(IElementSettings settings, IElementContent content)
        {
            return new ElementInfo<TagCloudSettings, TagCloudContent> { Settings = settings, Content = content };
        }

        public void Create(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _tagCloudRepository.Create((TagCloudSettings)settings, unitOfWork);
        }

        public void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null)
        {
            _tagCloudRepository.Copy(sourceTenantId, sourceElementId, destTenantId, destElementId, unitOfWork);
        }

        public void Read(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _tagCloudRepository.Read((TagCloudSettings)settings, unitOfWork);
        }

        public void Update(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _tagCloudRepository.Update((TagCloudSettings)settings, unitOfWork);
        }

        public void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null)
        {
            _tagCloudRepository.Delete(tenantId, elementId, unitOfWork);
        }

        public IElementContent GetContent(IElementSettings settings, IPageContext pageContext, IUnitOfWork unitOfWork = null)
        {
            // Get tag cloud content
            TagCloudContent content = new TagCloudContent { PartialViewName = "TagCloud" };

            // Get folder that tag cloud is targetting
            TagCloudSettings tagCloudSettings = (TagCloudSettings)settings;
            long tenantId = tagCloudSettings.PageTenantId ?? pageContext.Page.TenantId;
            long pageId = tagCloudSettings.PageId ?? pageContext.Page.PageId;
            if (pageId == pageContext.Page.PageId)
                content.Page = pageContext.Page;
            else
                content.Page = _pageService.Read(tenantId, pageId, unitOfWork);

            // Get tagged list
            content.TaggedList = new List<TagTagged>();
            foreach (Tag tag in pageContext.Tags)
            {
                IList<Tag> tags = pageContext.Tags.Where(t => t.Name != tag.Name).ToList();
                content.TaggedList.Add(new TagTagged
                {
                    Tag = tag,
                    RemoveTaggedList = _pagePortalService.GetTagsAsTextString(TagVariables.Separator, tags)
                });
            }

            // Get tags list if no tags currently selected
            if (content.TaggedList.Count == 0)
                content.TagList = _pageService.ListTags(tenantId, pageId, tagCloudSettings.Recursive, unitOfWork);
            else
                content.TagList = new List<TagCount>();

            // Get related tags list if tags currently selected
            if (content.TaggedList.Count > 0)
            {
                content.RelatedTagList = _pageService.ListTaggedPageTags(tenantId, pageId, pageContext.Tags, tagCloudSettings.Recursive, unitOfWork);
                content.Tags = _pagePortalService.GetTagsAsTextString(TagVariables.Separator, pageContext.Tags);
            }
            else
            {
                content.RelatedTagList = new List<TagCount>();
            }

            // Return result
            return content;
        }
    }
}
