using Etch.OrchardCore.Workflows.FormOutput.Workflows.Activities;
using Etch.OrchardCore.Workflows.FormOutput.Workflows.Drivers;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Workflows.Helpers;

namespace Etch.OrchardCore.Workflows.FormOutput
{
    [Feature(Constants.Features.FormOutput)]
    public class Startup : StartupBase
    {
        #region Implementation

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddActivity<FormOutputTask, FormOutputTaskDriver>();
        }

        #endregion Implementation
    }
}
