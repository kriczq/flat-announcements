using Flannounce.Configuration;
using Flannounce.Domain.DB;
using Flannounce.Domain.Parser;
using Flannounce.Domain.Services;
using Flannounce.Domain.Services.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
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
            services.AddControllers();
            services.Configure<FlannounceDatabaseSettings>(
                Configuration.GetSection(nameof(FlannounceDatabaseSettings)));

            services.AddSingleton<IFlannounceDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<FlannounceDatabaseSettings>>().Value);
            services.AddSingleton<IDbClient,DbClient>();
            services.AddSingleton<IAnnounceService,AnnounceService>();
            services.AddSingleton<IScrapService,ScrapService>();
            services.AddSingleton<IStreetService,StreetService>();
            services.AddTransient<IStreetParser, StreetParser>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}