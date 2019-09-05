using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;
using System;

namespace Etch.OrchardCore.Workflows.Export
{
    [Feature(Constants.Features.Export)]
    public class Startup : StartupBase
    {

        #region Implementation

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaRoute(
                name: "WorkflowExport",
                areaName: "Etch.OrchardCore.Workflows",
                template: "Admin/Workflows/Export",
                defaults: new { controller = "Export", action = "Index" }
            );
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<INavigationProvider, AdminMenu>();
            services.AddScoped<IPermissionProvider, Permissions>();
        }

        #endregion Implementation
    }
}
