using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riverside.Cms.Core.Administration;
using Riverside.Cms.Core.Assets;
using Riverside.Cms.Core.Authentication;
using Riverside.Cms.Core.Authorization;
using Riverside.Cms.Core.Data;
using Riverside.Cms.Core.Domains;
using Riverside.Cms.Core.Elements;
using Riverside.Cms.Core.MasterPages;
using Riverside.Cms.Core.Middleware;
using Riverside.Cms.Core.Pages;
using Riverside.Cms.Core.Templates;
using Riverside.Cms.Core.Tenants;
using Riverside.Cms.Core.Themes;
using Riverside.Cms.Core.Uploads;
using Riverside.Cms.Core.Webs;
using Riverside.Cms.Elements.Albums;
using Riverside.Cms.Elements.Authentication;
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
using Riverside.Cms.Elements.TestimonialCarousels;
using Riverside.Cms.Elements.Testimonials;
using Riverside.Cms.Elements.Themes;
using Riverside.UI.Forms;
using Riverside.UI.Grids;
using Riverside.UI.Web;
using Riverside.Utilities.Annotations;
using Riverside.Utilities.Data;
using Riverside.Utilities.Drawing;
using Riverside.Utilities.Http;
using Riverside.Utilities.Injection;
using Riverside.Utilities.Mail;
using Riverside.Utilities.Reflection;
using Riverside.Utilities.Security;
using Riverside.Utilities.Text;
using Riverside.Utilities.Validation;
using WebApplication.Services;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Register the IConfiguration instance which options bind to
            services.Configure<DbOptions>(Configuration);
            services.Configure<AzureStorageOptions>(Configuration);
            services.Configure<EmailOptions>(Configuration);

            // Authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.LoginPath = new PathString("/users/login"));

            // Register MVC services
            services.AddMvc();

            // ASP.NET core specific
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // Authentication
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IAuthenticationConfigurationService, AuthenticationConfigurationService>();
            services.AddTransient<IAuthenticationProviderService, CookieAuthenticationProviderService>();
            services.AddTransient<IAuthenticationValidator, AuthenticationValidator>();
            services.AddTransient<IAuthenticationUrlService, AuthenticationUrlService>();

            // Authorization
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddTransient<IAuthorizationRepository, SqlAuthorizationRepository>();
            services.AddTransient<IFunctionAuthorizer, FunctionAuthorizer>();

            // Administration
            services.AddTransient<IAdministrationService, AdministrationService>();
            services.AddTransient<IAdministrationPortalService, AdministrationPortalService>();

            // User interface
            services.AddTransient<IGridService, GridService>();

            // Templates
            services.AddTransient<ITemplateService, TemplateService>();
            services.AddTransient<ITemplateRepository, SqlTemplateRepository>();

            // Tenants
            services.AddTransient<ITenantRepository, SqlTenantRepository>();

            // Users
            services.AddTransient<IUserRepository, SqlUserRepository>();

            // Web services
            services.AddTransient<IWebService, WebService>();
            services.AddTransient<IWebRepository, SqlWebRepository>();
            services.AddTransient<IWebValidator, WebValidator>();

            // Domain services
            services.AddTransient<IDomainService, DomainService>();
            services.AddTransient<IDomainRepository, SqlDomainRepository>();
            services.AddTransient<IDomainValidator, DomainValidator>();

            // Master pages
            services.AddTransient<IMasterPageService, MasterPageService>();
            services.AddTransient<IMasterPageRepository, SqlMasterPageRepository>();
            services.AddTransient<IMasterPageValidator, MasterPageValidator>();

            // Pages
            services.AddTransient<Riverside.Cms.Core.Pages.IPageService, Riverside.Cms.Core.Pages.PageService>();
            services.AddTransient<IPageRepository, SqlPageRepository>();
            services.AddTransient<IPageValidator, PageValidator>();
            services.AddTransient<IPagePortalService, PagePortalService>();

            // Element services
            services.AddTransient<IElementService, ElementService>();
            services.AddTransient<IElementRepository, SqlElementRepository>();
            services.AddTransient<IElementFactory, ElementFactory>();
            services.AddTransient<IListElementServices, ListElementServices>();

            // Upload services
            services.AddTransient<IUploadService, UploadService>();
            services.AddTransient<IUploadRepository, SqlUploadRepository>();
            services.AddTransient<IUploadValidator, UploadValidator>();

            // Assets
            services.AddTransient<IAssetService, AssetService>();
            services.AddTransient<IAssetRepository, SqlAssetRepository>();

            // Storage
            services.AddTransient<IStorageService, AzureStorageService>();
            services.AddTransient<IAzureConfigurationService, AzureConfigurationService>();
            
            // Validation
            services.AddTransient<IModelValidator, ModelValidator>();

            // Data access
            services.AddTransient<IDatabaseManagerFactory, CmsDatabaseManagerFactory>();
            services.AddTransient<IUnitOfWorkFactory, UnitOfWorkFactory>();
            services.AddTransient<ISqlManager, SqlManager>();

            // Web helpers
            services.AddTransient<IWebHelperService, WebHelperService>();

            // Forms
            services.AddTransient<IFormHelperService, FormHelperService>();
            services.AddTransient<IFormServiceFactory, FormServiceFactory>();
            services.AddTransient<IListFormServices, ListFormServices>();

            // Utilities
            services.AddTransient<IDataAnnotationsService, DataAnnotationsService>();
            services.AddTransient<IEmailConfigurationService, EmailConfigurationService>();
            services.AddTransient<IEmailService, SmtpEmailService>();
            services.AddTransient<IEncodeDecodeService, WebHelperService>();
            services.AddTransient<IHtmlFormatService, HtmlFormatService>();
            services.AddTransient<IImageAnalysisService, ImageAnalysisService>();
            services.AddTransient<IInjectionService, InjectionService>();
            services.AddTransient<IReflectionService, ReflectionService>();
            services.AddTransient<ISecurityService, SecurityService>();
            services.AddTransient<IStringService, StringService>();

            // Albums
            services.AddTransient<IAlbumRepository, SqlAlbumRepository>();
            services.AddTransient<IAlbumValidator, AlbumValidator>();

            // Carousel
            services.AddTransient<ICarouselRepository, SqlCarouselRepository>();
            services.AddTransient<ICarouselValidator, CarouselValidator>();

            // Code snippet
            services.AddTransient<ICodeSnippetRepository, SqlCodeSnippetRepository>();

            // Contact
            services.AddTransient<IContactRepository, SqlContactRepository>();

            // Footer 
            services.AddTransient<IFooterRepository, SqlFooterRepository>();

            // Forms
            services.AddTransient<IFormRepository, SqlFormRepository>();

            // Forums
            services.AddTransient<IForumPortalService, ForumPortalService>();
            services.AddTransient<IForumRepository, SqlForumRepository>();
            services.AddTransient<IForumService, ForumService>();
            services.AddTransient<IForumAuthorizer, ForumAuthorizer>();
            services.AddTransient<IForumConfigurationService, ForumConfigurationService>();
            services.AddTransient<IForumUrlService, ForumUrlService>();
            services.AddTransient<IForumValidator, ForumValidator>();

            // HTML
            services.AddTransient<IHtmlRepository, SqlHtmlRepository>();
            services.AddTransient<IHtmlUrlService, HtmlUrlService>();
            services.AddTransient<IHtmlValidator, HtmlValidator>();

            // Latest threads
            services.AddTransient<ILatestThreadRepository, SqlLatestThreadRepository>();

            // Maps
            services.AddTransient<IMapRepository, SqlMapRepository>();

            // Nav bar
            services.AddTransient<INavBarRepository, SqlNavBarRepository>();

            // Page header
            services.AddTransient<IPageHeaderRepository, SqlPageHeaderRepository>();

            // Page list
            services.AddTransient<IPageListRepository, SqlPageListRepository>();

            // Share
            services.AddTransient<IShareRepository, SqlShareRepository>();

            // Tables
            services.AddTransient<ITableRepository, SqlTableRepository>();

            // Tag cloud
            services.AddTransient<ITagCloudRepository, SqlTagCloudRepository>();

            // Testimonials
            services.AddTransient<ITestimonialService, TestimonialService>();
            services.AddTransient<ITestimonialRepository, SqlTestimonialRepository>();

            // Dynamically created basic element services
            services.AddTransient<MasterPageFormService>();
            services.AddTransient<MasterPageZoneFormService>();
            services.AddTransient<MasterPageZonesFormService>();
            services.AddTransient<AuthenticationElementService>();
            services.AddTransient<ChangePasswordService>();
            services.AddTransient<ConfirmUserService>();
            services.AddTransient<ConfirmUserSetPasswordService>();
            services.AddTransient<CreateUserService>();
            services.AddTransient<ForgottenPasswordService>();
            services.AddTransient<LogonUserService>();
            services.AddTransient<ResetPasswordService>();
            services.AddTransient<Riverside.Cms.Elements.Pages.PageService>();
            services.AddTransient<PageZoneElementService>();
            services.AddTransient<ThemeService>();

            // Dynamically create advanced element services
            services.AddTransient<AlbumService>();
            services.AddTransient<CarouselService>();
            services.AddTransient<CodeSnippetService>();
            services.AddTransient<ContactService>();
            services.AddTransient<FooterService>();
            services.AddTransient<FormService>();
            services.AddTransient<ForumElementService>();
            services.AddTransient<HtmlService>();
            services.AddTransient<LatestThreadService>();
            services.AddTransient<MapService>();
            services.AddTransient<NavBarService>();
            services.AddTransient<PageHeaderService>();
            services.AddTransient<PageListService>();
            services.AddTransient<ShareService>();
            services.AddTransient<TableService>();
            services.AddTransient<TagCloudService>();
            services.AddTransient<TestimonialCarouselService>();
            services.AddTransient<TestimonialService>();

            // Dynamically created form services
            services.AddTransient<AuthenticationFormService>();
            services.AddTransient<ChangePasswordFormService>();
            services.AddTransient<ConfirmUserFormService>();
            services.AddTransient<ConfirmUserSetPasswordFormService>();
            services.AddTransient<CreateUserFormService>();
            services.AddTransient<ForgottenPasswordFormService>();
            services.AddTransient<LogonUserFormService>();
            services.AddTransient<ResetPasswordFormService>();
            services.AddTransient<PageFormService>();
            services.AddTransient<ThemeFormService>();
            services.AddTransient<AlbumAdminFormService>();
            services.AddTransient<CarouselAdminFormService>();
            services.AddTransient<CodeSnippetAdminFormService>();
            services.AddTransient<FooterAdminFormService>();
            services.AddTransient<FormFormService>();
            services.AddTransient<ForumFormService>();
            services.AddTransient<HtmlAdminFormService>();
            services.AddTransient<LatestThreadAdminFormService>();
            services.AddTransient<MapAdminFormService>();
            services.AddTransient<NavBarAdminFormService>();
            services.AddTransient<PageHeaderAdminFormService>();
            services.AddTransient<PageListAdminFormService>();
            services.AddTransient<PageZoneFormService>();
            services.AddTransient<ShareAdminFormService>();
            services.AddTransient<TagCloudAdminFormService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCmsMiddleware();

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                // Page routes
                routes.MapRoute(
                    name: "HomePage",
                    template: "",
                    defaults: new { controller = "pages", action = "home" });
                routes.MapRoute(
                    name: "ReadPage",
                    template: "pages/{pageid}/{*description}", 
                    defaults: new { controller = "pages", action = "read" });
                routes.MapRoute(
                    name: "ReadPageImage",
                    template: "images/pages/{pageid}/{*description}",
                    defaults: new { controller = "pages", action = "readimage" });

                // User routes
                routes.MapRoute(
                    name: "ReadUserImage",
                    template: "images/users/{userid}/{*description}",
                    defaults: new { controller = "users", action = "readimage" });

                // Elements
                routes.MapRoute(
                    name: "ReadElementUpload",
                    template: "elements/{elementid}/uploads/{uploadid}",
                    defaults: new { controller = "elements", action = "readupload" });

                // Authentication routes
                routes.MapRoute(
                    name: "CreateUser",
                    template: "users/register",
                    defaults: new { controller = "users", action = "create" });
                routes.MapRoute(
                    name: "ConfirmUserSetPassword",
                    template: "users/activatesetpassword",
                    defaults: new { controller = "users", action = "confirmsetpassword" });
                routes.MapRoute(
                    name: "LogonUser",
                    template: "users/login",
                    defaults: new { controller = "users", action = "logon" });
                routes.MapRoute(
                    name: "LogoffUser",
                    template: "users/logout",
                    defaults: new { controller = "users", action = "logoff" });
                routes.MapRoute(
                    name: "ChangePassword",
                    template: "users/changepassword",
                    defaults: new { controller = "users", action = "changepassword" });
                routes.MapRoute(
                    name: "UpdateUser",
                    template: "users/updateprofile",
                    defaults: new { controller = "users", action = "updateuser" });
                routes.MapRoute(
                    name: "ConfirmUser",
                    template: "users/activate",
                    defaults: new { controller = "users", action = "confirm" });
                routes.MapRoute(
                    name: "ForgottenPassword",
                    template: "users/forgottenpassword",
                    defaults: new { controller = "users", action = "forgottenpassword" });
                routes.MapRoute(
                    name: "ResetPassword",
                    template: "users/resetpassword",
                    defaults: new { controller = "users", action = "resetpassword" });

                // Admin actions
                routes.MapRoute(
                    name: "AdminUpdateTheme",
                    template: "admin/themes",
                    defaults: new { controller = "pages", action = "theme" });
                routes.MapRoute(
                    name: "AdminCreatePage",
                    template: "admin/pages/create/{masterpageid}",
                    defaults: new { controller = "pages", action = "create" });
                routes.MapRoute(
                    name: "AdminUpdatePage",
                    template: "admin/pages/update/{pageid}",
                    defaults: new { controller = "pages", action = "update" });
                routes.MapRoute(
                    name: "AdminCreateMasterPage",
                    template: "admin/masterpages/create",
                    defaults: new { controller = "pages", action = "createmasterpage" });
                routes.MapRoute(
                    name: "AdminUpdateMasterPage",
                    template: "admin/masterpages/{masterpageid}",
                    defaults: new { controller = "pages", action = "updatemasterpage" });
                routes.MapRoute(
                    name: "AdminUpdateMasterPageZone",
                    template: "admin/masterpages/{masterpageid}/masterpagezones/{masterpagezoneid}",
                    defaults: new { controller = "pages", action = "updatemasterpagezone" });
                routes.MapRoute(
                    name: "AdminUpdateMasterPageZones",
                    template: "admin/masterpages/{masterpageid}/masterpagezones",
                    defaults: new { controller = "pages", action = "updatemasterpagezones" });
                routes.MapRoute(
                    name: "AdminUpdatePageZone",
                    template: "admin/pages/{pageid}/pagezones/{pagezoneid}",
                    defaults: new { controller = "pages", action = "updatepagezone" });
                routes.MapRoute(
                    name: "AdminUpdatePageElement",
                    template: "admin/pages/{pageid}/elements/{elementid}",
                    defaults: new { controller = "pages", action = "updatepageelement" });
                routes.MapRoute(
                    name: "AdminUpdateMasterPageElement",
                    template: "admin/masterpages/{masterpageid}/elements/{elementid}",
                    defaults: new { controller = "pages", action = "updatemasterpageelement" });
            });
        }
    }
}
