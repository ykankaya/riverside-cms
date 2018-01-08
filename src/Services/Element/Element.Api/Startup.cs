using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Riverside.Cms.Services.Element.Domain;
using Riverside.Cms.Services.Element.Infrastructure;
using Swashbuckle.AspNetCore.Swagger;

namespace Element.Api
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
            services.AddTransient<IElementService, ElementService>();
            services.AddTransient<IElementRepository, SqlElementRepository>();

            services.AddTransient<IElementService<PageHeaderElementSettings>, PageHeaderElementService>();
            services.AddTransient<IElementRepository<PageHeaderElementSettings>, SqlPageHeaderElementRepository>();
        }

        private void ConfigureOptionServices(IServiceCollection services)
        {
            services.Configure<SqlOptions>(Configuration);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Element HTTP API", Version = "v1" });
            });

            ConfigureDependencyInjectionServices(services);
            ConfigureOptionServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger()
              .UseSwaggerUI(c =>
              {
                  c.SwaggerEndpoint("/swagger/v1/swagger.json", "Element HTTP API v1");
              });
        }
    }
}
