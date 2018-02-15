using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riverside.Cms.Services.Core.Client;
using Riverside.Cms.Services.Element.Client;
using RiversideCms.Mvc.Services;

namespace RiversideCms.Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private void ConfigureDependencyInjectionServices(IServiceCollection services)
        {
            // Core services
            services.AddTransient<IPageViewService, PageViewService>();

            // Element services
            services.AddTransient<ICodeSnippetElementService, CodeSnippetElementService>();
            services.AddTransient<IFooterElementService, FooterElementService>();
            services.AddTransient<IPageHeaderElementService, PageHeaderElementService>();
            services.AddTransient<IShareElementService, ShareElementService>();

            // Element factory
            services.AddTransient<IElementServiceFactory, ElementServiceFactory>();
        }

        private void ConfigureOptionServices(IServiceCollection services)
        {
            services.Configure<CoreApiOptions>(Configuration);
            services.Configure<ElementApiOptions>(Configuration);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            ConfigureDependencyInjectionServices(services);
            ConfigureOptionServices(services);
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

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute("Home", "", new { controller = "pages", action = "home" });
                routes.MapRoute("Page", "pages/{pageId}/{*description}", new { controller = "pages", action = "read" });
                routes.MapRoute("Login", "users/login", new { controller = "account", action = "login" });
                routes.MapRoute("Logout", "users/logout", new { controller = "account", action = "logout" });
                routes.MapRoute("UpdateProfile", "users/updateprofile", new { controller = "account", action = "updateprofile" });
                routes.MapRoute("ChangePassword", "users/changepassword", new { controller = "account", action = "changepassword" });
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
