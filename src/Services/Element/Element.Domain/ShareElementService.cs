using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Services.Core.Client;

namespace Riverside.Cms.Services.Element.Domain
{
    public class ShareElementSettings : ElementSettings
    {
        public string DisplayName { get; set; }
        public bool ShareOnDigg { get; set; }
        public bool ShareOnFacebook { get; set; }
        public bool ShareOnGoogle { get; set; }
        public bool ShareOnLinkedIn { get; set; }
        public bool ShareOnPinterest { get; set; }
        public bool ShareOnReddit { get; set; }
        public bool ShareOnStumbleUpon { get; set; }
        public bool ShareOnTumblr { get; set; }
        public bool ShareOnTwitter { get; set; }
    }

    public class ShareElementContent : IElementContent
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Via { get; set; }
        public string Hashtags { get; set; }
        public string Image { get; set; }
        public string IsVideo { get; set; }
    }

    public interface IShareElementService : IElementSettingsService<ShareElementSettings>, IElementContentService<ShareElementContent>
    {
    }

    public class ShareElementService : IShareElementService
    {
        private readonly IElementRepository<ShareElementSettings> _elementRepository;
        private readonly IPageService _pageService;

        public ShareElementService(IElementRepository<ShareElementSettings> elementRepository, IPageService pageService)
        {
            _elementRepository = elementRepository;
            _pageService = pageService;
        }

        public Task<ShareElementSettings> ReadElementSettingsAsync(long tenantId, long elementId)
        {
            return _elementRepository.ReadElementSettingsAsync(tenantId, elementId);
        }

        public async Task<ShareElementContent> ReadElementContentAsync(long tenantId, long elementId, long pageId)
        {
            ShareElementContent elementContent = new ShareElementContent();

            Page page = await _pageService.ReadPageAsync(tenantId, pageId);

            elementContent.Description = page.Description ?? string.Empty;
            elementContent.Hashtags = string.Empty;
            elementContent.Image = string.Empty;
            elementContent.IsVideo = string.Empty;
            elementContent.Title = page.Name;

            return elementContent;
        }
    }
}
