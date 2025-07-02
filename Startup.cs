using EPiServer.Cms.Shell;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.DependencyInjection;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Optimizely.ContentGraph.Cms.Configuration;

namespace graphql_link;

public class Startup
{
    private readonly IWebHostEnvironment _webHostingEnvironment;

    public Startup(IWebHostEnvironment webHostingEnvironment)
    {
        _webHostingEnvironment = webHostingEnvironment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        if (_webHostingEnvironment.IsDevelopment())
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(_webHostingEnvironment.ContentRootPath, "App_Data"));

            services.Configure<SchedulerOptions>(options => options.Enabled = false);
        }

        services
            .AddCmsAspNetIdentity<ApplicationUser>()
            .AddCms()
            .AddAdminUserRegistration()
            .AddEmbeddedLocalization<Startup>();

        services.ConfigureContentApiOptions(o =>
        {
            o.IncludeInternalContentRoots = true;
            o.IncludeSiteHosts = true;
        });

        services.AddContentDeliveryApi();
        services.AddContentGraph();

        services.Configure<EventIndexingOptions>(ops =>
        {
            ops.SyncContentTypesOnInit = true;
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapContent();
        });
    }
}
