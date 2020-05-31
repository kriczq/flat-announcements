using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Flannounce.Configuration;
using Flannounce.Domain.DB;
using Flannounce.Domain.Installers;
using Flannounce.Domain.Parser;
using Flannounce.Domain.Services;
using Flannounce.Domain.Services.Implementation;
using Flannounce.Domain.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using AutoMapper;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Flannounce
{
    public class Startup
    {public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddConfiguration(configuration);
            
            Configuration = configurationBuilder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
          services.InstallServicesInAssembly(Configuration);
          services.AddControllers();
          services.Configure<FlannounceDatabaseSettings>(
                Configuration.GetSection(nameof(FlannounceDatabaseSettings)));
          services.AddSwaggerGen();
          services.AddAutoMapper(typeof(Startup));
          
          services.AddSingleton<IFlannounceDatabaseSettings>(sp =>sp.GetRequiredService<IOptions<FlannounceDatabaseSettings>>().Value);
          services.AddSingleton<IDbClient,DbClient>();
          services.AddSingleton<IAnnounceService,AnnounceService>();
          services.AddSingleton<IScrapService,ScrapService>();
          services.AddSingleton<IStreetService,StreetService>();
          services.AddTransient<IStreetParser, StreetParser>();
          services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
          services.AddSingleton<IUriService>(provider =>
          {
              var accessor = provider.GetRequiredService<IHttpContextAccessor>();
              var request = accessor.HttpContext.Request;
              var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), "/");
              return new UriService(absoluteUri);
          });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper)
        {
            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();
            
            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });

            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
            });
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}