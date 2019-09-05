using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;
using System;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Workflows.Export
{
    public class AdminMenu : INavigationProvider
    {
        #region Dependencies

        public IStringLocalizer T { get; set; }

        #endregion Dependencies

        #region Constructor

        public AdminMenu(IStringLocalizer<AdminMenu> localizer)
        {
            T = localizer;
        }

        #endregion Constructor

        #region Implementation

        public Task BuildNavigationAsync(string name, NavigationBuilder builder)
        {
            if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            builder.Add(T["Workflow Export"], "5.1", workflowExport => workflowExport
                .AddClass("workflows").Id("workflows-export").Action("Index", "Export", new { area = "Etch.OrchardCore.Workflows" })
                .Permission(Permissions.ExportWorkflows)
                .LocalNav());

            return Task.CompletedTask;
        }

        #endregion Implementation
    }
}
