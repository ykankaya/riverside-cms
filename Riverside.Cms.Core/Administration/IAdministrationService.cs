using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.MasterPages;
using Riverside.Cms.Core.Pages;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Core.Administration
{
    public interface IAdministrationService
    {
        IPageContext GetUpdateThemeContext(long tenantId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null);
        IPageContext GetCreatePageContext(long tenantId, long masterPageId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null);
        IPageContext GetUpdatePageContext(long tenantId, long pageId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null);
        IPageContext GetUpdatePageElementContext(long tenantId, long pageId, long elementId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null);
        IPageContext GetUpdateMasterPageElementContext(long tenantId, long masterPageId, long elementId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null);
        IPageContext GetUpdatePageZoneContext(long tenantId, long pageId, long pageZoneId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null);
        IPageContext GetCreateUserContext(long tenantId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null);
        IPageContext GetConfirmUserSetPasswordContext(long tenantId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null);
        IPageContext GetConfirmUserContext(long tenantId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null);
        IPageContext GetLogonUserContext(long tenantId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null);
        IPageContext GetChangePasswordContext(long tenantId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null);
        IPageContext GetUpdateUserContext(long tenantId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null);
        IPageContext GetForgottenPasswordContext(long tenantId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null);
        IPageContext GetResetPasswordContext(long tenantId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null);
        IPageContext GetPageContext(long tenantId, long pageId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null);
        IPageContext GetCreateMasterPageContext(long tenantId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null);
        IPageContext GetUpdateMasterPageContext(long tenantId, long masterPageId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null);
        IPageContext GetUpdateMasterPageZoneContext(long tenantId, long masterPageId, long masterPageZoneId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null);
        IPageContext GetUpdateMasterPageZonesContext(long tenantId, long masterPageId, IDictionary<string, string> parameters, IList<Tag> tags, IUnitOfWork unitOfWork = null);
        IAdministrationOptions GetAdministrationOptions(IPageContext pageContext, List<IElementInfo> pageElements, List<IElementInfo> masterElements, List<PageZone> configurablePageZones, IUnitOfWork unitOfWork = null);
    }
}
