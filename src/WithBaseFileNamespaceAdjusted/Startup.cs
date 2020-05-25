using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using WithBaseFileNamespaceAdjusted.Resources;

namespace WithBaseFileNamespaceAdjusted
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            
            services
                .Configure<RequestLocalizationOptions>(options =>
                {
                    var cultures = new[]
                    {
                        new CultureInfo("en"),
                        new CultureInfo("pt")
                    };
                    options.DefaultRequestCulture = new RequestCulture("en");
                    options.SupportedCultures = cultures;
                    options.SupportedUICultures = cultures;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            
            app.UseRequestLocalization(app.ApplicationServices
                .GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync(
                        context
                            .RequestServices
                            .GetRequiredService<IStringLocalizer<SharedResource>>()
                            .GetString("SampleSharedString")
                        );
                });
            });
        }
    }
}
