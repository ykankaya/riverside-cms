using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Riverside.Cms.Services.Element.Domain
{
    public class HtmlElementSettings : ElementSettings
    {
        public string Html { get; set; }
    }

    public class HtmlElementContent : IElementContent
    {
        public string FormattedHtml { get; set; }
    }

    public interface IHtmlElementService : IElementSettingsService<HtmlElementSettings>, IElementContentService<HtmlElementContent>
    {
    }

    public class HtmlElementService : IHtmlElementService
    {
        private readonly IElementRepository<HtmlElementSettings> _elementRepository;

        public HtmlElementService(IElementRepository<HtmlElementSettings> elementRepository)
        {
            _elementRepository = elementRepository;
        }

        public Task<HtmlElementSettings> ReadElementSettingsAsync(long tenantId, long elementId)
        {
            return _elementRepository.ReadElementSettingsAsync(tenantId, elementId);
        }

        private string FormatHtml(string html)
        {
            return html.Replace("%YEAR%", DateTime.UtcNow.Year.ToString());
        }

        public async Task<HtmlElementContent> ReadElementContentAsync(long tenantId, long elementId, long pageId)
        {
            HtmlElementSettings elementSettings = await _elementRepository.ReadElementSettingsAsync(tenantId, elementId);
            return new HtmlElementContent
            {
                FormattedHtml = FormatHtml(elementSettings.Html)
            };
        }
    }
}
