using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.Footers
{
    public class FooterService : IAdvancedElementService
    {
        private IFooterRepository _footerRepository;

        public FooterService(IFooterRepository footerRepository)
        {
            _footerRepository = footerRepository;
        }

        public Guid ElementTypeId
        {
            get
            {
                return new Guid("f1c2b384-4909-47c8-ada7-cd3cc7f32620");
            }
        }

        public IElementSettings New(long tenantId)
        {
            return new FooterSettings { TenantId = tenantId, ElementTypeId = ElementTypeId };
        }

        public IElementInfo NewInfo(IElementSettings settings, IElementContent content)
        {
            return new ElementInfo<FooterSettings, FooterContent> { Settings = settings, Content = content };
        }

        public void Create(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _footerRepository.Create((FooterSettings)settings, unitOfWork);
        }

        public void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null)
        {
            _footerRepository.Copy(sourceTenantId, sourceElementId, destTenantId, destElementId, unitOfWork);
        }

        public void Read(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _footerRepository.Read((FooterSettings)settings, unitOfWork);
        }

        public void Update(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _footerRepository.Update((FooterSettings)settings, unitOfWork);
        }

        public void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null)
        {
            _footerRepository.Delete(tenantId, elementId, unitOfWork);
        }

        private string FormatMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return null;
            return message.Replace("%YEAR%", DateTime.UtcNow.Year.ToString());
        }

        public IElementContent GetContent(IElementSettings settings, IPageContext pageContext, IUnitOfWork unitOfWork = null)
        {
            // Construct element content
            FooterContent footerContent = new FooterContent();
            footerContent.PartialViewName = "Footer";

            // Populate element content according to element settings
            FooterSettings footerSettings = (FooterSettings)settings;
            footerContent.FormattedMessage = FormatMessage(footerSettings.Message);

            // Return resulting element content
            return footerContent;
        }
    }
}
