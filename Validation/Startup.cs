using Etch.OrchardCore.Workflows.Validation.Workflows.Activities;
using Etch.OrchardCore.Workflows.Validation.Workflows.Drivers;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Workflows.Helpers;

namespace Etch.OrchardCore.Workflows.Validation
{
    [Feature(Constants.Features.Validation)]
    public class Startup : StartupBase
    {
        #region Implementation

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddActivity<ValidateMatchingTask, ValidateMatchingFormDisplay>();
            services.AddActivity<ValidateMultipleTask, ValidateMultipleTaskDisplay>();
        }

        #endregion Implementation
    }
}
