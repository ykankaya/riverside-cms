using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using Riverside.Utilities.Data;
using Riverside.Utilities.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Riverside.UI.Web;

namespace Riverside.Cms.Elements.Shares
{
    public class ShareService : IAdvancedElementService
    {
        private IShareRepository _shareRepository;
        private IWebHelperService _webHelperService;

        public ShareService(IShareRepository shareRepository, IWebHelperService webHelperService)
        {
            _shareRepository = shareRepository;
            _webHelperService = webHelperService;
        }

        public Guid ElementTypeId
        {
            get
            {
                return new Guid("cf0d7834-54fb-4a6e-86db-0f238f8b1ac1");
            }
        }

        public IElementSettings New(long tenantId)
        {
            return new ShareSettings { TenantId = tenantId, ElementTypeId = ElementTypeId };
        }

        public IElementInfo NewInfo(IElementSettings settings, IElementContent content)
        {
            return new ElementInfo<ShareSettings, ShareContent> { Settings = settings, Content = content };
        }

        public void Create(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _shareRepository.Create((ShareSettings)settings, unitOfWork);
        }

        public void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null)
        {
            _shareRepository.Copy(sourceTenantId, sourceElementId, destTenantId, destElementId, unitOfWork);
        }

        public void Read(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _shareRepository.Read((ShareSettings)settings, unitOfWork);
        }

        public void Update(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _shareRepository.Update((ShareSettings)settings, unitOfWork);
        }

        public void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null)
        {
            _shareRepository.Delete(tenantId, elementId, unitOfWork);
        }

        public IElementContent GetContent(IElementSettings settings, IPageContext pageContext, IUnitOfWork unitOfWork = null)
        {
            // Construct element content
            ShareContent shareContent = new ShareContent();

            // Get this page's URL
            // UrlParameters urlParameters = new UrlParameters { RouteName = "ReadPage", RouteValues = new { pageid = pageContext.Page.PageId, description = pageContext.Page.Description } };
            string url = _webHelperService.GetRequestUrl(); // _webHelperService.GetUrl(urlParameters);

            // Populate element content
            shareContent.PartialViewName = "Share";
            shareContent.Description = _webHelperService.UrlEncode(pageContext.Page.Description ?? string.Empty);
            shareContent.Hashtags = _webHelperService.UrlEncode(string.Empty);
            shareContent.Image = _webHelperService.UrlEncode(string.Empty);
            shareContent.IsVideo = _webHelperService.UrlEncode(string.Empty);
            shareContent.Title = _webHelperService.UrlEncode(pageContext.Page.Name);
            shareContent.Url = _webHelperService.UrlEncode(url);

            // Return resulting element content
            return shareContent;
        }
    }
}
