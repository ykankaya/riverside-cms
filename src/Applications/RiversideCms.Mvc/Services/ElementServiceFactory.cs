using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Riverside.Cms.Services.Element.Client;

namespace RiversideCms.Mvc.Services
{
    public class ElementServiceFactory : IElementServiceFactory
    {
        private readonly ICodeSnippetElementService _codeSnippetElementService;
        private readonly IFooterElementService _footerElementService;
        private readonly IHtmlElementService _htmlElementService;
        private readonly IPageHeaderElementService _pageHeaderElementService;
        private readonly IShareElementService _shareElementService;

        public ElementServiceFactory(ICodeSnippetElementService codeSnippetElementService, IFooterElementService footerElementService, IHtmlElementService htmlElementService, IPageHeaderElementService pageHeaderElementService, IShareElementService shareElementService)
        {
            _codeSnippetElementService = codeSnippetElementService;
            _footerElementService = footerElementService;
            _htmlElementService = htmlElementService;
            _pageHeaderElementService = pageHeaderElementService;
            _shareElementService = shareElementService;
        }

        public async Task<IElementSettings> ReadElementSettingsAsync(long tenantId, Guid elementTypeId, long elementId)
        {
            switch (elementTypeId.ToString())
            {
                case "5401977d-865f-4a7a-b416-0a26305615de":
                    return await _codeSnippetElementService.ReadElementSettingsAsync(tenantId, elementId);

                case "f1c2b384-4909-47c8-ada7-cd3cc7f32620":
                    return await _footerElementService.ReadElementSettingsAsync(tenantId, elementId);

                case "c92ee4c4-b133-44cc-8322-640e99c334dc":
                    return await _htmlElementService.ReadElementSettingsAsync(tenantId, elementId);

                case "1cbac30c-5deb-404e-8ea8-aabc20c82aa8":
                    return await _pageHeaderElementService.ReadElementSettingsAsync(tenantId, elementId);

                case "cf0d7834-54fb-4a6e-86db-0f238f8b1ac1":
                    return await _shareElementService.ReadElementSettingsAsync(tenantId, elementId);

                default:
                    return null;
            }
        }

        public async Task<object> ReadElementContentAsync(long tenantId, Guid elementTypeId, long elementId, long pageId)
        {
            switch (elementTypeId.ToString())
            {
                case "5401977d-865f-4a7a-b416-0a26305615de": // Code snippet
                    return null;

                case "f1c2b384-4909-47c8-ada7-cd3cc7f32620":
                    return await _footerElementService.ReadElementContentAsync(tenantId, elementId, pageId);

                case "c92ee4c4-b133-44cc-8322-640e99c334dc":
                    return await _htmlElementService.ReadElementContentAsync(tenantId, elementId, pageId);

                case "1cbac30c-5deb-404e-8ea8-aabc20c82aa8":
                    return await _pageHeaderElementService.ReadElementContentAsync(tenantId, elementId, pageId);

                case "cf0d7834-54fb-4a6e-86db-0f238f8b1ac1":
                    return await _shareElementService.ReadElementContentAsync(tenantId, elementId, pageId);

                default:
                    return null;
            }
        }
    }
}
