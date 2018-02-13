using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Element.Domain
{
    public class FooterElementSettings : ElementSettings
    {
        public bool ShowLoggedOnUserOptions { get; set; }
        public bool ShowLoggedOffUserOptions { get; set; }
        public string Message { get; set; }
    }

    public class FooterElementView
    {
        public FooterElementSettings Settings { get; set; }
    }

    public interface IFooterElementService : IElementSettingsService<FooterElementSettings>
    {
        Task<FooterElementView> GetElementViewAsync(long tenantId, long elementId, long pageId);
    }

    public class FooterElementService : IFooterElementService
    {
        private readonly IElementRepository<FooterElementSettings> _elementRepository;

        public FooterElementService(IElementRepository<FooterElementSettings> elementRepository)
        {
            _elementRepository = elementRepository;
        }

        public Task<FooterElementSettings> ReadElementSettingsAsync(long tenantId, long elementId)
        {
            return _elementRepository.ReadElementSettingsAsync(tenantId, elementId);
        }

        public async Task<FooterElementView> GetElementViewAsync(long tenantId, long elementId, long pageId)
        {
            FooterElementSettings elementSettings = await _elementRepository.ReadElementSettingsAsync(tenantId, elementId);
            FooterElementView elementView = new FooterElementView
            {
                Settings = elementSettings
            };
            return elementView;
        }
    }
}
