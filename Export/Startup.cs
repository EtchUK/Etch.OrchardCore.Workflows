using Etch.OrchardCore.Workflows.Export.Services;
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

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaControllerRoute(
                name: "WorkflowExport",
                areaName: "Etch.OrchardCore.Workflows",
                pattern: "Admin/Workflows/Export",
                defaults: new { controller = "Export", action = "Index" }
            );

            routes.MapAreaControllerRoute(
                name: "WorkflowExportPreview",
                areaName: "Etch.OrchardCore.Workflows",
                pattern: "Admin/Workflows/Export/{id}/Preview",
                defaults: new { controller = "Export", action = "Preview" }
            );
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IExportService, ExportService>();
            services.AddScoped<INavigationProvider, AdminMenu>();
            services.AddScoped<IPermissionProvider, Permissions>();
        }

        #endregion Implementation
    }
}
