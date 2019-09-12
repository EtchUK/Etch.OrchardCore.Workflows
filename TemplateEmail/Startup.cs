using Etch.OrchardCore.Workflows.TemplateEmail.Workflows.Activities;
using Etch.OrchardCore.Workflows.TemplateEmail.Workflows.Drivers;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Workflows.Helpers;

namespace Etch.OrchardCore.Workflows.TemplateEmail
{
    [Feature(Constants.Features.TemplateEmail)]
    public class Startup : StartupBase
    {
        #region Implementation

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddActivity<TemplateEmailTask, TemplateEmailTaskDriver>();
        }

        #endregion Implementation
    }
}
