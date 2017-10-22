using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Elements.Forums;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.LatestThreads
{
    public class LatestThreadService : IAdvancedElementService
    {
        private IForumRepository _forumRepository;
        private ILatestThreadRepository _latestThreadRepository;

        public LatestThreadService(IForumRepository forumRepository, ILatestThreadRepository latestThreadRepository)
        {
            _forumRepository = forumRepository;
            _latestThreadRepository = latestThreadRepository;
        }

        public Guid ElementTypeId
        {
            get
            {
                return new Guid("f9557287-ba01-48e3-9ab4-e2f4831933d0");
            }
        }

        public IElementSettings New(long tenantId)
        {
            return new LatestThreadSettings { TenantId = tenantId, ElementTypeId = ElementTypeId, PageSize = 10 };
        }

        public IElementInfo NewInfo(IElementSettings settings, IElementContent content)
        {
            return new ElementInfo<LatestThreadSettings, LatestThreadContent> { Settings = settings, Content = content };
        }

        public void Create(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _latestThreadRepository.Create((LatestThreadSettings)settings, unitOfWork);
        }

        public void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null)
        {
            _latestThreadRepository.Copy(sourceTenantId, sourceElementId, destTenantId, destElementId, unitOfWork);
        }

        public void Read(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _latestThreadRepository.Read((LatestThreadSettings)settings, unitOfWork);
        }

        public void Update(IElementSettings settings, IUnitOfWork unitOfWork = null)
        {
            _latestThreadRepository.Update((LatestThreadSettings)settings, unitOfWork);
        }

        public void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null)
        {
            _latestThreadRepository.Delete(tenantId, elementId, unitOfWork);
        }

        public IElementContent GetContent(IElementSettings settings, IPageContext pageContext, IUnitOfWork unitOfWork = null)
        {
            LatestThreadSettings latestThreadSettings = (LatestThreadSettings)settings;
            long tenantId = latestThreadSettings.PageTenantId ?? pageContext.Page.TenantId;
            long pageId = latestThreadSettings.PageId ?? pageContext.Page.PageId;
            ForumThreads threads = null;
            if (pageContext.Tags.Count == 0)
                threads = _forumRepository.ListLatestThreads(tenantId, pageId, latestThreadSettings.PageSize, latestThreadSettings.Recursive, unitOfWork);
            else
                threads = _forumRepository.ListTaggedLatestThreads(tenantId, pageId, latestThreadSettings.PageSize, pageContext.Tags, latestThreadSettings.Recursive, unitOfWork);
            return new LatestThreadContent { PartialViewName = "LatestThread", Threads = threads };
        }
    }
}
