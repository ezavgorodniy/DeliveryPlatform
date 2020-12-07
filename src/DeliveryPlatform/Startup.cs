using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using DeliveryPlatform.DataLayer.Models;
using DeliveryPlatform.HostedService;
using DeliveryPlatform.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared;
using Shared.Interfaces;
using Shared.Middlewares;
using Shared.Models;

namespace DeliveryPlatform
{
    [ExcludeFromCodeCoverage]
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
            services.AddControllers().AddJsonOptions(c =>
            {
                c.JsonSerializerOptions.IgnoreNullValues = true;
                c.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddHostedService<DeliveryExpirationBackgroundService>();

            services.Configure<AuthSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<DeliveryExpirationSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<ConnectionStringSettings>(Configuration.GetSection("AppSettings"));
            
            // This will allow to inject execution context via DI
            services.AddScoped<ExecutionContext>();
            services.AddScoped<IExecutionContext>(x => x.GetRequiredService<ExecutionContext>());

            Shared.Configuration.ConfigureServices(services);
            Core.Configuration.ConfigureServices(services);
            DataLayer.Configuration.ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
