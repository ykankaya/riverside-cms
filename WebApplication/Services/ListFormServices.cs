using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.MasterPages;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Themes;
using Riverside.Cms.Elements.Albums;
using Riverside.Cms.Elements.Carousels;
using Riverside.Cms.Elements.CodeSnippets;
using Riverside.Cms.Elements.Contacts;
using Riverside.Cms.Elements.Footers;
using Riverside.Cms.Elements.Forms;
using Riverside.Cms.Elements.Forums;
using Riverside.Cms.Elements.Html;
using Riverside.Cms.Elements.LatestThreads;
using Riverside.Cms.Elements.Maps;
using Riverside.Cms.Elements.NavBars;
using Riverside.Cms.Elements.PageHeaders;
using Riverside.Cms.Elements.PageList;
using Riverside.Cms.Elements.Pages;
using Riverside.Cms.Elements.Shares;
using Riverside.Cms.Elements.Tables;
using Riverside.Cms.Elements.TagCloud;
using Riverside.Cms.Elements.Testimonials;
using Riverside.UI.Forms;

namespace WebApplication.Services
{
    public class ListFormServices : IListFormServices
    {
        public List<Type> ListTypes()
        {
            return new List<Type>
            {
                typeof(AuthenticationFormService),
                typeof(ChangePasswordFormService),
                typeof(ConfirmUserFormService),
                typeof(ConfirmUserSetPasswordFormService),
                typeof(CreateUserFormService),
                typeof(ForgottenPasswordFormService),
                typeof(LogonUserFormService),
                typeof(ResetPasswordFormService),
                typeof(MasterPageFormService),
                typeof(MasterPageZoneFormService),
                typeof(MasterPageZonesFormService),
                typeof(PageFormService),
                typeof(ThemeFormService),
                typeof(AlbumAdminFormService),
                typeof(CarouselAdminFormService),
                typeof(CodeSnippetAdminFormService),
                typeof(ContactService),
                typeof(FooterAdminFormService),
                typeof(FormFormService),
                typeof(ForumFormService),
                typeof(HtmlAdminFormService),
                typeof(LatestThreadAdminFormService),
                typeof(MapAdminFormService),
                typeof(NavBarAdminFormService),
                typeof(PageHeaderAdminFormService),
                typeof(PageListAdminFormService),
                typeof(PageZoneFormService),
                typeof(ShareAdminFormService),
                typeof(TableService),
                typeof(TagCloudAdminFormService),
                typeof(TestimonialService)
            };
        }
    }
}
